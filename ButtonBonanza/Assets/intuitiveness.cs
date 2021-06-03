using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class intuitiveness : MonoBehaviour
{
	public GameObject[] questionGroups; 
	public int[] answers;
	public Button submitButton;
	
	HashSet<string> changedSliders = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        answers = new int[questionGroups.Length];
    }
    
    void Update() // to test
    {
    	if (changedSliders.Count == 6) submitButton.interactable = true;
    }
    
    public void SubmitRTLX()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswer(questionGroups[i]);
    		
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		
    		// save R-TLX results to firebase
    		// ... 
    	}
    	
    	SceneManager.LoadScene("MainMenu");
    }
    
    public void UpdateSliderChanges(string name)
    {
    	changedSliders.Add(name);
    	Debug.Log("Changed Sliders count: " + changedSliders.Count);
    }
    
    int ReadAnswer(GameObject questionGroup)
    {
    	int result = -1;
    	
    	GameObject a = questionGroup.transform.Find("Answer").gameObject;
    	
    	if (a.GetComponentInChildren<Slider>() != null)
    	{
    		Slider slider = a.GetComponentInChildren<Slider>();
    		result = (int) a.GetComponentInChildren<Slider>().value;
    	}
    	
    	return result;
    }
}
