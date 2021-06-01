using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class surface : MonoBehaviour
{
    private bool initialized = false;
	Vector3 velocity;
    float obstacleType;
    float obstaclePos;
    bool addPoints;
    float timeSinceSpeedup;

    GameObject thimble;
    GameObject hangingButtons;
    //GameObject wall1 = transform.GetChild(2);
    //GameObject wall2 = transform.GetChild(3);

    // Start is called before the first frame update
    void Start()
    {
        thimble = transform.GetChild(0).gameObject;
        hangingButtons = transform.GetChild(1).gameObject;
        velocity = new Vector3(0,0,2.5f);
        // Hide obstacles (so the player isn't stuck in the middle when starting the game)
        setObstacleVisible(false);
        Random.InitState(PlayerPrefs.GetInt("randomSeed"));

        timeSinceSpeedup = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
    	// end game after one minute
    	if (60f < Time.timeSinceLevelLoad)
    	{
    		// save values to database/firebase
    		// ...
    		
    		// return to main menu
    		SceneManager.LoadScene("MainMenu");
    	}
    
        // Speedup every 20s
        if(timeSinceSpeedup + 20f < Time.time)
        {
            velocity += new Vector3(0, 0, 0.5f); // Verify if this speedup is doable
            timeSinceSpeedup = Time.time;
        }

        if (Mathf.RoundToInt(transform.position.z) == 1 && addPoints) // Approximately when the bear passes the obstacles
        {
            scores.playerScore += 10;
            Debug.Log("Player score: " + scores.playerScore);
            addPoints = false;
        }

        transform.position -= velocity*Time.deltaTime;
    	if (transform.position.z <= -3)
    	{

            transform.position += new Vector3(0, 0, 12);
            obstaclePos = Mathf.Round(Random.Range(-1f,1f));
            obstacleType = Mathf.Round(Random.Range(0f,1f));

            switch (obstacleType)
            {
                case 0:
                    // Show all obstacles except hangingButtons
                    setObstacleVisible(true);
                    hangingButtons.SetActive(false);

                    thimble.transform.position = new Vector3(obstaclePos, thimble.transform.position.y, thimble.transform.position.z);

                    break;
                case 1:
                    // Show all obstacles except thimble
                    setObstacleVisible(true);
                    thimble.SetActive(false);

                    hangingButtons.transform.position = new Vector3(obstaclePos, hangingButtons.transform.position.y, hangingButtons.transform.position.z);
                    break;
            }

            // Set wall positions
            switch (obstaclePos)
            {
                case -1:
                    // wall1.transform.position = new Vector(0, wall1.transform.position.y, wall1.transform.position.z);
                    // wall2.transform.position = new Vector(1, wall2.transform.position.y, wall2.transform.position.z);
                    break;
                case 0:
                    // wall1.transform.position = new Vector(-1, wall1.transform.position.y, wall1.transform.position.z);
                    // wall2.transform.position = new Vector(1, wall2.transform.position.y, wall2.transform.position.z);
                    break;
                case 1:
                    // wall1.transform.position = new Vector(-1, wall1.transform.position.y, wall1.transform.position.z);
                    // wall2.transform.position = new Vector(0, wall2.transform.position.y, wall2.transform.position.z);
                    break;
            }
            addPoints = true;
        }
    }

    void setObstacleVisible(bool visible)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(visible);
        }
    }
}
