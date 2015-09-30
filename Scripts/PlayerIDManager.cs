//-*-java-*-
using UnityEngine; 
using System.Collections.Generic; 

public class PlayerIDManager : MonoBehaviour {
    private Dictionary<double,GameObject> playerDict = new Dictionary<double,GameObject>(); 
	
    [RPC]
	public void AssignPlayerID(GameObject gObj, double timestamp){
	playerDict.Add(timestamp,gObj); 
	Debug.Log("Added:" + timestamp); 
    }

    public GameObject GetPlayerFromID(int playerID){
	GameObject foundObject; 
	playerDict.TryGetValue(playerID, out foundObject ); 
	return foundObject; 
    }
}
