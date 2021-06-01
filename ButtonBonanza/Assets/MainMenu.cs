using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
    
    public void loadTutorialLevel(int level)
    {
        PlayerPrefs.SetInt("randomSeed", level);
        Debug.Log("Loading Tutorial level: " + level);
        //SceneManager.LoadScene("TutorialGameScene");
    }
}
