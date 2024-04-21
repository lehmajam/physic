using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

/*
    Accelerates the cube to which it is attached, modelling an harmonic oscillator.
    Writes the position, velocity and acceleration of the cube to a CSV file.
    
    Remark: For use in "Physics Engines" module at ZHAW, part of physics lab
    Author: kemf
    Version: 1.0
*/
public class CubeController : MonoBehaviour
{
    public Rigidbody cube2;
    private Rigidbody cube1;
    public float springConstant; // kg
    public float springLength; // m
    public float windForce; // N
    private float springForceCube1; // N
    private float springForceCube2; // N
    private float springActive;
    private float currentTimeStep; // s
    private List<List<float>> timeSeries;
    public TextMesh velocityText;
    public TextMesh timeText;

    // Start is called before the first frame update
    void Start()
    {
        cube1 = GetComponent<Rigidbody>();
        timeSeries = new List<List<float>>();
        velocityText = GetComponentInChildren<TextMesh>();
        springForceCube1 = 0f;
        springForceCube2 = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        velocityText.text = "xVelocity1: " + cube1.velocity.x.ToString("F2");
        timeText.text = "t: " + currentTimeStep.ToString("F2");
    }

    // FixedUpdate can be called multiple times per frame
    void FixedUpdate() {
        springActive = 0;
        if (cube1.position.x > cube2.position.x - springLength) {
            springActive = 1;
            windForce = 0f;
            springForceCube1 = -springConstant * (cube1.position.x - cube2.position.x + springLength);
            springForceCube2 = springConstant * (cube1.position.x - cube2.position.x + springLength);
            cube1.AddForce(new Vector3(springForceCube1, 0f, 0f));
            cube2.AddForce(new Vector3(springForceCube2, 0f, 0f));
        }
        cube1.AddForce(new Vector3(windForce, 0f, 0f));
        currentTimeStep += Time.deltaTime;
        timeSeries.Add(new List<float>() {currentTimeStep, cube1.position.x, cube1.velocity.x, springForceCube1, cube2.position.x, cube2.velocity.x, springForceCube2, springActive});
    }
    
    void OnApplicationQuit() {
        WriteTimeSeriesToCSV();
    }

    void WriteTimeSeriesToCSV() {
        using (var streamWriter = new StreamWriter("time_series.csv")) {
            streamWriter.WriteLine("t,cube1PositionX(t),cube1Velocity(t),springForceCube1,cube2PositionX(t),cube2Velocity(t),springForceCube2,springActive");
            
            foreach (List<float> timeStep in timeSeries) {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
}
