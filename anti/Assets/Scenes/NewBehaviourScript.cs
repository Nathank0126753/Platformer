using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using KeyEventArgs.KeyEventArgs;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D bx2d;
    private float moveSpeed;
    private float jumpForce;
    private bool isJumping;
    private float moveHorizontal;
    private float moveVertical;
    private float counterAction;
    private Color rayColor;
    //private float groundDistance;
    private RaycastHit2D dist;
    //private RaycastHit2D shortdist;
    //private GameObject groundRayObject;
    // Start is called before the first frame update
    void Start()
    {
        //lowercase gameObject references this.gameobject
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
        //groundDistance = 50f;
        moveSpeed = 3f;
        jumpForce = 80f;
        isJumping = false;
        counterAction = 3f;
        
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
        //invoke("isGrounded", 5.5f);
        isGrounded();
        //have movement code here bc we need unitys physics to run
        //instead of 0, getaxisraw technically returns only 1/-1/0, but its just margin of error for more compledx things
        if(moveHorizontal > 0.1f || moveHorizontal < -0.1f){
            //rb2d.velocity = new Vector2(moveHorizontal*moveSpeed, 0f);//Time.deltaTime, 0f);
            //normally, you might also multiply Time.Deltatime/Time.fixedDeltaTime, but addforce already does that so no need too
            rb2d.AddForce(new Vector2(moveHorizontal*moveSpeed, 0f), ForceMode2D.Impulse); //vector 2 is x/y axis, vector 3 is x/y/z axises

        }
        //else if (isGrounded()){
        //    rb2d.AddForce(-rb2d.velocity*counterAction, ForceMode2D.Impulse);
       // }
        if (moveVertical > 0.1f && !isJumping){
            //groundDistance = dist.distance;
            rb2d.AddForce(new Vector2(0f, moveVertical*jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
        //else{
            //rb2d.AddForce(-rb2d.velocity*counterAction, ForceMode2D.Impulse);
        //}
    }
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Platform"){
            //isJumping = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.tag == "Platform"){
            //isJumping = true;
        }
    }
    public void isGrounded(){
        dist = Physics2D.Raycast(transform.position, -Vector2.up);
        RaycastHit2D Hit1 = Physics2D.Raycast(transform.position + new Vector3(-bx2d.bounds.size.x/2,0f, 0f), -Vector2.up, 0.5f);
        RaycastHit2D Hit2 = Physics2D.Raycast(transform.position + new Vector3(-0f,0f, 0f), -Vector2.up, 0.5f);
        RaycastHit2D Hit3 = Physics2D.Raycast(transform.position + new Vector3(+bx2d.bounds.size.x/2,0f, 0f), -Vector2.up, 0.5f);
        //shortdist = Physics2D.Raycast(transform.position, -Vector2.up, 0.6f);
        //Debug.Log("groundist " + groundDistance);
        if ((Hit1.collider != null || Hit2.collider != null || Hit3.collider != null)  && rb2d.velocity.y == 0){ //&& shortdist.collider != null){
            //if (dist.collider !=null){
            //Debug.Log(dist.distance + " " + dist.collider.name);
            rayColor = Color.green;
            Debug.DrawRay(transform.position  + new Vector3(-bx2d.bounds.size.x/2,0f, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(transform.position  + new Vector3(0f,0f, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(transform.position  + new Vector3(bx2d.bounds.size.x/2,0f, 0f), -Vector2.up*5.0f, rayColor);
            isJumping = false;
            //}
            //else{
            //    rayColor = Color.red;
            //    Debug.DrawRay(transform.position, -Vector2.up*Hit.distance, rayColor);
             //   isJumping = true;
           // }
        }
        //return false;
        else{
            //if (dist.collider != null){
                //Debug.Log(dist.distance + " " + dist.collider.name);
            //}
            isJumping = true;
            rayColor = Color.red;
            Debug.DrawRay(transform.position, -Vector2.up*dist.distance, rayColor); //transform.position + -transform.up, rayColor);
            
        }
        
        //Debug.DrawRay(transform.position , Hit.point, rayColor);
    }
    
}
