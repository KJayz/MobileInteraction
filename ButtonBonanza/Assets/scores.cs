using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class scores : MonoBehaviour
{

	public static int playerScore; // 10 points for obstacles dodged, 0 points for obstacles hit
	
	public static int inputMethod; // (1 = swiping, 2 = tapping virtual buttons)
	public static int nrPlayedLevels = 0;
    public static float timeSinceLevelLoad;


    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        timeSinceLevelLoad = Time.timeSinceLevelLoad;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        submitToDB();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(playerScore);
    }

    static void submitToDB()
    {

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Collection("users").Document("alovelace");
        Dictionary<string, object> user = new Dictionary<string, object>
{
        { "First", "Ada" },
        { "Last", "Lovelace" },
        { "Born", 1815 },
};
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the alovelace document in the users collection.");
        });
    }
}
