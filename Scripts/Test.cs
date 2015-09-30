using UnityEngine; 

public class Test : MonoBehaviour{
       public PolygonCollider2D ourPolygon;
       public void Start(){
       Vector2[] points = new Vector2[4];	
       points[0] = new Vector2(0,0);
       points[1] = new Vector2(10,0);
       points[2] = new Vector2(10,10); 
       points[3] = new Vector2(0,10); 
       ourPolygon.SetPath(0,points); 
}
public void Update(){
       Vector2[] points = ourPolygon.GetPath(0);
       for(int i = 0 ;i<points.Length; i++){
       Debug.Log("point: " + points[i].x + " " + points[i].y); 
}
}
}