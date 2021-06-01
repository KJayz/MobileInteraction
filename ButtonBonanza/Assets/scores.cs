using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scores : MonoBehaviour
{

	public static int playerScore; // 10 points for obstacles dodged, 0 points for obstacles hit
	
	public static int inputMethod; // (1 = swiping, 2 = tapping virtual buttons)
	public static int nrPlayedLevels = 0; 

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
