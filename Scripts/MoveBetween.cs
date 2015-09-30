//-*- java -*-
using UnityEngine; 

public class MoveBetween : Photon.MonoBehaviour{
    public Vector2 start; 
    public Vector2 end; 
    public Transform toMove; 

    private Vector2 targetPos; 

    private float startPosX, startPosY; 
	
    public float speed; 

    private float difX, difY; 

    private float TwoPi = Mathf.PI * 2; 

    void Start(){
	difX = end.x - start.x; 
	difY = end.y - start.y; 

	startPosX = toMove.position.x;
	startPosY = toMove.position.y; 
    }

    void FixedUpdate(){
	float scale = Mathf.Cos(.2f * (float)PhotonNetwork.time); 
	targetPos = new Vector2( startPosX + scale*difX,
				 startPosY + scale*difY);

    }
    void Update(){
	transform.position = (Vector3)Vector2.Lerp(transform.position, targetPos, Time.deltaTime); 
    }
}
