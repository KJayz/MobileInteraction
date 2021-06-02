using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class consent : MonoBehaviour
{
	public Button nextButton; 
	public Toggle agreeToggle; 
	
	public void ReadConsent()
	{
		agreeToggle.interactable = !agreeToggle.interactable;
	}
    
    public void ToggleAgree()
    {
    	nextButton.interactable = !nextButton.interactable;
    }
    
    public void ProvideConsent()
    {
    	SceneManager.LoadScene("Demographics");
    }
}
