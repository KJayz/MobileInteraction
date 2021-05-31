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
        SceneManager.LoadScene("GameScene");
    }
    
    public void loadTutorialLevel(int level)
    {
        PlayerPrefs.SetInt("randomSeed", level);
        Debug.Log("Loading Tutorial level: " + level);
        //SceneManager.LoadScene("TutorialGameScene");
    }
}
