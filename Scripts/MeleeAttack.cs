//-*- java -*-
using UnityEngine;
using System.Collections;

using Enums; 

//Manages primary attack 
//Self contained class for managing animation, sound, and logic of a melee 
//attack

public class MeleeAttack : MonoBehaviour
{
    // Reference to the PlayerControl script.
    public PhotonView photonView; 
    public PlayerControl playerCtrl;		
    public Animator anim;

    public float creationDelay; 
    public float knockback;

    public float meleeLength; 

    public float damage; 

    public GameObject debugMelee; 

    void Awake(){
    }

    void Update (){
	if(playerCtrl.isControllable){
	    // If the fire button is pressed...
	    if(Input.GetButtonDown("Fire2")){
		StartCoroutine(DoMeleeAttack()); 
	    }
	}
    }

    [RPC] 
	void AnimMeleeAttack(bool facingRight){
	if(facingRight && !playerCtrl.facingRight){
	    playerCtrl.Flip(); 
	}
	else if(!facingRight && playerCtrl.facingRight){
	    playerCtrl.Flip(); 
	}
	anim.SetTrigger("Melee"); 
    }

    IEnumerator DoMeleeAttack(){
	yield return new WaitForSeconds( this.creationDelay ); 

	Vector2 dir = Camera.main.
	    ScreenToWorldPoint(Input.mousePosition) - 
	    transform.position; 
	if(dir.x > 0){
	    if(!playerCtrl.facingRight){
		playerCtrl.Flip(); 
	    }
	}
	else{
	    if(playerCtrl.facingRight){
		playerCtrl.Flip(); 
	    }
	}
	anim.SetTrigger("Melee"); 

	//animate melee attack on other clients 
	this.photonView.RPC("AnimMeleeAttack",PhotonTargets.Others,playerCtrl.facingRight); 

	//hitbox

	//Vector2 distX = new Vector2(transform.position.x - playerCtrl.transform.position.x,0);
	RaycastHit2D hit = Physics2D.Raycast( (Vector2)transform.position, new Vector2(dir.x,0), meleeLength); 

	/*
	if(dir.x > 0)
	    Debug.DrawRay((Vector2)transform.position,new Vector2(meleeLength,0),Color.black,1f,false);
	else
	    Debug.DrawRay((Vector2)transform.position,new Vector2(-meleeLength,0),Color.black,1f,false); 
	*/
				
	//Note exception: unlike hit client who determines if hit, 
	//melee attacker determines who's hit here 
	if(hit!=null && hit.transform!=null){
	    GameObject hitObject = hit.transform.gameObject; 
	    Debug.Log("here1"); 
	    if( hitObject.tag == "Player" ){
		Debug.Log("here2"); 
		if( dir.x > 0 ) dir.x = 1; 
		else dir.x = -1; 

		hitObject.GetComponent<PlayerNetwork>().photonView.RPC("ApplyForceDamage",PhotonTargets.Others, dir*knockback, damage );  
	    }
	}


	


	//debugMeleeBox.SetActive(false); 

	//photonView.RPC("DoMeleeAttack",PhotonTargets.All);
    }
}
