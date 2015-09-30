//-*- java -*-
using UnityEngine;

public class Blink : MonoBehaviour{
    public PhotonView photonView; 
    public PlayerControl playerCtrl;
    public GameObject blinkTrailPrefab; 

    public string hitKey; 

    public ManaManager playerMana; 
    public float manaCost; 
   
    public AudioClip blinkSound; 

    void Update(){
	if(playerCtrl.isControllable){
	    if(Input.GetKeyDown(hitKey)&& playerMana.CheckManaCost(manaCost)){
		
		playerMana.SubtractCost(manaCost); 

		Vector2 movePos = Camera.main.
		    ScreenToWorldPoint(Input.mousePosition);
		
		Vector2 dist = movePos - (Vector2)transform.position;

		RaycastHit2D[] hits = Physics2D.RaycastAll( (Vector2)transform.position, dist, dist.magnitude); 

		/*
		Debug.DrawRay( (Vector2)transform.position,
			       dist,
			       Color.black,
			       100 ); 
		*/

		//Allow move through players and platforms 
		for(int i = 0; i<hits.Length; i++){

		    GameObject otherGO = hits[i].transform.gameObject; 
		    if( otherGO.tag == "ground"){
			//update maximum possible move distance ========
			Vector2 newDist = (Vector2)hits[i].point
			    - (Vector2)transform.position;
			
			if(dist.magnitude > newDist.magnitude){
			    dist = newDist; 
			}
		    }
		    else if( otherGO.tag == "DestEnviro"){
			//check is platform 
			if( otherGO.GetComponent<FallThroughTrigger>()
			    == null ){
			    //update maximum possible move distance ======
			    Vector2 newDist = (Vector2)hits[i].point
				- (Vector2)transform.position;

			    if(dist.magnitude > newDist.magnitude){
				dist = newDist;
			    }
			}
			
		    }
		}

		photonView.RPC("CreateFX",PhotonTargets.All,new object[]{
			(Vector2)transform.position, 
			(Vector2)transform.position + (Vector2)dist}); 

		transform.position += (Vector3)dist; 

		


/*
		if(hit != null){
		    transform.position = (Vector2)hit.point;// - dir; 
		}
		else{
		    transform.position = dir; 
		}
*/
	    }
	}
    }

    [RPC]
    void CreateFX(Vector2 start, Vector2 end){
	AudioSource.PlayClipAtPoint(blinkSound, start); 
	
	GameObject newTrail = Instantiate(this.blinkTrailPrefab,
					  transform.position,
					  Quaternion.identity) as GameObject; 
	Lerp lerp = newTrail.GetComponent<Lerp>(); 
	lerp.start = start; 
	lerp.end = end; 
	lerp.toMove = newTrail.transform; 

	StopParticleSystemAfterTime stopper = newTrail.GetComponent<
	    StopParticleSystemAfterTime>(); 
	stopper.pSystem = newTrail.GetComponent<ParticleSystem>(); 
    }
}

