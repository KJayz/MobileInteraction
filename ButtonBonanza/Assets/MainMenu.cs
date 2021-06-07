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
	public Button tutorialBtn, lvl1Btn, lvl2Btn;
    public Text InternetErrorText;
    int nrPlayedLevels;

    void start()
    {
        nrPlayedLevels = PlayerPrefs.GetInt("nrPlayedLevels");
    }
    
    void update()
    {
        FirebaseDatabase.DefaultInstance.GetReference(".info/connected").ValueChanged += HandleConnectedChanged;
    }
    
    void Awake()
	{
		if (SceneManager.GetActiveScene().name != "MainMenu") return;
		
        if (nrPlayedLevels %3 == 0)
	    {
	    	tutorialBtn.interactable = true;
	    	lvl1Btn.interactable = false;
	    	lvl2Btn.interactable = false;
	    }
	    else if (nrPlayedLevels %3 == 1)
	    {
	    	tutorialBtn.interactable = false;
	    	lvl1Btn.interactable = true;
	    	lvl2Btn.interactable = false;
	    } 
	    else if (nrPlayedLevels %3 == 2)
	    {
	    	tutorialBtn.interactable = false;
	    	lvl1Btn.interactable = false;
	    	lvl2Btn.interactable = true;
	    }
	}

	public void loadLevel(int level)
	{
        PlayerPrefs.SetInt("randomSeed", level);      
        Debug.Log("Loading level: " + level);   
        
        if (nrPlayedLevels == 0)
        {
        	float controlType = Mathf.Round(Random.Range(1f,2f));
        	if (controlType == 1) scores.inputMethod = 1;
        	else if (controlType == 2) scores.inputMethod = 2;
        }
        else if (nrPlayedLevels == 3) // tutorial + 2 levels
        {
        	scores.inputMethod = 3 - scores.inputMethod; // 1 -> 2, 2 -> 1
        }
        
        
        if (level < 10)
        {
            scores.tutorialLevel = false;		    
		    SceneManager.LoadScene("GameScene");
        } 
        else
        {
            scores.tutorialLevel = true;
            if (scores.inputMethod == 1) SceneManager.LoadScene("SwipeExpl");
            else if (scores.inputMethod == 2) SceneManager.LoadScene("VirtBtnExpl");
            else SceneManager.LoadScene("TutorialScene"); // default
        }

        PlayerPrefs.SetInt("nrPlayedLevels", nrPlayedLevels++);
	}

    public void openHelp()
    {
		SceneManager.LoadScene("HelpPage", LoadSceneMode.Additive);
    }
    
    public void quit()
    {
    	SceneManager.LoadScene("QuitPage", LoadSceneMode.Additive);
    }
    
    public void undoQuit()
    {
    	SceneManager.UnloadSceneAsync("QuitPage");
    }
    
    public void confirmQuit()
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
