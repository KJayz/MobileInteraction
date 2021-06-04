using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;

public class MainMenu : MonoBehaviour
{

    public Text InternetErrorText;

    void start()
    {
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
    }
    void update()
    {
        FirebaseDatabase.DefaultInstance.GetReference(".info/connected").ValueChanged += HandleConnectedChanged;
    }

	public void loadLevel(int level)
	{
        PlayerPrefs.SetInt("randomSeed", level);      
        Debug.Log("Loading level: " + level);   
        
        if (scores.nrPlayedLevels == 0)
        {
        	float controlType = Mathf.Round(Random.Range(1f,2f));
        	if (controlType == 1) scores.inputMethod = 1;
        	else if (controlType == 2) scores.inputMethod = 2;
        }
        else if (scores.nrPlayedLevels == 3) // tutorial + 2 levels
        {
        	scores.inputMethod = 3 - scores.inputMethod; // 1 -> 2, 2 -> 1
        }
        
        if (level < 10)
        {
            scores.tutorialLevel = false;
            SceneManager.LoadScene("GameScene");
        } else
        {
            scores.tutorialLevel = true;
            if (scores.inputMethod == 1) SceneManager.LoadScene("SwipeExpl");
            else if (scores.inputMethod == 2) SceneManager.LoadScene("VirtBtnExpl");
            else SceneManager.LoadScene("TutorialScene"); // default
        }
        scores.nrPlayedLevels += 1;
	}

    public void openHelp()
    {
    	// TODO: Create help page
		// SceneManager.LoadScene("HelpPage", LoadSceneMode.Additive);
		// SceneManager.UnloadSceneAsync("HelpPage");
    }

    public void quit()
    {
        Application.Quit();
    }

    private void HandleConnectedChanged(object sender, ValueChangedEventArgs e)
    {
        var s = e.Snapshot;
        if(s == null)
        {
            return;
        }
        if ((bool)s.GetValue(false))
        {
            InternetErrorText.gameObject.SetActive(true);
        } else
        {
            InternetErrorText.gameObject.SetActive(false);
        }
    }
}
