//-*- java -*-
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    [HideInInspector]
	public double creationTime; 
    [HideInInspector]
	public int projectileID; 
    [HideInInspector]
	public RangedAttack projectileManager; 
    [HideInInspector]
	public Vector3 startPos;
    [HideInInspector]
	public Vector2 dir; 


    public float gravityScale; 
    public float speed; 
    public float lifeTime; 

    public GameObject explosion;		// Prefab of explosion effect.
    public AbObjectEffect myCollisionEffect; 


	void Start () 
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		//Destroy(gameObject, lifeTime);
	}
	
	void Update(){
	    float timePassed = (float)(PhotonNetwork.time - creationTime);
	    transform.position = this.startPos + (Vector3)( (dir*speed) ) * timePassed;
	    transform.position = (Vector2)transform.position - new Vector2(0,gravityScale) * timePassed * timePassed; 
	    Debug.Log(timePassed); 
	    
	    //Don't need to synchronize this destruction as it should occur 
	    //on all clients regardless
	    if(timePassed > lifeTime) 
		this.projectileManager.RemoveProjectile(projectileID,gameObject.transform.position,null); 
	}


	public void OnExplode(Vector2 position){
	    if(explosion!=null){
	    Quaternion quaternion = Quaternion.Euler(0f, 0f, Mathf.Tan(dir.y/dir.x)); 
	    Instantiate(explosion, position, quaternion); 
	    }

	    Destroy (gameObject);
	}

	
	//check if lag has called bullet to spawn past a target it should have
	//collided with 
	public void CheckSpawnCollision(double curTime){
	    //check if projectile might have hit the player on spawn position
	    //Need to RayCast ALL to avoid having spawned projectile hitting 
	    //the player which spawned it 
	    RaycastHit2D[] hits = Physics2D.RaycastAll( (Vector2)startPos, dir, (float)(speed * (curTime - creationTime)) ); 

	    for(int i = 0; i<hits.Length; i++){
		if(hits[i]!=null && hits[i].transform!=null){
		    GameObject hitObject = hits[i].transform.gameObject; 

		    Debug.Log(projectileManager.photonView.isMine); 

		    if(hitObject==this.gameObject){
			//Do nothing if raycasts on itself!!!! 
		    }
		    else if(hitObject.tag == "Player"){
			PlayerNetwork player = hitObject.GetComponent<PlayerNetwork>(); 
			if(player.photonView.isMine && !projectileManager.photonView.isMine){
			myCollisionEffect.EffectGO(hitObject,
				   (Vector2)this.transform.position,dir); 
			this.projectileManager.TellMeProjectileHit(
			           this.projectileID); 
			    
			}
		    }
		    else if(hitObject.tag == "Untagged" || hitObject.tag == "Explosion"){

		    }
		    //COLLISION TYPES 
		    else{
			myCollisionEffect.EffectGO(hitObject,
				   (Vector2)this.transform.position,dir); 
			this.projectileManager.TellMeProjectileHit(
			           this.projectileID); 
		    }
		}
	    }
	}
	//Runs when collides with enemy 
	void OnTriggerEnter2D (Collider2D col) {

	    if(col.gameObject.tag == "Player"){
			PlayerNetwork player = col.gameObject.GetComponent<PlayerNetwork>(); 
			if(player.photonView.isMine && !projectileManager.photonView.isMine){
			myCollisionEffect.EffectGO(col.gameObject,
				   (Vector2)this.transform.position,dir); 
			this.projectileManager.TellMeProjectileHit(
			           this.projectileID); 
			    
			}
	    }
	    else if(col.gameObject.tag == "Untagged" || col.gameObject.tag == "Explosion"){

	    }
	    else{
	    myCollisionEffect.EffectGO(col.gameObject,(Vector2)this.transform.position,dir); 
	    this.projectileManager.TellMeProjectileHit(this.projectileID);
	    }


	    /*
	    if(col.gameObject.layer == LayerMask.NameToLayer("Environment") ||
	       col.gameObject.tag == "Player"){
		myCollisionEffect.EffectGO(col.gameObject,(Vector2)this.transform.position,dir); 
		this.projectileManager.TellMeProjectileHit(this.projectileID);
	    }
	    else{
		Debug.Log("WARNING, BaseProjectile.OnTriggerEnter2D unhandled collision type"); 
	    }
	    */
	}
}
