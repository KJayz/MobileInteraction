using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class surface : MonoBehaviour
{
	Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
    	velocity = new Vector3(0,0,3);
    }

    // Update is called once per frame
    void Update()
    {
    	transform.position -= velocity*Time.deltaTime;
    	if (transform.position.z <= -3)
    	{
    		transform.position += new Vector3(0,0,9);
    	}
    }
}
