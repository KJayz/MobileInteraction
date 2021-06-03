using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// inspired by: https://www.youtube.com/watch?v=zbNxrGl4nfc
// note: three similar submit-functions to make them more easy to add to database
public class questions : MonoBehaviour
{
	public GameObject[] questionGroups; 
	public string[] answers;
	// public Text allQuestionsAnsweredText;

    // Start is called before the first frame update
    void Start()
    {
        answers = new string[questionGroups.Length];
    }
    
    public void SubmitDemographics()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswer(questionGroups[i]);
    		
    		/* if (answers[i] == "" || answers[i] == null)
            {
                Debug.Log("Empty Answer");
                allQuestionsAnsweredText.gameObject.SetActive(true);
                return;
            } */	
    		
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		
    		// save demographics to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("MainMenu");
    }
    
    public void SubmitGEQ1()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswerLabels(questionGroups[i]).ToString();
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		
    		// save first half of GEQ to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("Enjoyment2");
    }
    
    public void SubmitGEQ2()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswerLabels(questionGroups[i]).ToString();
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		
    		// save second half of GEQ to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("MainMenu");
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
    
    int ReadAnswerLabels(GameObject questionGroup)
    {
    	int result = -1;
    	
    	GameObject a = questionGroup.transform.Find("Answer").gameObject;
    	
    	if (a.GetComponent<ToggleGroup>() != null)
    	{
    		for (int i = 0; i < a.transform.childCount; i++)
    		{
    			// still has an error somewhere!!!
    			Toggle toggleChild = a.transform.GetChild(i).GetComponent<Toggle>();
    			if (toggleChild != null && toggleChild.isOn)
    			{
    				result = toggleChild.transform.GetSiblingIndex() - 2;
    				break;
    			}
    		}
    	}
    	
    	return result;
    }
}
