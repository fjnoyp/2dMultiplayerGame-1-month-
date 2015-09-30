//-*-java-*-
using UnityEngine;

public class AudioTrigger : MonoBehaviour{
    public AudioSource audio;
    public Collider2D trigger; 

    
    void Start(){
	audio.Pause(); 
    }
    void OnTriggerEnter2D(Collider2D col){
	if(col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerNetwork>().photonView.isMine){
	    audio.Play(); 
	}
    }
    void OnTriggerExit2D(Collider2D col){
	if(col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerNetwork>().photonView.isMine){
	    audio.Pause(); 
	}

    }
    
}
