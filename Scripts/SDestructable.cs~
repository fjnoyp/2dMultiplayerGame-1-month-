//-*- java -*-
using UnityEngine; 

using ClipperLib; 

using System.Collections.Generic; //just for List<T>

public class SDestructable : MonoBehaviour{

    public Material material; 
    public PolygonCollider2D ourPolygon; 

    private Mesh mesh; 

    //public PolygonCollider2D test; 

    public void IntoLocalSpace(Vector2[] vertices){

	Vector2 ourScale = gameObject.transform.localScale; 

	//Check if map flipped 
	if(gameObject.transform.parent!=null && gameObject.transform.parent.transform.localScale.x == -1){
	    ourScale.x = ourScale.x * -1; 
	}


	for(int i = 0; i<vertices.Length; i++){
	    vertices[i] = new Vector2( vertices[i].x * ourScale.x, 
				       vertices[i].y * ourScale.y ); 
	}
    }
    public void OutLocalSpace(Vector2[] vertices){

	Vector2 ourScale = gameObject.transform.localScale; 

	//Check if map flipped 
	if(gameObject.transform.parent!=null && gameObject.transform.parent.transform.localScale.x == -1){
	    ourScale.x = ourScale.x * -1; 
	}

	for(int i = 0; i<vertices.Length; i++){
	    vertices[i] = new Vector2( vertices[i].x / ourScale.x, 
				       vertices[i].y / ourScale.y ); 
	}
    }

    //Assuming explosionPoints centered on 0,0 
    public void ExplosionCut(Vector2[] cutVertices, Vector2 worldHitPoint, ClipType clipType = ClipType.ctDifference){
	Transform ourTransform = gameObject.transform; 

	PolyClipper.OffsetVertices( cutVertices, 
	  PolyClipper.GetOffset((Vector2)ourTransform.position,
				worldHitPoint));

	//IF YOU MAKE NON KINEMATIC, PROBLEM: 
	//Moved Relative to PARENT, NOT ROTATED RELATIVE TO PARENT 
	//Polygon points are all moved relative to parent GO's transform 
	Vector2[] ourVertices = ourPolygon.points; 
	IntoLocalSpace(ourVertices); 

	//  public enum ClipType { ctIntersection, ctUnion, ctDifference, ctXor };
	List<Vector2[]> cutPolygons = PolyClipper.ClipPoly(ourVertices,cutVertices,clipType); 

	if(cutPolygons.Count == 0){
	    Destroy(this.gameObject);
	    return; 
	}

	OutLocalSpace(cutPolygons[0]);
	UpdateShape( cutPolygons[0] ); 

	for(int i = 1; i<cutPolygons.Count; i++){
	    //OutLocalSpace(cutPolygons[i]);
	    CreateCopy( cutPolygons[i] ); 
	}
	
    }

    /*
    //Normalize points
    private Vector2 GetPointsCenter(Vector2[] points){
	float centerX = 0;
	float centerY = 0; 
	for(int i = 0; i<points.Length; i++){
	    centerX += points[i].x;
	    centerY += points[i].y;
	}
	return new Vector2( centerX / (float)points.Length,
			    centerY / (float)points.Length ); 
    }
    private void PointCenterToOrigin(Vector2[] points, Vector2 center){
	for(int i = 0; i<points.Length; i++){
	    points[i] = center - points[i];
	}
    }
    */

    // MANAGING POLY CUTS and SHAPE REGENERATION =============================

    //Create a new instance of this GameObject with "points" 
    private void CreateCopy(Vector2[] points){
	GameObject newCopy = Instantiate(this.gameObject,this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject; 
	//PointCenterToOrigin(points,GetPointsCenter(points));
	newCopy.GetComponent<SDestructable>().UpdateShape( points ); 
	
	//FOR FLIPPED MAP: ASSIGN PARENT 
	newCopy.gameObject.transform.parent = this.gameObject.transform.parent;
    }

    private void UpdateShape(Vector2[] newPoints){

	// Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(newPoints);
        int[] indices = tr.Triangulate();

	//Create the new PolygonCollider2D
	ourPolygon.SetPath(0,newPoints); 

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[newPoints.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(newPoints[i].x, newPoints[i].y, 0);
        }

        // Create the New Mesh 
	this.mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
	//Set up texture coordinate for New Mesh 
        Vector2[] uvs = new Vector2[vertices.Length];
	for(int i = 0; i<uvs.Length; i++){
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
	}
	mesh.uv = uvs; 

        // Set up game object with mesh;
	if(GetComponent<MeshRenderer>()==null)
	    gameObject.AddComponent(typeof(MeshRenderer));

	if(GetComponent<MeshFilter>()==null)
	    gameObject.AddComponent(typeof(MeshFilter)); 




	
	MeshFilter filter = GetComponent<MeshFilter>() as MeshFilter; 
        filter.mesh = this.mesh;
	MeshRenderer mRenderer = GetComponent<MeshRenderer>() as MeshRenderer;
	mRenderer.material = this.material; 

	//NOTE HARD CODING !!! 
	mRenderer.sortingLayerName = "Foreground"; 
    }

    void Start(){
	UpdateShape(ourPolygon.points); 
    }

    /*
    //Test Behavior 
    void Start(){
	Vector2[] otherVertices2D = cutObject.GetComponent<PolygonCollider2D>().points; 
	ExplosionCut( otherVertices2D,
		      cutObject.transform.position ); 
	Destroy(cutObject); 
	}
    */

}
