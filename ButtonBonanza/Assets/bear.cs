using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bear : MonoBehaviour
{
	public int lane;
	int laneMovement;
    bool isMoving = false;
    float speed = 2f;
    float strafeSpeed, jumpSpeed, duckSpeed;
    float timeSinceSpeedup;

    // materials needed for changing back and forth to ducking bear sprite
    public Material bearMat; 
	public Material bearDuckMat;
    public Material bearJumpMat;
    public Material bearMoveMat1;
    public Material bearMoveMat2;
    public Material bearHurt;

	// input control variables
    Vector2 startTouchPos; 
    int input;
    public int lastInput;
	public Button leftBtn, rightBtn, upBtn, downBtn;
	public Canvas canvas;
    float lastInputTime;


    // Start is called before the first frame update
    void Start()

    {
        timeSinceSpeedup = Time.timeSinceLevelLoad;
        lastInputTime = Time.timeSinceLevelLoad;
        lane = 0; // (negative = left lane, 0 = middle lane, positive = right lane)
    	input = 0; // (0 = none, 1 = left, 2 = right, 3 = up, 4 = down)
    	duckSpeed = 1f*speed;
    	jumpSpeed = 1f*speed;
    	strafeSpeed = 2*speed;
    	
    	if (scores.inputMethod == 2) // add buttons if tapping condition
		{
			listenForTaps();
		}
    }

    // Update is called once per frame
    void Update()
    {

        // Speedup every 20s - if the level is faster, the bear needs to be able to do the same
        if (timeSinceSpeedup + 20f < Time.time)
        {
            strafeSpeed += strafeSpeed * 0.30f;
            jumpSpeed += jumpSpeed * 0.30f;
            duckSpeed += duckSpeed * 0.30f;
            timeSinceSpeedup = Time.time;
        }


		// understand user's input if swiping condition
		if (scores.inputMethod == 1)
		{
			listenForSwipes();
		}

		// respond to user's input
        respondToInput();

        // Reset position & speed, if bear's position too low or too much to one of the sizes
        resetPosition();
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
            
            //if (touch.phase == TouchPhase.Ended) startTouchPos = new Touch().position;
        }
    }
    
    void respondToInput()
    {
        if(GetComponent<Rigidbody>().velocity == Vector3.zero && !isMoving)
        {
            // If input or if no input in last 1s
            if(lastInputTime + 1f < Time.time)
            {
                lastInputTime = Time.time;
                lastInput = 0;
            }
        	// input.getkeys are kept, to be able to test easily on desktop
            if ((Input.GetKey("a") || input == 1) && lane >= 0)
            {
                lastInput = 1;
                lastInputTime = Time.time;
                laneMovement = -1;
                StartCoroutine(laneChange());
            }
            
            if ((Input.GetKey("d") || input == 2) && lane <= 0)
            {
                lastInput = 2;
                lastInputTime = Time.time;
                laneMovement = 1;
                StartCoroutine(laneChange());
            }

            if (Input.GetKey("w") || input == 3)
            {
                lastInput = 3;
                lastInputTime = Time.time;
                StartCoroutine(jump());
            }
            
            if (Input.GetKey("s") || input == 4)
            {
                lastInput = 4;
                lastInputTime = Time.time;
				StartCoroutine(duck());
			}
        }
        input = 0;
    }
    
    IEnumerator laneChange()
    {
        isMoving = true;
        GetComponent<Rigidbody>().velocity = new Vector3(laneMovement, 0, 0) * strafeSpeed;
        lane += laneMovement;

        //Changing the material of the bear
        if (laneMovement == 1)
        {
            GetComponent<MeshRenderer>().material = bearMoveMat2;
        }
        else
        {
            GetComponent<MeshRenderer>().material = bearMoveMat1;
        }
        yield return new WaitForSeconds(1 / strafeSpeed);
        if (laneMovement == 1)
        {
            GetComponent<MeshRenderer>().material = bearMoveMat1;
        }
        else
        {
            GetComponent<MeshRenderer>().material = bearMoveMat2;
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    	laneMovement = 0;
        yield return new WaitForSeconds(1 / (strafeSpeed*3));
        GetComponent<MeshRenderer>().material = bearMat;
        isMoving = false;

        transform.position = new Vector3((float)lane, transform.position.y, transform.position.z);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("Reset pos");
    }
    
    IEnumerator jump()
    {
        isMoving = true;
        //bear starts jumping
        GetComponent<MeshRenderer>().material = bearJumpMat;
        GetComponent<Rigidbody>().velocity = jumpSpeed * Vector3.up * 1.5f;
    	GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity - jumpSpeed * Vector3.up * Time.deltaTime;
    	yield return new WaitWhile(() => transform.position.y < 1.5f);

        //bear starts moving notmally again
        GetComponent<MeshRenderer>().material = bearMat;
    	GetComponent<Rigidbody>().velocity = jumpSpeed * Vector3.down * 2f;
    	GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + jumpSpeed * Vector3.down * Time.deltaTime; 
    	yield return new WaitWhile(() => transform.position.y > 0.5f);
        isMoving = false;
    }
    
    IEnumerator duck()
    {
        isMoving = true;
        // bear starts ducking
        GetComponent<MeshRenderer>().material = bearDuckMat;
        GetComponent<BoxCollider>().size = new Vector3(8,0.075f,2.5f);
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
        scores.playerHit = true;
        GetComponent<MeshRenderer>().material = bearHurt;
        Debug.Log("Bear: Collision detected");
    }
}
