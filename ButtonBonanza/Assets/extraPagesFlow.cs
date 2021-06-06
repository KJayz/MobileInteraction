using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class extraPagesFlow : MonoBehaviour
{
	public Text score; 

    // Start is called before the first frame update
    void Start()
    {
    	if (score != null) score.text = "Score: " + scores.playerScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Return()
    {
    	SceneManager.UnloadSceneAsync("HelpPage");
    }
    
    public void TryIt()
    {
    	SceneManager.LoadScene("TutorialScene");
    }
    
    public void OutOfTime()
    {
		if (scores.freePlay)
		{ 
			SceneManager.LoadScene("FreePlay");
			return;
		}
		
		// return to main menu or go to questionnaires
		if (scores.nrPlayedLevels == 3 || scores.nrPlayedLevels == 6)
		{
			SceneManager.LoadScene("HoldingPhone");
		}
		else SceneManager.LoadScene("MainMenu");
    }
    
    public void ThankYou()
    {
    	SceneManager.LoadScene("FreePlay"); 
    }
}
