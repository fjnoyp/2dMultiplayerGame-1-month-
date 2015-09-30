using UnityEngine; 
public class FastForwardParticleSystem : MonoBehaviour{
       public float fastForwardBy; 

       void Start(){
       particleSystem.Simulate(fastForwardBy); 
       particleSystem.Play(); 
       }
}