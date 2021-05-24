using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bear : MonoBehaviour
{
	int lane;
	int laneMovement;
    float startPosY;
    float strafeSpeed = 2f;
    float jumpSpeed = 2f;

	Vector3 velocity = Vector3.zero;
	public Material bearMat; 
	public Material bearDuckMat;

    // Start is called before the first frame update
    void Start()
    {
    	lane = 0; // (negative = left lane, 0 = middle lane, positive = right lane)
        startPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset position & speed if cube is on track again
        if (transform.position.y < 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Reset pos");
        }

        if (GetComponent<Rigidbody>().velocity.y != 0)
        {
            // Simulate Gravity
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity - jumpSpeed * Vector3.up * Time.deltaTime;
        }
        if(GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            if (Input.GetKey("a") && lane >= 0)
            {
                laneMovement = -1;
                StartCoroutine(laneChange());
            }
            if (Input.GetKey("d") && lane <= 0)
            {
                laneMovement = +1;
                StartCoroutine(laneChange());
            }

            if (Input.GetKey("w"))
            {
                GetComponent<Rigidbody>().velocity = Vector3.up * jumpSpeed;
            }
            
            if (Input.GetKey("s"))
            {
				StartCoroutine(duck());
            }
        }
    }
    
    IEnumerator laneChange()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(laneMovement, 0, 0) * strafeSpeed;
        yield return new WaitForSeconds(1 / strafeSpeed);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        lane += laneMovement;
    	laneMovement = 0;
    }
    
    IEnumerator duck()
    {
        GetComponent<MeshRenderer>().material = bearDuckMat;
        GetComponent<BoxCollider>().size = new Vector3(8,0.075f,4.5f);
        GetComponent<BoxCollider>().center = new Vector3(0,0,2.7f);
        yield return new WaitForSeconds(1.5f / strafeSpeed);
        GetComponent<MeshRenderer>().material = bearMat;
        GetComponent<BoxCollider>().size = new Vector3(8,0.075f,9.5f);
        GetComponent<BoxCollider>().center = new Vector3(0,0,0);
    }
    
	void OnTriggerEnter(Collider other)
	{
		scores.playerScore -= 10;
		Debug.Log("Bear: Collision detected");
	}
}
