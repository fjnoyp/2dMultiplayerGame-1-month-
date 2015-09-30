//-*-java-*-
using UnityEngine; 

public class MeshGenerator : MonoBehaviour{

    private Mesh mesh;
    
    public PolygonCollider2D optPolygon; 
    public Material optMaterial; 

    void Start(){
	if(optPolygon!=null){
	    CreateMesh(optPolygon.points,optMaterial); 
	}
    }

    public void CreateMesh(Vector2[] newPoints, Material material){
	// Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(newPoints);
        int[] indices = tr.Triangulate();

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
	mRenderer.material = material; 

	//NOTE HARD CODING !!! 
	mRenderer.sortingLayerName = "Foreground"; 
    }
}
