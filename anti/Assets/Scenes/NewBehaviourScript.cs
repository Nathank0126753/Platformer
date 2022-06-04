using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using KeyEventArgs.KeyEventArgs;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    private float moveSpeed;
    private float jumpForce;
    private bool isJumping;
    private float moveHorizontal;
    private float moveVertical;
    // Start is called before the first frame update
    void Start()
    {
        //lowercase gameObject references this.gameobject
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        moveSpeed = 5f;
        jumpForce = 60f;
        isJumping = false;
        
    }
    // Update is called once per frame

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal"); //are they clicking a/d or left/right can be changed in project settings
        //if presing a/left, it returns -1, if pressing d/right, it returns 1, otherwise it returns 0
        moveVertical = Input.GetAxisRaw("Vertical");
        // Make it move 10 meters per second instead of 10 meters per frame...
        // Move translation along the object's z-axis
        // Rotate around our y-axis
        //transform.Rotate(0, rotation, 0);
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        //if (Input.GetKeyDown(KeyCode.Space)){
          //  Debug.Log("saweaweawee" + "x pos: " + x + " y pos: " + y);
            //transform.position = new Vector3(x+3000, y+3000, 2);
        //}
    }
    void FixedUpdate(){
        //have movement code here bc we need unitys physics to run
        //instead of 0, getaxisraw technically returns only 1/-1/0, but its just margin of error for more compledx things
        if(moveHorizontal > 0.1f || moveHorizontal < -0.1f){
            //normally, you might also multiply Time.Deltatime/Time.fixedDeltaTime, but addforce already does that so no need too
            rb2d.AddForce(new Vector2(moveHorizontal*moveSpeed, 0f), ForceMode2D.Impulse); //vector 2 is x/y axis, vector 3 is x/y/z axises

        }
        if (!isJumping && moveVertical > 0.1f){
            rb2d.AddForce(new Vector2(0f, moveVertical*jumpForce), ForceMode2D.Impulse);
        }
    }
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Platform"){
            isJumping = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.tag == "Platform"){
            isJumping = true;
        }
    }
    
}
