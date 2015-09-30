//-*-java-*-
using UnityEngine;

public class KillZone : MonoBehaviour{
    void OnTriggerEnter2D(Collider2D other){
	if(other.gameObject.tag == "Player"){
	    PhotonView pView = other.gameObject.GetComponent<PlayerNetwork>().photonView;
	    if(pView.isMine)
		pView.RPC("ApplyForceDamage",PhotonTargets.All, new object[]{new Vector2(0,0),100000.0f}); 
	}
	else
	    Destroy(other.gameObject); 

    }
}
