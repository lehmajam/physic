using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2Controller : MonoBehaviour
{
    public Rigidbody cube1;
    private Rigidbody cube2;

    // Start is called before the first frame update
    void Start()
    {
        cube2 = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate can be called multiple times per frame
    void FixedUpdate() {
        
    }
}
