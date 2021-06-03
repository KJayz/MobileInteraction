using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// inspired by: https://www.youtube.com/watch?v=zbNxrGl4nfc
public class demographics : MonoBehaviour
{
	public GameObject[] questionGroups; 
	public string[] answers;

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
    		
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		
    		// save demographics to firebase
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
}
