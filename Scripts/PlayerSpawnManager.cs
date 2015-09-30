//-*- java -*- 
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour{
    
    public GameObject[] spawnPos; 

    public Vector2 GetRandomSpawnPos(){
	return (Vector2)spawnPos[ Random.Range(0,spawnPos.Length) ].transform.position; 
    }
}
