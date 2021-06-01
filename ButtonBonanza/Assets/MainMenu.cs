using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	// NOTE: Original loadLevel function with my added code
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
        else if (scores.nrPlayedLevels == 2)
        {
        	scores.inputMethod = 3 - scores.inputMethod; // 1 -> 2, 2 -> 1
        }
        
        SceneManager.LoadScene("GameScene");
        scores.nrPlayedLevels += 1;
	}

    // Swipe Levels
    public void loadSwipeLevel(int level)
    {
       PlayerPrefs.SetInt("inputMethod", 1);
       PlayerPrefs.SetInt("randomSeed", level);
       Debug.Log("Loading Swipe level: " + level); 
       SceneManager.LoadScene("GameScene");
       scores.nrPlayedLevels += 1;
    }

    public void loadSwipeTutorialLevel(int level)
    {
        PlayerPrefs.SetInt("inputMethod", 1);
        PlayerPrefs.SetInt("randomSeed", level);
        Debug.Log("Loading Swipe Tutorial level: " + level);
        SceneManager.LoadScene("TutorialScene");
    }

    // Buttons levels
    public void loadButtonsLevel(int level)
    {
        PlayerPrefs.SetInt("inputMethod", 2);
        PlayerPrefs.SetInt("randomSeed", level);
        Debug.Log("Loading Buttons level: " + level);
        SceneManager.LoadScene("GameScene");
    }


    public void loadButtonsTutorialLevel(int level)
    {
        PlayerPrefs.SetInt("inputMethod", 2);
        PlayerPrefs.SetInt("randomSeed", level);
        Debug.Log("Loading Buttons Tutorial level: " + level);
        SceneManager.LoadScene("TutorialScene");
    }

    public void openHelp()
    {
        // TODO: Show help popup
    }

    public void quit()
    {
        Application.Quit();
    }
}
