using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_ShapeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is the other cube
        if (collision.gameObject.name == "Cube2")
        {
            // Get the FixedJoint component
            FixedJoint fj = GetComponent<FixedJoint>();

            // Attach the cubes together
            fj.connectedBody = collision.rigidbody;
        }
    }
}
