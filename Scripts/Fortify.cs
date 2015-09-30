//-*- java -*-
using UnityEngine;

public class Fortify : MonoBehaviour{

    public PlayerControl playerCtrl; 

    public string hotkey; 

    public GameObject barrierPrefab; 

    public ManaManager playerMana; 
    public float manaCost; 


    void Update(){
	if(playerCtrl.isControllable){
	    if(Input.GetButtonDown(hotkey) && playerCtrl.IsGrounded() && playerMana.CheckManaCost(manaCost)){
		playerMana.SubtractCost(manaCost); 

		GameObject controlScripts = GameObject.Find("ControlScripts"); 
		SynchedObjectCreator creator = controlScripts.GetComponent<SynchedObjectCreator>(); 

	        creator.photonView.RPC("Create",PhotonTargets.All,
				       (Vector2)this.transform.position,
				       Quaternion.identity,
				       barrierPrefab.GetComponent<PrefabID>().GetPrefabID()); 
		}
	}
    }
       
}
