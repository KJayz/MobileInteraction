using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class consent : MonoBehaviour
{
	public GameObject popupPanel;
	public Button nextButton; 
	public Toggle agreeToggle; 
	
	bool agreed = false; 
	
    void Start()
    {
        if(PlayerPrefs.GetInt("FreePlay") == 1)
        {
            SceneManager.LoadScene("FreePlay");
        }
    }

	public void ReadConsent()
	{
		agreeToggle.interactable = true;
	}
    
    public void ToggleAgree()
    {
    	agreed = !agreed;
    	if (agreed) nextButton.GetComponent<Image>().color = new Color(0.6737718f,0.8622429f,0.9716981f);
    	else nextButton.GetComponent<Image>().color = new Color(0.7843137f,0.7843137f,0.7843137f);
    }
    
    public void ProvideConsent()
    {
    	if (agreed == false)
    	{
    		popupPanel.SetActive(true);
    	}
    	else
    	{
    		SceneManager.LoadScene("Demographics");
    	}
    }
}
