//-*-java-*-
using UnityEngine; 

public abstract class AbObjectVisualEffect : MonoBehaviour {
    abstract public void StartEffect(GameObject gObj, Vector2 pos, Vector2 dir);

    //abstract public void ExplosionAt(Vector2 position, float rotation); 
    /*
    abstract public void EffectPlayer(GameObject gObj, 
				      Vector2 pos, Vector2 velocity); 
    abstract public void EffectEnvironment(GameObject gObj);
    */
}

