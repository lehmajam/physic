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
        if (!isAttached) {
            // Code for calculating angular momentum before collision
            velocity2Text.fontSize = 50;
            Vector3 p1 = lShape1.mass * lShape1.velocity;
            Vector3 p2 = cube2.mass * cube2.velocity;

            Vector3 r1 = lShape1.transform.position - lShape1.centerOfMass;
            Vector3 r2 = cube2.transform.position - cube2.centerOfMass;

            Vector3 L1 = Vector3.Cross(r1, p1);
            Vector3 L2 = Vector3.Cross(r2, p2);

            velocity2Text.text = "Angular Momentum L-Shape: " + L1.ToString("F2") + "\n" +
            "Angular Momentum Cube2: " + L2.ToString("F2");
        } else {
            // Calculate total mass
            float totalMass = lShape1.mass + lShape2.mass + lShape3.mass + cube2.mass;
            
            // Calculate the center of mass
            Vector3 com = (lShape1.mass * lShape1.position + lShape2.mass * lShape2.position + lShape3.mass * lShape3.position + cube2.mass * cube2.position) / totalMass;
            
            // Calculate angular momentum about the new center of mass
            Vector3 L1 = Vector3.Cross(lShape1.position - com, lShape1.mass * lShape1.velocity);
            Vector3 L2 = Vector3.Cross(lShape2.position - com, lShape2.mass * lShape2.velocity);
            Vector3 L3 = Vector3.Cross(lShape3.position - com, lShape3.mass * lShape3.velocity);
            Vector3 L4 = Vector3.Cross(cube2.position - com, cube2.mass * cube2.velocity);

            Vector3 totalAngularMomentum = L1 + L2 + L3 + L4;

            // Calculate velocity of the combined body
            Vector3 velocity = (lShape1.mass * lShape1.velocity + lShape2.mass * lShape2.velocity + lShape3.mass * lShape3.velocity + cube2.mass * cube2.velocity) / totalMass;

            // Display the center of mass, velocity, and angular momentum
            velocity2Text.text = "Velocity: " + velocity.ToString("F2") + "\n" +
            "Angular Momentum: " + totalAngularMomentum.ToString("F2");
        }
    }

    // FixedUpdate can be called multiple times per frame
    void FixedUpdate() {
        currentTimeStep += Time.deltaTime;
        timeSeries.Add(new List<float>() {currentTimeStep, 
        lShape1.position.x, lShape1.position.y, lShape1.position.z, lShape1.velocity.x, lShape1.velocity.y, lShape1.velocity.z,
        lShape2.position.x, lShape2.position.y, lShape2.position.z, lShape2.velocity.x, lShape2.velocity.y, lShape2.velocity.z,
        lShape3.position.x, lShape3.position.y, lShape3.position.z, lShape3.velocity.x, lShape3.velocity.y, lShape3.velocity.z,
        isAttached ? 1 : 0, 
        cube2.position.x, cube2.position.y, cube2.position.z, cube2.velocity.x, cube2.velocity.y, cube2.velocity.z});
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
        }
    }
    
    void OnApplicationQuit() {
        WriteTimeSeriesToCSV();
    }

    void WriteTimeSeriesToCSV() {
        using (var streamWriter = new StreamWriter("time_series_l_shape.csv")) {
            streamWriter.WriteLine("t,lShape1PositionX(t),lShape1PositionY(t),lShape1PositionZ(t),lShape1VelocityX(t),lShape1VelocityY(t),lShape1VelocityZ(t)"+
            ",lShape2PositionX(t),lShape2PositionY(t),lShape2PositionZ(t),lShape2VelocityX(t),lShape2VelocityY(t),lShape2VelocityZ(t)"+
            ",lShape3PositionX(t),lShape3PositionY(t),lShape3PositionZ(t),lShape3VelocityX(t),lShape3VelocityY(t),lShape3VelocityZ(t)"+
            ",isAttached"+
            ",cube2PositionX(t),cube2PositionY(t),cube2PositionZ(t),cube2VelocityX(t),cube2VelocityY(t),cube2VelocityZ(t)");
            
            foreach (List<float> timeStep in timeSeries) {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
}
