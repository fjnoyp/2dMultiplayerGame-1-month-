//-*-java-*-
/*
Simple script to activate/deactivate being able to move through a platform

Allows for one-way colliders 
*/

using UnityEngine;
using System.Collections;

public class JumpThroughTrigger : MonoBehaviour {

public PolygonCollider2D platform;

void Start(){
     platform.isTrigger = false; 
}

void OnTriggerEnter2D(Collider2D other){
     GameObject otherGO = other.gameObject; 
     if(otherGO.tag == "Player" && 
     otherGO.GetComponent<PlayerNetwork>().photonView.isMine)     
          platform.isTrigger = true; 
}

void OnTriggerExit2D(Collider2D other){
     GameObject otherGO = other.gameObject; 
     if(otherGO.tag == "Player" && 
     otherGO.GetComponent<PlayerNetwork>().photonView.isMine)     
          platform.isTrigger = false; 
}

}
