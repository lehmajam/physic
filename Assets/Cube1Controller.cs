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
    private float u1; // m/s
    private float u2; // m/s
    private float v1; // m/s
    private float v2; // m/s
    private float currentTimeStep; // s
    private List<List<float>> timeSeries;

    // Start is called before the first frame update
    void Start()
    {
        cube1 = GetComponent<Rigidbody>();
        timeSeries = new List<List<float>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate can be called multiple times per frame
    void FixedUpdate() {
        if (cube1.position.x > cube2.position.x - springLength) {
            windForce = 0f;
            float springForceCube1 = -springConstant * (cube1.position.x - cube2.position.x + springLength);
            float springForceCube2 = springConstant * (cube1.position.x - cube2.position.x + springLength);
            cube1.AddForce(new Vector3(springForceCube1, 0f, 0f));
            cube2.AddForce(new Vector3(springForceCube2, 0f, 0f));
        }
        cube1.AddForce(new Vector3(windForce, 0f, 0f));

        currentTimeStep += Time.deltaTime;
        timeSeries.Add(new List<float>() {currentTimeStep, cube1.position.x, cube1.velocity.x, windForce, cube2.position.x, cube2.velocity.x});
    }

    void OnApplicationQuit() {
        WriteTimeSeriesToCSV();
    }

    void WriteTimeSeriesToCSV() {
        using (var streamWriter = new StreamWriter("time_series.csv")) {
            streamWriter.WriteLine("t,cube1PositionX(t),cube1Velocity(t),windF(t) (added),cube2PositionX(t),cube2Velocity(t)");
            
            foreach (List<float> timeStep in timeSeries) {
                streamWriter.WriteLine(string.Join(",", timeStep));
                streamWriter.Flush();
            }
        }
    }
}
