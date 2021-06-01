using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Swipe Levels
    public void loadSwipeLevel(int level)
    {
        PlayerPrefs.SetInt("inputMethod", 1);
        PlayerPrefs.SetInt("randomSeed", level);
        Debug.Log("Loading level: " + level);
        SceneManager.LoadScene("GameScene");
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
        Debug.Log("Loading level: " + level);
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
