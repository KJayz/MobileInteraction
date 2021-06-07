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
	public Text LvlSelectText;
    public Text InternetErrorText;
    int nrPlayedLevels;

	bool controlSet = false;

    void start()
    {
    }
    
    void update()
    {
        FirebaseDatabase.DefaultInstance.GetReference(".info/connected").ValueChanged += HandleConnectedChanged;
    }
    
    void Awake()
    {
        nrPlayedLevels = PlayerPrefs.GetInt("nrPlayedLevels");
        Debug.Log("nrPlayedLevels: "+ nrPlayedLevels);
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
	    
	    
	    if (nrPlayedLevels == 0 && !controlSet) 
	    {
	    	scores.inputMethod = (int) Mathf.Round(Random.Range(1f,2f));
	    	LvlSelectText.text = "Level Select 1 of 2";
	    }
        else if (nrPlayedLevels == 3 && !controlSet) 
        {
        	scores.inputMethod = 3 - scores.inputMethod; // 1 -> 2, 2 -> 1
        	LvlSelectText.text = "Level Select 2 of 2";
        }
    	if (scores.inputMethod == 1) 
    	{
    		lvl1Btn.GetComponentInChildren<Text>().text = "Swiping 1";
    		lvl2Btn.GetComponentInChildren<Text>().text = "Swiping 2";
    	}
    	else if (scores.inputMethod == 2) 
    	{
    		lvl1Btn.GetComponentInChildren<Text>().text = "Tapping 1";
    		lvl2Btn.GetComponentInChildren<Text>().text = "Tapping 2";
    	}
	    
	}

	public void loadLevel(int level)
    {
        nrPlayedLevels = PlayerPrefs.GetInt("nrPlayedLevels");
        PlayerPrefs.SetInt("randomSeed", level);      
        Debug.Log("Loading level: " + level);   
        
        if (nrPlayedLevels == 0)
        {
        	// float controlType = Mathf.Round(Random.Range(1f,2f));
        	if (scores.inputMethod == 1) 
        	{
        		// scores.inputMethod = 1;
        		lvl1Btn.GetComponentInChildren<Text>().text = "Swiping 1";
        		lvl2Btn.GetComponentInChildren<Text>().text = "Swiping 2";
        	}
        	else if (scores.inputMethod == 2) 
        	{
        		// scores.inputMethod = 2;
        		lvl1Btn.GetComponentInChildren<Text>().text = "Tapping 1";
        		lvl2Btn.GetComponentInChildren<Text>().text = "Tapping 2";
        	}
        }
        nrPlayedLevels++;
        PlayerPrefs.SetInt("nrPlayedLevels", nrPlayedLevels);

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
