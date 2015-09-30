//-*-java-*-
using UnityEngine;

public class EffectObjectsInArea : MonoBehaviour{
    public AbObjectEffect myCollisionEffect; 
    //public PolygonCollider2D ourPolygon; 

    void OnTriggerEnter2D(Collider2D col){
	Vector2 colPosition = col.gameObject.transform.position; 

	myCollisionEffect.EffectGO(col.gameObject,this.transform.position,(Vector2)colPosition - (Vector2)transform.position); 

	//myCollisionEffect.EffectGO(col.gameObject,(Vector2)colPosition,(Vector2)colPosition - (Vector2)transform.position); 

	
	/*
	if(col.gameObject.tag == "Player"){
	    myCollisionEffect.EffectPlayer(col.gameObject,(Vector2)this.transform.position, col.gameObject.transform.position - this.transform.position ); 
	}
	else if(destroyEnviro && col.gameObject.tag == "DestEnviro"){
	    col.gameObject.GetComponent<SDestructable>().ExplosionCut(ourPolygon.points,(Vector2)this.transform.position); 
	}
	*/
    }
}
