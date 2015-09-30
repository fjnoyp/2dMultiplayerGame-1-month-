//-*- java -*- 
using UnityEngine;

using Enums; 

using System.Collections;

//Synchronizes PlayerControl (player movement and position) among clients, serves as "hub" class for synching a player among all clients? 
public class PlayerNetwork : Photon.MonoBehaviour{

    public Animator anim; //to play death anim 

   public PlayerControl playerControl; //Used for getting and storing velocity , TO REMOVE:velocity synchronization, make it so characters just block movement ... don't really need anything else , 


   private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
   private Vector2 correctPlayerVel = Vector2.zero; 

   private double PunLastTimeStamp = 0; 
   private double PunUpdateRate = 0; 
   private int increment = 0; 

   public double myPlayerID; //quick ID for identifying player over network 

   public double interpolationBackTime = 0.15;
    // We store twenty states with "playback" information
    State[] m_BufferedState = new State[20];
    // Keep track of what slots are used
    int m_TimestampCount;

    public PlayerHealth myHealth; 


   internal struct State
    {
        internal double timestamp;
        internal Vector2 pos;
        internal Vector2 velocity; //currently only used for animation
	//should switch to an int ??? 
    }

   void Start(){
       if (!photonView.isMine){
	   //turn off, gravity managed by other player 
	   rigidbody2D.gravityScale = 0; 
	   rigidbody2D.isKinematic = true; 
	   //GetComponent<PlayerHealth>().enabled = false; 
       }
   }

    void CharacterCollision(Collision2D col){
	if(photonView.isMine && col.gameObject.tag == "Player")
	    col.gameObject.GetComponent<PlayerNetwork>().photonView.RPC("ApplyForce", PhotonTargets.Others, playerControl.velocity * 10); 
    }

    void OnCollisionEnter2D(Collision2D col){
	CharacterCollision(col);
    }
    void OnCollisionStay2D(Collision2D col){
	CharacterCollision(col); 
    }
    
    /*NOTES ON RPC SYNCH 
      - RPCs all make distinction between photonView.isMine versus not, we 
      need some synch but actual client manager has other responsibilities 
      as well 
     */

    //Synchronize a force applied to this character instance from another client
    [RPC] 
    void ApplyForce(Vector2 force){
	//implicit if(photonView.isMine) due to character in other client 
	//being kinematic 
	rigidbody2D.AddForce(force); 
    }

    [RPC]
    void ApplyForceDamage(Vector2 force, float healthDamage){
	rigidbody2D.AddForce(force);
	if(photonView.isMine) //Player's manage own health, not other clients
	    myHealth.DecreaseHealth(healthDamage); 
    }

    [RPC]
    void KillPlayer(){
	//if(photonView.isMine): we have this check later on in the Die()
	//method call 
	StartCoroutine(Die()); 
	anim.SetTrigger("Die"); 
    }

    public IEnumerator Die(){

	if(photonView.isMine){
	//Disable User Control 
	GetComponent<PlayerControl>().enabled = false;
	GetComponent<PlayerHealth>().enabled = false; 
	GetComponentInChildren<RangedAttack>().enabled = false;
	GetComponentInChildren<MeleeAttack>().enabled = false;
	}

	//Make body a ragdoll
	//rigidbody2D.fixedAngle = false; 

	//Let dead body lie there for a bit 
	yield return new WaitForSeconds(5); 
	Debug.Log("DIED"); //DYING TOO MANY TIMES 
	this.Respawn(); 
	/*
	//Disable colliders 
	Collider2D[] cols = GetComponents<Collider2D>();
	foreach(Collider2D c in cols){
		c.isTrigger = true;
	}
	//Disable view 
	SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
	foreach(SpriteRenderer s in spr){
	    Color color = s.color; 
	    color.a = 0; 
	    s.color = color; 
	}
	*/

    }
    public void Respawn(){
	if(photonView.isMine){
	    //Respawn Point 
	    this.transform.position = GameObject.Find("SpawnPositions").GetComponent<PlayerSpawnManager>().GetRandomSpawnPos();
	    //new Vector3(56,-19f,0); 

	//Enable User Control 
	GetComponent<PlayerControl>().enabled = true; 
	PlayerHealth health = GetComponent<PlayerHealth>(); 
	health.ResetHealth(); 
	health.enabled = true; 

	GetComponentInChildren<RangedAttack>().enabled = true; 
	GetComponentInChildren<MeleeAttack>().enabled = true; 
	}

	//Reset animation to non dying 
	photonView.RPC("ResetAnim",
		       PhotonTargets.All);
	//Unragdoll
	//rigidbody2D.fixedAngle = true; 
    }

    [RPC]
	void ResetAnim(){
	anim.SetTrigger("Reset"); 
    }


    public void SynchDeath(){
	photonView.RPC("KillPlayer",
		       PhotonTargets.All);
    }

