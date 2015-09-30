//-*- java -*-
using UnityEngine;

public class SDestructibleNature : MonoBehaviour{
    public BoxCollider2D fallenCollider; 
    public CircleCollider2D initialCollider;
    public Rigidbody2D rigidBody; 

    public AudioClip[] fallingSounds; 
    
    void Start(){
	if(fallenCollider!=null)
	fallenCollider.enabled = false; 
    }

    void OnTriggerEnter2D (Collider2D col) {

	if(col.gameObject.tag=="Explosion"){
	    if(fallenCollider!=null){
		
		if(fallingSounds.Length!=0)
		AudioSource.PlayClipAtPoint(fallingSounds[Random.Range(0,fallingSounds.Length)], 
					    transform.position); 

		Destroy(initialCollider); 
		fallenCollider.enabled = true; 
		rigidBody.isKinematic = false; 
		gameObject.layer = LayerMask.NameToLayer("NPCollisionObjects");
	    }
	    else{
		Destroy(this.gameObject); 
	    }


	}
    }
}
