//-*- java -*-
using UnityEngine;
using System.Collections; //just for IEnumerator
using System.Collections.Generic; //just for List<T>

using Enums; 

//Manages primary attack 
//Self contained class for managing animation, sound, and logic of a fireball
//attack

public class RangedAttack : MonoBehaviour{

	//External control variables 
    public PhotonView photonView; 
	public PlayerControl playerCtrl;		
	public Animator anim; 

	public string actionAnim; 
	public string hotkey; 

	public float coolDown;
	private float nextFire = 0; 

	public GameObject projectile; //projectile prefab to generate		


	public ManaManager playerMana; 
	public float manaCost; 

	//Public behavior variables 
	private float spawnDist; 

	//public AudioClip shootSound; 
	



	//public float projectileSpeed; 
	//public float projectileLifetime; 
	//public int projectileKnockback; 
	//public int projectileDamage; 

	//RPC synched projectiles 
	public float creationDelay; //to allow for better client synch

	[HideInInspector]
	private List<BaseProjectile> allProjectiles = new List<BaseProjectile>(); 
	private int lastProjectileID = 0; 

	void Awake(){
	    this.spawnDist = Mathf.Abs(((Vector2)this.transform.position - (Vector2)playerCtrl.transform.position).magnitude); 
	}
	void Update(){
	    if(playerCtrl.isControllable){
		// If the fire button is pressed...
		if(Input.GetButtonDown(this.hotkey) && Time.time > nextFire && playerMana.CheckManaCost(manaCost)){

		    playerMana.SubtractCost(manaCost); 

		    nextFire = Time.time + coolDown; 
		    StartCoroutine(DoRangedAttack()); 
		    //photonView.RPC("DoRangedAttack",PhotonTargets.All);
		}
	    }
	}

	//RangedAttack will not explicitly manage the Projectile it creates
	//but the Projectiles will notify RangedAttack to delete itself 
	//when it collides with something 

        IEnumerator DoRangedAttack(){
	    //Find shoot from position 
	    Vector2 dir = Camera.main.
		ScreenToWorldPoint(Input.mousePosition) - 
		transform.position; 
	    dir.Normalize(); 
	    Vector2 pos = (Vector2)playerCtrl.transform.position + (dir * spawnDist);

	    //Create projectile, avoid synch issues by only spawning projectile
	    //100 ms after firing, this way the projectile we see isn't
	    //100 ms ahead when other players receive the RPC call 
	    
	    //Note speed is constant, stored among clients 
	    photonView.RPC("CreateProjectile",PhotonTargets.Others,new object[]{pos,dir,lastProjectileID});
	    yield return new WaitForSeconds( this.creationDelay ); 
	    this.CreateProjectile(pos,dir,lastProjectileID,PhotonNetwork.time); 

	}

	void CreateProjectile(Vector2 position, Vector2 dir, int projectileID, double timestamp){
	    //Shoot sound 

	    //AudioSource.PlayClipAtPoint(shootSound, position); 

	    //check if projectileID was destroyed before we got around 
	    //to creating a bullet with that id
	    if(projectileID >= this.lastProjectileID){
	    anim.SetTrigger(this.actionAnim); 

	    GameObject projectileInstance = Instantiate(projectile, (Vector3)position, Quaternion.Euler(new Vector3(0,0,Mathf.Rad2Deg*Mathf.Atan2(dir.y,dir.x)))) as GameObject; 
	    //projectileInstance.rigidbody2D.velocity = velocity; 

	    BaseProjectile baseProjectile = projectileInstance.GetComponent<BaseProjectile>(); 
	    baseProjectile.speed = baseProjectile.speed + playerCtrl.rigidbody2D.velocity.magnitude; 

	    baseProjectile.startPos = (Vector3)position; 
	    baseProjectile.dir = dir; 
	    baseProjectile.creationTime = timestamp; 
	    baseProjectile.projectileID = projectileID; 
	    baseProjectile.projectileManager = this; 

	    //else add to allProjectiles 
	    allProjectiles.Add(baseProjectile); 
	    lastProjectileID ++; 

	    //check spawn tunneling case 
	    baseProjectile.CheckSpawnCollision( PhotonNetwork.time); 
	    }
	}

	[RPC]
	    void CreateProjectile(Vector2 position, Vector2 dir, int projectileID, PhotonMessageInfo info){
	    CreateProjectile(position,dir,projectileID,info.timestamp); 
	}
			   
	//Called by spawned projectiles when they hit something 
	public void TellMeProjectileHit(int projectileID){
	    BaseProjectile theProjectile = allProjectiles.Find( item => item.projectileID == projectileID);

	    //null if another client deleted the projectile before
	    //and we deleted that but then registered a collision on our
	    //own instnace before deleting said projectile 
	    if(theProjectile!=null)
	    photonView.RPC("RemoveProjectile",
			   PhotonTargets.All,
			   projectileID,
			   (Vector2)theProjectile.gameObject.
			   transform.position); 
	}

	[RPC]
	    public void RemoveProjectile(int projectileID, Vector2 position, 
					 PhotonMessageInfo info){
	    //Destroy projectile 
	    BaseProjectile baseProjectile = allProjectiles.Find( item => item.projectileID == projectileID);
	    if(baseProjectile!=null){
		baseProjectile.OnExplode( position ); 
		allProjectiles.Remove(baseProjectile); 
	    }
	    //NOTE: check if we accidentally remove a projectile before 
	    //one of our clients gets around to creating that same 
	    //projectile 
	    if(this.lastProjectileID <= projectileID)
		this.lastProjectileID = projectileID + 1; 
	}
}
