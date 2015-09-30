//-*-java-*-
using UnityEngine; 

public class AudioEffectOnEnter : MonoBehaviour {

    public string[] activationTags; 
    public AudioClip[] audioClips; 

    void OnCollisionEnter2D(Collision2D coll){
	PlaySound(coll.gameObject.tag, 
		  (Vector2)coll.gameObject.transform.position); 
    }
    void OnTriggerEnter2D(Collider2D coll){
	PlaySound(coll.gameObject.tag, 
		  (Vector2)coll.gameObject.transform.position); 
    }

    private void PlaySound(string tag, Vector2 position){
	for(int i = 0; i<activationTags.Length; i++){
	    if(activationTags[i] == tag){
		AudioSource.
		    PlayClipAtPoint(audioClips[Random.Range(0,
							    audioClips.Length)],
				    position);
		return; 
	    }
	}
    }
}
