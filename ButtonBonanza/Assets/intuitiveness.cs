using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class intuitiveness : MonoBehaviour
{
	public GameObject[] questionGroups; 
	public int[] answers;
	public GameObject popupPanel;
	public Button nextButton;
	
	HashSet<string> changedSliders = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        answers = new int[questionGroups.Length];
    }
    
    public void SubmitRTLX()
    {
    	for (int i = 0; i < answers.Length; i++)
    	{
    		answers[i] = ReadAnswer(questionGroups[i]);
    		if (changedSliders.Count != 6)
            {
				popupPanel.SetActive(true);
                return;
            }
    		
    		Debug.Log("Answer for question " + i + " is " + answers[i]);
    		// save R-TLX results to firebase
    		// ... 
    	}
        saveToDB(answers);
    	
    	SceneManager.LoadScene("Enjoyment1");
    }
    
    public void UpdateSliderChanges(string name)
    {
    	changedSliders.Add(name);
    	if (changedSliders.Count == 6) nextButton.GetComponent<Image>().color = new Color(0.6737718f,0.8622429f,0.9716981f);
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

    void saveToDB(int[] answers)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Collection("User " + PlayerPrefs.GetInt("UserID")).Document(SceneManager.GetActiveScene().name + "Input" + scores.inputMethod);
        Dictionary<string, object> docData = new Dictionary<string, object>
        {
            { "arrayExample", answers }
        };
        docRef.SetAsync(docData);
    }
}
