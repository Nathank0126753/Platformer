using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using KeyEventArgs.KeyEventArgs;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        //if (Input.GetKeyDown(KeyCode.Space)){
          //  Debug.Log("saweaweawee" + "x pos: " + x + " y pos: " + y);
            //transform.position = new Vector3(x+3000, y+3000, 2);
        //}
    }
    
}
