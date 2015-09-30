//-*-java-*-
using UnityEngine; 

public class HideAreaTrigger : MonoBehaviour{
    public SpriteRenderer hideArea; 
    private Color hideAreaColor; 

    private bool done = true; 
    private bool isFadingIn = false; 
    private float curAlpha = 1; 


    void Start(){
	hideAreaColor = hideArea.color; 
    }

    void OnTriggerEnter2D(Collider2D other){
	GameObject otherGO = other.gameObject; 
	if(otherGO.tag == "Player" && 
	   otherGO.GetComponent<PlayerNetwork>().photonView.isMine)     
	    FadeOutHideArea(); 
    }

    void OnTriggerExit2D(Collider2D other){
	GameObject otherGO = other.gameObject; 
	if(otherGO.tag == "Player" && 
	   otherGO.GetComponent<PlayerNetwork>().photonView.isMine)     
	    FadeInHideArea(); 
    }

    void FadeOutHideArea(){
	done = false; 
	isFadingIn = false; 
    }
    void FadeInHideArea(){
	done = false; 
	isFadingIn = true; 
    }

    void Update(){
	if(!done){
	    if(isFadingIn){
		if(curAlpha < 1) curAlpha += .1f; 
		if(curAlpha >= 1) done = true; 
	    }
	    else{
		if(curAlpha > 0) curAlpha -= .1f; 
		if(curAlpha <= 0) done = true; 
	    }
	    hideAreaColor.a = curAlpha; 
	    hideArea.color = hideAreaColor; 
	}
    }

}
