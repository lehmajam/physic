using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Rigidbody Cube1;
    public Rigidbody Cube2;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        transform.position = (Cube1.position + Cube2.position) / 2 + offset;
    }
}
