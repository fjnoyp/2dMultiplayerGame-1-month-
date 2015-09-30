//-*-java-*-
using UnityEngine;

public class FallThroughTrigger : MonoBehaviour{
    public PolygonCollider2D platform; 

    void OnCollisionStay2D(Collision2D other){
	GameObject otherGO = other.gameObject; 
	if(otherGO.tag == "Player" && 
	   otherGO.GetComponent<PlayerNetwork>().photonView.isMine && otherGO.GetComponent<PlayerControl>().movingDown){
	    Debug.Log("here"); 
	platform.isTrigger = true; 
	}
    }

    /*
    void OnTriggerExit2D(Collider2D other){
	GameObject otherGO = other.gameObject; 
	if(otherGO.tag == "Player" && 
	   otherGO.GetComponent<PlayerNetwork>().photonView.isMine)     
	    platform.isTrigger = false; 
    }
    */


}

