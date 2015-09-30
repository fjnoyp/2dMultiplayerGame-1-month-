
//-*-java-*-
using UnityEngine;
using System.Collections.Generic; 

/*
Script for generating objects across all clients 
*/


public class SynchedObjectCreator : MonoBehaviour{
    public PhotonView photonView; 
    public GameObject[] allPrefabs; 
    private Dictionary<int,GameObject> prefabDict; 

    void Start(){
	prefabDict = new Dictionary<int,GameObject>(); 
	for(int i = 0; i<allPrefabs.Length; i++){
	    prefabDict.Add( allPrefabs[i].GetComponent<PrefabID>().GetPrefabID(),
			    allPrefabs[i] ); 
	}
    }

    [RPC] 
    public void Create(Vector2 pos, Quaternion rotation, int objectID){
	GameObject toCreate; 
	prefabDict.TryGetValue(objectID,out toCreate); 

	GameObject newOBJ = Instantiate(toCreate,pos,rotation) as GameObject; 

	//CHEATING DUE TO CHEAT IN PENETRATE OBJECT EFFECT
	if(objectID==1){
	    Destroy(newOBJ.GetComponent<PrefabID>()); 
	}
    }
}
