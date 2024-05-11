using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class L_ShapeController : MonoBehaviour
{
    public Rigidbody cube2;
    public TextMesh velocity2Text;
    private Rigidbody lShape1;
    public Rigidbody lShape2;
    public Rigidbody lShape3;
    private bool isAttached = false;
    private List<List<float>> timeSeries;
    private float currentTimeStep; // s
    // Start is called before the first frame update
    void Start()
    {
        lShape1 = GetComponent<Rigidbody>();
        timeSeries = new List<List<float>>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttached) {
            velocity2Text.text = "xVelocityL-shape1: " + lShape1.velocity.x.ToString("F2") + "\n" + 
            "xVelocityL-shape2: " + lShape2.velocity.x.ToString("F2") + "\n" + 
            "xVelocityL-shape3: " + lShape3.velocity.x.ToString("F2") + "\n" + 
            "xVelocityCube2: " + cube2.velocity.x.ToString("F2");
        } else {
            velocity2Text.text = "xVelocityCube2: " + cube2.velocity.x.ToString("F2");
        }
    }

    // FixedUpdate can be called multiple times per frame
    void FixedUpdate() {
        currentTimeStep += Time.deltaTime;
        timeSeries.Add(new List<float>() {currentTimeStep, lShape1.position.x, lShape1.velocity.x, lShape2.position.x, lShape2.velocity.x, lShape3.position.x, lShape3.velocity.x, isAttached ? 1 : 0, cube2.position.x, cube2.velocity.x});
   
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

            // Set the isAttached flag to true
            isAttached = true;

            velocity2Text.fontSize = 50;
        }
    }
    
    void OnApplicationQuit() {
        WriteTimeSeriesToCSV();
    }

    void WriteTimeSeriesToCSV() {
        using (var streamWriter = new StreamWriter("time_series_l_shape.csv")) {
            streamWriter.WriteLine("t,lShape1PositionX(t),lShape1Velocity(t),lShape2PositionX(t),lShape2Velocity(t),lShape3PositionX(t),lShape3Velocity(t),isAttached,cube2PositionX(t),cube2Velocity(t)");
            
            foreach (List<float> timeStep in timeSeries) {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
}
