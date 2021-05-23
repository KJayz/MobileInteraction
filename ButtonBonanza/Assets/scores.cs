using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scores : MonoBehaviour
{

	public static int playerScore; // 10 points for obstacles dodged, 0 points for obstacles hit
	// playerScore does work, but 

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(playerScore);
    }
}
