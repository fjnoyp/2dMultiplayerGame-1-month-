using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform toFollow;

	void Awake(){
	toFollow = this.transform;
	}

	public void Follow(Transform toFollow){
	this.toFollow = toFollow; 
	}	
	void Update(){
	this.transform.position = new Vector3(toFollow.position.x, toFollow.position.y, -2); 
	}
}

