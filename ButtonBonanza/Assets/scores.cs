using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class scores : MonoBehaviour
{
    public GameObject player;
    static bear bearScript;

	public static int playerScore; // 10 points for obstacles dodged, 0 points for obstacles hit
	
	public static int inputMethod; // (1 = swiping, 2 = tapping virtual buttons)
	public static int nrPlayedLevels = 0;
    public static float timeSinceLevelLoad;

    public static bool playerHit = false;

    static int correctSwipes;
    static int incorrectSwipes;
    static int missedSwipes;
    static int poorlyTimedSwipes;

    public static bool tutorialLevel = false;
    public static bool freePlay = false;

    // Start is called before the first frame update
    void Start()
    {
        bearScript = player.GetComponent<bear>();
        playerScore = 0;
        correctSwipes = 0;
        incorrectSwipes = 0;
        missedSwipes = 0;
        poorlyTimedSwipes = 0;
        timeSinceLevelLoad = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void submitToDB()
    {

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Collection("User " + PlayerPrefs.GetInt("UserID")).Document("Level" + PlayerPrefs.GetInt("randomSeed") + "Input"+ scores.inputMethod);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "Score", playerScore },
                { "CorrectSwipes", correctSwipes },
                { "PoorlyTimedSwipes", poorlyTimedSwipes },
                { "IncorrectSwipes", incorrectSwipes },
                { "MissedSwipes", missedSwipes },
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Score Data Saved");
        });
    }

    public static void updateScore(float obstaclePos, float obstacleType)
    { 

        if (playerHit == false)
        {
            playerScore += 10;
            correctSwipes++;
            //Debug.Log("Correct Swipes: " + correctSwipes);
            return;
        }

        playerHit = false;
        Debug.Log("Obstacle Type: " + obstacleType);
        //Debug.Log("Last Input: " + bearScript.lastInput);

        Debug.Log("Obstacle Pos: " + obstaclePos);
        Debug.Log("Bear Lane: " + bearScript.lane);
        Debug.Log("Bear LastInput: " + bearScript.lastInput);
        if (bearScript.lane == obstaclePos && obstacleType + 3 == bearScript.lastInput)
        {
            poorlyTimedSwipes++;
            //Debug.Log("Poorly Timed Swipes: " + poorlyTimedSwipes);
            return;
        }

        if(bearScript.lastInput != 0)
        {
            incorrectSwipes++;
            //Debug.Log("Incorrect Swipes: " + incorrectSwipes);
            return;
        } else
        {
            missedSwipes++;
            //Debug.Log("Missed Swipes: " + missedSwipes);
            return;
        }
    }
}
