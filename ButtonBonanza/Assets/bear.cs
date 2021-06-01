using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bear : MonoBehaviour
{
	int lane;
	int laneMovement;
    bool isMoving = false;
    float speed = 2f;
    float strafeSpeed, jumpSpeed, duckSpeed;
    float timeSinceSpeedup;

    // materials needed for changing back and forth to ducking bear sprite
    public Material bearMat; 
	public Material bearDuckMat;

	// input control variables
	public int inputMethod = 2; // (1 = swiping, 2 = tapping virtual buttons)
    Vector2 startTouchPos; 
    int input;
	public Button leftBtn, rightBtn, upBtn, downBtn;
	public Canvas canvas;


    // Start is called before the first frame update
    void Start()

    {

        timeSinceSpeedup = Time.timeSinceLevelLoad;
        lane = 0; // (negative = left lane, 0 = middle lane, positive = right lane)
    	input = 0; // (0 = none, 1 = left, 2 = right, 3 = up, 4 = down)
    	duckSpeed = speed;
    	jumpSpeed = 1.5f*speed;
    	strafeSpeed = 2*speed;
    	
    	if (inputMethod == 2) // add buttons if tapping condition
		{
			listenForTaps();
			Debug.Log("tapping virtual buttons - input function not yet created");
		}
    }

    // Update is called once per frame
    void Update()
    {

        // Speedup every 20s - if the level is faster, the bear needs to be able to do the same
        if (timeSinceSpeedup + 20f < Time.time)
        {
            strafeSpeed += strafeSpeed * 0.10f;
            jumpSpeed += jumpSpeed * 0.10f;
            duckSpeed += duckSpeed * 0.10f;
            timeSinceSpeedup = Time.time;
        }

        // Reset position & speed, if bear's position too low or too much to one of the sizes
        resetPosition();

		// understand user's input if swiping condition
		if (inputMethod == 1)
		{
			duckSpeed = 2*speed; // Why change the duckspeed?
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
    
    public void listenForTaps() // for not-dynamic testing purposes
    {
    	Button leftButton = (Button) Instantiate(leftBtn);
    	leftButton.transform.SetParent(canvas.transform, false);
    	leftButton.onClick.AddListener(() => input = 1);
    	
    	Button rightButton = (Button) Instantiate(rightBtn);
    	rightButton.transform.SetParent(canvas.transform, false);
    	rightButton.onClick.AddListener(() => input = 2);
    	
    	Button upButton = (Button) Instantiate(upBtn);
    	upButton.transform.SetParent(canvas.transform, false);
    	upButton.onClick.AddListener(() => input = 3);
    	
    	Button downButton = (Button) Instantiate(downBtn);
    	downButton.transform.SetParent(canvas.transform, false);
    	downButton.onClick.AddListener(() => input = 4);
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
