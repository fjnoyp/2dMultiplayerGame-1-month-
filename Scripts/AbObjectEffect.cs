//-*-java-*-
using UnityEngine; 

public abstract class AbObjectEffect : MonoBehaviour {
    abstract public void EffectGO(GameObject gObj, 
				   Vector2 pos, Vector2 velocity); 

    //abstract public void ExplosionAt(Vector2 position, float rotation); 
    /*
    abstract public void EffectPlayer(GameObject gObj, 
				      Vector2 pos, Vector2 velocity); 
    abstract public void EffectEnvironment(GameObject gObj);
    */
}

