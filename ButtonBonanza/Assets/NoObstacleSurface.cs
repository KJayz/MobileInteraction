using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoObstacleSurface : MonoBehaviour
{

    Vector3 velocity;
    float timeSinceSpeedup;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, 2.5f);
        timeSinceSpeedup = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        // Speedup every 20s
        if (timeSinceSpeedup + 20f < Time.time)
        {
            // TODO: Test level & bear speeds and see which speedup rate is best for both
            velocity += new Vector3(0, 0, velocity.z * 0.10f); // Verify if this speedup is doable - percentile increase so bear speed can increase at the same rate
            timeSinceSpeedup = Time.time;
        }
        transform.position -= velocity * Time.deltaTime;
        if (transform.position.z <= -3)
        {
            transform.position += new Vector3(0, 0, 12);
        }
    }
}
