using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
	public Transform surfaceObj;
	private Vector3 nextSurfaceSpawn;

    // Start is called before the first frame update
    void Start()
    {
         nextSurfaceSpawn = new Vector3(0,0,6); 
         StartCoroutine(spawnSurface());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator spawnSurface()
    {
    	yield return new WaitForSeconds(1);
    	
    	Instantiate(surfaceObj, nextSurfaceSpawn, surfaceObj.rotation);
    	nextSurfaceSpawn.z += 3;
    	
    	StartCoroutine(spawnSurface());
    }
}
