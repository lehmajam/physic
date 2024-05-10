using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2Controller : MonoBehaviour
{
    public Rigidbody cube1;
    private Rigidbody cube2;
    public TextMesh velocityText;

    // Start is called before the first frame update
    void Start()
    {
        cube2 = GetComponent<Rigidbody>();
        velocityText = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        velocityText.text = "xVelocity2: " + cube2.velocity.x.ToString("F2");
    }

    // FixedUpdate can be called multiple times per frame
    void FixedUpdate() {
        
    }

}
