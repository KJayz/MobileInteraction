using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bear : MonoBehaviour
{
	int lane;
	int laneMovement;
    bool isMoving = false;
    float speed = 2f;
    float strafeSpeed, jumpSpeed, duckSpeed;

	// materials needed for changing back and forth to ducking bear sprite
	public Material bearMat; 
	public Material bearDuckMat;

	// input control variables
	public int inputMethod = 1; // (1 = swiping, 2 = tapping virtual buttons)
    Vector2 startTouchPos; 
    int input;



    // Start is called before the first frame update
    void Start()
    {
    	lane = 0; // (negative = left lane, 0 = middle lane, positive = right lane)
    	input = 0; // (0 = none, 1 = left, 2 = right, 3 = up, 4 = down)
    	duckSpeed = speed;
    	jumpSpeed = 1.5f*speed;
    	strafeSpeed = 2*speed;
    }

    // Update is called once per frame
    void Update()
    {      
        // Reset position & speed, if bear's position too low or too much to one of the sizes
		resetPosition();

		// understand user's input
		if (inputMethod == 2)
			Debug.Log("tapping virtual buttons - input function not yet created");
		else // inputMethod == 1
		{
			duckSpeed = 2*speed;
			listenForSwipes();
		}

		// respond to user's input
        respondToInput();
    }
    
    
    void resetPosition()
    {
    	// reset pos after jump
    	if (transform.position.y < 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Reset pos");
        }
        
        // reset pos for lanes, to keep the lanes discrete
        if (transform.position.x < -1f || transform.position.x > 1f) 
        {
            transform.position = new Vector3((float)lane, transform.position.y, transform.position.z);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Reset pos");
        }
    }
    
    void listenForSwipes() // inspired by: https://www.youtube.com/watch?v=kreI-0i_oHw
    {
    	// Note: not perfect yet in terms of performance...
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began) startTouchPos = touch.position;
            
            if (touch.phase == TouchPhase.Moved)
            {
            	float xMoved = startTouchPos.x - touch.position.x;
            	float yMoved = startTouchPos.y - touch.position.y;
            	float movedDist = Mathf.Sqrt(xMoved*xMoved + yMoved*yMoved);
            	
            	if (movedDist > 80f)
            	{
            	    bool horSwipe = Mathf.Abs(xMoved) > Mathf.Abs(yMoved);
            		if (horSwipe && xMoved > 0) input = 1; // left swipe
            		else if (horSwipe && xMoved < 0) input = 2; // right swipe
            		else if (!horSwipe && yMoved < 0) input = 3; // up swipe
            		else if (!horSwipe && yMoved > 0) input = 4; // down swipe
            	}
            }
            
            if (touch.phase == TouchPhase.Ended) startTouchPos = new Touch().position;
        }
    }
    
    void respondToInput()
    {
        if(GetComponent<Rigidbody>().velocity == Vector3.zero && !isMoving)
        {
        	// input.getkeys are kept, to be able to test easily on desktop
            if ((Input.GetKey("a") || input == 1) && lane >= 0)
            {
                laneMovement = -1;
                input = 0;
                StartCoroutine(laneChange());
            }
            
            if ((Input.GetKey("d") || input == 2) && lane <= 0)
            {
                laneMovement = 1;
                input = 0;
                StartCoroutine(laneChange());
            }

            if (Input.GetKey("w") || input == 3)
            {
            	input = 0;
                StartCoroutine(jump());
            }
            
            if (Input.GetKey("s") || input == 4)
            {
            	input = 0;
				StartCoroutine(duck());
			}
        }
    }
    
    IEnumerator laneChange()
    {
        isMoving = true;
        GetComponent<Rigidbody>().velocity = new Vector3(laneMovement, 0, 0) * strafeSpeed;
        lane += laneMovement;
        yield return new WaitForSeconds(1 / strafeSpeed);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    	laneMovement = 0;
        isMoving = false;
    }
    
    IEnumerator jump()
    {
        isMoving = true;
        GetComponent<Rigidbody>().velocity = jumpSpeed * Vector3.up;
    	GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity - jumpSpeed * Vector3.up * Time.deltaTime;
    	yield return new WaitWhile(() => transform.position.y < 1.5f);
    	GetComponent<Rigidbody>().velocity = jumpSpeed * Vector3.down;
    	GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + jumpSpeed * Vector3.down * Time.deltaTime; 
    	yield return new WaitWhile(() => transform.position.y > 0.5f);
        isMoving = false;
    }
    
    IEnumerator duck()
    {
        isMoving = true;
        // bear starts ducking
        GetComponent<MeshRenderer>().material = bearDuckMat;
        GetComponent<BoxCollider>().size = new Vector3(8,0.075f,4.5f);
        GetComponent<BoxCollider>().center = new Vector3(0,0,2.7f);
        yield return new WaitForSeconds(1.5f / duckSpeed);
        
        // bear starts moving normally again
        GetComponent<MeshRenderer>().material = bearMat;
        GetComponent<BoxCollider>().size = new Vector3(8,0.075f,9.5f);
        GetComponent<BoxCollider>().center = new Vector3(0,0,0);
        isMoving = false;
    }
    
	void OnTriggerEnter(Collider other)
	{
		scores.playerScore -= 10;
		Debug.Log("Bear: Collision detected");
	}
}
