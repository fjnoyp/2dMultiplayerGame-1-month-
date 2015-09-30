using UnityEngine; 
public class SetRenderLayer : MonoBehaviour{
       public string desiredLayer;   
public string component; 

       void Start(){
       if(component == "ParticleSystem"); 
       particleSystem.renderer.sortingLayerName = desiredLayer; 
}
}