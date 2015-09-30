//-*-java-*-
using UnityEngine;
using System.Collections;

//Allows 2D box collider "killzone" to remove any gameobjects that enter it 
//Resets game. 

public class Remover : MonoBehaviour
{
	public GameObject splash;
	//ISSUE THIS IS NOT WORKING !!! CORRECTLY AS IT RELOADS THE SCENE 

	void OnTriggerEnter2D(Collider2D col)
	{
	    /*
		// If the player hits the trigger...
	    if(col.gameObject.tag == "Player"){
		// ... instantiate the splash where the player falls in.
		Instantiate(splash, col.transform.position, transform.rotation);
		// hide the dead player for a bit 
		SpriteRenderer[] spr = col.gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer s in spr){
		    Color color = s.color; 
		    color.a = 0; 
		    s.color = color; 
		}

		// ... reload the level.

		}
		//Else, code behavior for enemy falling in 
		*/

	}

	/*
	StartCoroutine("ReloadGame");
	IEnumerator ReloadGame()
	{			
		// ... pause briefly
		yield return new WaitForSeconds(2);
		// ... and then reload the level.
		Application.LoadLevel(Application.loadedLevel);
	}
	*/
}
