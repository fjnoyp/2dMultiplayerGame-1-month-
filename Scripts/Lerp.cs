//-*-java-*-
using UnityEngine; 

public class Lerp : MonoBehaviour{
    public Vector2 start; 
    public Vector2 end; 
    public float time; 
    public Transform toMove;

    private double startTime; 
    void Start(){
	startTime = Time.time; 
    }
    void Update(){
	toMove.position = (Vector3)Vector2.Lerp(start,end, (float)(Time.time-startTime)/time ); 
    }
}
