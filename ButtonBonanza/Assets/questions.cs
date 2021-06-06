using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// inspired by: https://www.youtube.com/watch?v=zbNxrGl4nfc
// note: three similar submit-functions to make them more easy to add to database
public class questions : MonoBehaviour
{
	public GameObject popupPanel;
	public Button nextButton;
	
	public GameObject[] questionGroups; 
	public string[] answers;

    void Start()
    {
        answers = new string[questionGroups.Length];
    }
    
    public void SubmitDemographics()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswer(questionGroups[i]);
    		if (answers[i] == "" || answers[i] == null)
            {
				popupPanel.SetActive(true);
                return;
            }
    		
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		// save demographics to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("MainMenu");
    }
    
    public void SubmitPhoneHolding()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswerLabels(questionGroups[i]);
    		if (answers[i] == "" || answers[i] == null)
            {
				popupPanel.SetActive(true);
                return;
            }

    		Debug.Log("Answer to the question is " + answers[i]);
    		// save phone holding to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("Enjoyment1");
    }
    
    public void SubmitGEQ1()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswerLabels(questionGroups[i]);
    		if (answers[i] == "" || answers[i] == null)
            {
				popupPanel.SetActive(true);
                return;
            }
            
    		Debug.Log("Answer for question " + i + " is " + answers[i].Substring(0,1));
    		// save first half of GEQ to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("Enjoyment2");
    }
    
    public void SubmitGEQ2()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswerLabels(questionGroups[i]);
    		if (answers[i] == "" || answers[i] == null)
            {
				popupPanel.SetActive(true);
                return;
            }
            
    		Debug.Log("Answer for question " + i + " is " + answers[i].Substring(0,1));
    		// save second half of GEQ to firebase
    		// ... 
    	}
    	
    	if (scores.nrPlayedLevels == 6) SceneManager.LoadScene("ThankYou");
		else SceneManager.LoadScene("MainMenu");
    }
    
    string ReadAnswer(GameObject questionGroup)
    {
    	string result = "";
    	
    	GameObject a = questionGroup.transform.Find("Answer").gameObject;
    	
    	if (a.GetComponent<ToggleGroup>() != null)
    	{
    		for (int i = 0; i < a.transform.childCount; i++)
    		{
    			if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
    			{
    				result = a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
    				break;
    			}
    		}
    	}
    	else if (a.GetComponentInChildren<InputField>() != null)
    	{
    		result = a.GetComponentInChildren<InputField>().text;
    	}
    	
    	return result;
    }
    
    string ReadAnswerLabels(GameObject questionGroup)
    {
    	string result = "";
    	
    	GameObject a = questionGroup.transform.Find("Answer").gameObject;
    	
    	if (a.GetComponent<ToggleGroup>() != null)
    	{
    		for (int i = 0; i < a.transform.childCount; i++)
    		{
    			Toggle toggleChild = a.transform.GetChild(i).GetComponent<Toggle>();
    			if (toggleChild != null && toggleChild.isOn) // if null, then scale label encountered
    			{
    				result = toggleChild.ToString();
    				break;
    			}
    		}
    	}
    	
    	return result;
    }
}