    /* TO REDUCE BANDWIDTH consumption we can only send Vector2 force when 
       necessary (ie. when affected character is not photonView.isMine 
    void ApplyForce(Vector2 force){
	if(photonView.isMine){
	    rigidbody2D.AddForce(force); 
	}
	else{
	    photonView.RPC("SynchForce",PhotonTargets.Others,force); 
	}
    }
    void ApplyForceDamage(Vector2 force, float healthDamage){
	if(photonView.isMine){
	    rigidbody2D.AddForce(force);
	    photonView.RPC("SynchHealthChange",PhotonTargets.All,healthDamage);
	}
	else{
	    //update health 
	    photonView.RPC("SynchForceDamage",PhotonTargets.Others,new Object[]{force,healthDamage}); 
	}
    }
    */

    [RPC]
    void NotifyAssignPlayerID(PhotonMessageInfo info){
	//Need to update global converter for use in other methods 
        GameObject.Find("ControlScripts").GetComponent<PlayerIDManager>().AssignPlayerID(this.gameObject,info.timestamp);

	//Our id for identifying this object accross the network 
	this.myPlayerID = info.timestamp; 
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
	// Always send transform(depending on reliability of the network view)
        if (stream.isWriting)
        {
            Vector2 pos = (Vector2)transform.localPosition;
	    Vector2 velocity = playerControl.velocity; 
	    bool isFacingRight = playerControl.facingRight; 
            stream.Serialize(ref pos);
            stream.Serialize(ref velocity);
	    stream.Serialize(ref isFacingRight); 
        }
        // When receiving, buffer the information
        else
        {
			// Receive latest state information
            Vector2 pos = Vector2.zero;
	    Vector2 velocity = Vector2.zero;
	    bool isFacingRight = false; 
            stream.Serialize(ref pos);
	    stream.Serialize(ref velocity); 
	    stream.Serialize(ref isFacingRight); 

	    //Make proper face direction 
	    playerControl.UpdateFaceDir(isFacingRight); 

            // Shift buffer contents, oldest data erased, 18 becomes 19, ... , 0 becomes 1
            for (int i = m_BufferedState.Length - 1; i >= 1; i--)
            {
                m_BufferedState[i] = m_BufferedState[i - 1];
            }
			
			
            // Save currect received state as 0 in the buffer, safe to overwrite after shifting
            State state;
            state.timestamp = info.timestamp;
            state.pos = pos;
	    state.velocity = velocity; 
            m_BufferedState[0] = state;

            // Increment state count but never exceed buffer size
            m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);

            // Check integrity, lowest numbered state in the buffer is newest and so on
            for (int i = 0; i < m_TimestampCount - 1; i++)
            {
                if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
                    Debug.Log("State inconsistent");
            }
	}
    }

    // This only runs where the component is enabled, which is only on remote peers (server/clients)
    void Update(){
	if(!photonView.isMine){
        double currentTime = PhotonNetwork.time;
        double interpolationTime = currentTime - interpolationBackTime;
        // We have a window of interpolationBackTime where we basically play 
        // By having interpolationBackTime the average ping, you will usually use interpolation.
        // And only if no more data arrives we will use extrapolation

        // Use interpolation
        // Check if latest state exceeds interpolation time, if this is the case then
        // it is too old and extrapolation should be used
        if (m_BufferedState[0].timestamp > interpolationTime)
	    {
		for (int i = 0; i < m_TimestampCount; i++)
		    {
			// Find the state which matches the interpolation time (time+0.1) or use last state
			if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount - 1)
			    {
				// The state one slot newer (<100ms) than the best playback state
				State rhs = m_BufferedState[Mathf.Max(i - 1, 0)];
				// The best playback state (closest to 100 ms old (default time))
				State lhs = m_BufferedState[i];

				// Use the time between the two slots to determine if interpolation is necessary
				double length = rhs.timestamp - lhs.timestamp;
				float t = 0.0F;
				// As the time difference gets closer to 100 ms t gets closer to 1 in 
				// which case rhs is only used
				if (length > 0.0001)
				    t = (float)((interpolationTime - lhs.timestamp) / length);

				// if t=0 => lhs is used directly
				transform.localPosition = Vector3.Lerp( (Vector3)lhs.pos, (Vector3)rhs.pos, t);
				playerControl.velocity = Vector2.Lerp( lhs.velocity, rhs.velocity, t); 
				return;
			    }
		    }
	    }
        // Use extrapolation. Here we do something really simple and just repeat the last
        // received state. You can do clever stuff with predicting what should happen.
        else
	    {
		State latest = m_BufferedState[0];

		transform.localPosition = Vector3.Lerp(transform.localPosition, (Vector3)latest.pos, Time.deltaTime * 20 );
	    }
	}
    }

}
