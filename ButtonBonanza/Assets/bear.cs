using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bear : MonoBehaviour
{
	int lane;
	int laneMovement;
    float playerSpeed = 2;

	Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    	lane = 0; // (negative = left lane, 0 = middle lane, positive = right lane)
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            if (Input.GetKey("a") && lane >= 0)
            {
                laneMovement = -1;
                StartCoroutine(laneChange());
            }
            if (Input.GetKey("d") && lane <= 0)
            {
                laneMovement = +1;
                StartCoroutine(laneChange());
            }
        }
    }
    
    IEnumerator laneChange()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(laneMovement, 0, 0) * playerSpeed;
        yield return new WaitForSeconds(1 / playerSpeed);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        lane += laneMovement;
    	laneMovement = 0;
    }
}
