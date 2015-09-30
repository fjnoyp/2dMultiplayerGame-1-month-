//-*-java-*-
using UnityEngine; 

using ClipperLib; //just for clip type 
using System.Collections.Generic; //just for List<T>

public class ExplosionObjectVisualEffect : AbObjectVisualEffect{

    public GameObject[] debrisPrefabs;
    public float creationSpread; 
    public int minCount, maxCount; 
    public Vector2 minSpeed;
    public Vector2 maxSpeed; 

    public bool playOnStart; 

    void Start(){
	SpawnDebris((Vector2)transform.position,new Vector2(0,0)); 
    }

    //pos should be projectile's hit location 
    override public void StartEffect(GameObject gObj, Vector2 pos, Vector2 dir){
	if(gObj.tag == "DestEnviro"){
	    SpawnDebris(pos,dir); 
	}
    }

    private void SpawnDebris(Vector2 pos, Vector2 dir){
	for(int i = 0; i<Random.Range(minCount,maxCount); i++){
	    GameObject debris = Instantiate( debrisPrefabs[ Random.Range(0,debrisPrefabs.Length) ],
	        new Vector3( pos.x + 
	        Random.Range(-creationSpread,creationSpread),
	        pos.y + 
	        Random.Range(-creationSpread,creationSpread),
	        0),
	        Quaternion.Euler(0,0,Random.Range(0,360))) as GameObject; 
	    debris.rigidbody2D.velocity = new Vector2(Random.Range(minSpeed.x,maxSpeed.x), Random.Range(minSpeed.y,maxSpeed.y)); 
								     
	}
    }

    /* RAND DEBRIS CREATOR 
      Vector2[] randPoints0 = GenerateRandomPoints(pos,5); 
      Vector2[] randPoints1 = GenerateRandomPoints(pos,5); 

      List<Vector2[]> polygons = PolyClipper.ClipPoly(randPoints0,
      randPoints1,
      ClipType.ctXor);
	    
      polygons = PolyClipper.SimplifyPolys(polygons); 

      for(int i = 0; i<polygons.Count; i++){
      CreateDebris(pos,dir,polygons[i]); 
      }

    private Vector2[] GenerateRandomPoints(Vector2 centerPos, int num){
	Vector2[] randPoints = new Vector2[num];
	for(int i = 0; i<num; i++){
	    randPoints[i] = new Vector2( centerPos.x + Random.Range(-2f,2f),
					 centerPos.y + Random.Range(-2f,2f));
	}
	return randPoints; 
    }

    private void CreateDebris(Vector2 pos, Vector2 dir, Vector2[] vertices){
	GameObject debris = GameObject.Instantiate(debrisPrefab,new Vector3(0,0,0),
	    Quaternion.Euler(new Vector3(0,0,
					 Mathf.Rad2Deg*Mathf.Atan2(dir.y,dir.x)))) as GameObject; 
	debris.GetComponent<MeshGenerator>().CreateMesh(vertices,
							debrisMaterial);
    }
    */

    
}
