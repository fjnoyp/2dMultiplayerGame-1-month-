//-*- java -*- 
using UnityEngine;

public class Matchmaker : Photon.MonoBehaviour
{
    GameObject playerCharacterType; 

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {

	Vector3 spawnPos = GameObject.Find("SpawnPositions").GetComponent<PlayerSpawnManager>().GetRandomSpawnPos();
        GameObject hero = PhotonNetwork.Instantiate("WizardPrefab", spawnPos, Quaternion.identity, 0);
        hero.GetComponent<PlayerControl>().isControllable = true;

	//Assign player ID , UNUSED 
	hero.GetComponent<PlayerNetwork>().photonView.RPC("NotifyAssignPlayerID", PhotonTargets.All);

	//Setup Camera tracking 
	Camera camera = Camera.main; 
	camera.gameObject.GetComponent<CameraFollow>().Follow(hero.transform); 

	//Setup HurtUI 
	//if(hero.GetComponent<PlayerNetwork>().photonView.isMine){
	PlayerHealth playerHealth = hero.GetComponent<PlayerHealth>();
	playerHealth.hurtUI = GameObject.Find("HurtUI"); 
	playerHealth.Initialize(); 
	
	//Setup ManaUI
	ManaManager playerMana = hero.GetComponent<ManaManager>(); 
	playerMana.manaBarUI = GameObject.Find("ManaUI"); 
	playerMana.Initialize(); 

	PlayerNetwork pNet = hero.GetComponent<PlayerNetwork>(); 

	//hero.GetComponent<PlayerNetwork>().Die(); 

	    //}
    }

}

//ISSUE ISSUE ISSUE 
// jumping not registering ...
// hits forcing player to look in wrong direction(resolve with additional bool face dir) 
// bullets seem to be tunneling through still, I think they're resolving but sine they're resolved on the hit client's side it looks like they're going through. 
