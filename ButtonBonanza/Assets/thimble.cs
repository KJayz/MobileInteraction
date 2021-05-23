using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thimble : MonoBehaviour
{
	Vector3 velocity;
	bool pointsAdded;

    // Start is called before the first frame update
    void Start()
    {
    	velocity = new Vector3(0,0,3);
    	pointsAdded = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= velocity*Time.deltaTime;
        
        // add points if thimble encountered (they are substracted again in bear.cs if hit)
        if (Mathf.RoundToInt(transform.position.z) == 0 && pointsAdded == false)
        {
   			scores.playerScore += 10;
    		Debug.Log("Player score: " + scores.playerScore);
			pointsAdded = true;
        }
        
        // reusage of thimble
    	if (transform.position.z <= -3)
    	{
    		int curLane = Mathf.RoundToInt(transform.position.x);
    		int rndLane = Random.Range(-1,2);
    		int moveLane;
    		if (curLane == 0)
    		{
    			moveLane = rndLane;
    		}
    		else if (Mathf.Abs(curLane) == 1)
    		{
    			moveLane = rndLane - curLane;
    		}
    		else
    		{
    			moveLane = 0; // default; should not be encountered
    			Debug.Log("Something went wrong with initializing lane of thimble");
    		}
    		transform.position += new Vector3(moveLane,0,9);
    		pointsAdded = false;
    	}
    }
}
