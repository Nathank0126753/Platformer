using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using KeyEventArgs.KeyEventArgs;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D bx2d;
    [Range(0, 10f)] [SerializeField] public float moveSpeed = 3f;
    [Range(0, 100f)] [SerializeField] public float jumpForce = 50f;
   // private float jumpForce;
    private bool isJumping = false;
    private bool isTouchingRight = false;
    private bool isTouchingLeft = false;
    private float moveHorizontal;
    private float moveVertical;
    private float counterAction;
    //private float groundDistance;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .25f;
    private bool m_FacingRight = true;
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
        counterAction = 3f;

    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal"); 
        moveVertical = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate(){
        
        isGrounded();
        isTouchingLEFT();
        isTouchingRIGHT();
        if(moveHorizontal > 0.1f || moveHorizontal < -0.1f){
            if (moveHorizontal > 0 && !isTouchingRight)
			{
                Vector3 targetVelocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
			    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
				if (!m_FacingRight){
                    Flip();
                }
			}

			else if (moveHorizontal < 0 && !isTouchingLeft)
			{
                Vector3 targetVelocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
			    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
                if (m_FacingRight){
				    Flip();
                }
			}

        }
        if(moveHorizontal == 0){
                Vector3 haltVelocity = new Vector2(0, rb2d.velocity.y);
			// And then smoothing it out and applying it to the character
			    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, haltVelocity, ref m_Velocity, 0);// m_MovementSmoothing);
        }
        if (moveVertical > 0.1f && !isJumping){
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);//, ForceMode2D.Impulse);
            isJumping = true;
        }
    }
    private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
        Color rayColor;
        RaycastHit2D dist = Physics2D.Raycast(bx2d.bounds.center, -Vector2.up);
        RaycastHit2D Hit1 = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up, 0.5f);
        RaycastHit2D Hit2 = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-0f,-bx2d.bounds.size.y/2, 0f), -Vector2.up, 0.5f);
        RaycastHit2D Hit3 = Physics2D.Raycast(bx2d.bounds.center + new Vector3(+bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up, 0.5f);
        //shortdist = Physics2D.Raycast(transform.position, -Vector2.up, 0.6f);
        //Debug.Log("groundist " + groundDistance);
        if ((Hit1.collider != null || Hit2.collider != null || Hit3.collider != null)  && rb2d.velocity.y == 0){ //&& shortdist.collider != null){
            rayColor = Color.green;
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(0f,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            isJumping = false;
        }
        else{
            isJumping = true;
            rayColor = Color.red;
            Debug.DrawRay(bx2d.bounds.center + new Vector3(0f, -bx2d.bounds.size.y/2, 0f), -Vector2.up*dist.distance, rayColor); //transform.position + -transform.up, rayColor);
            
        }
    }
    public void isTouchingLEFT(){
        Color rayColor;
        RaycastHit2D dist = Physics2D.Raycast(bx2d.bounds.center, Vector2.left);
        RaycastHit2D leftUP = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, +bx2d.bounds.size.y/2, 0f), Vector2.left, 0.025f);
        RaycastHit2D leftDOWN = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, -bx2d.bounds.size.y/2, 0f), Vector2.left, 0.025f);
        if (leftUP.collider != null || leftDOWN.collider != null ){
            isTouchingLeft = true;
            rayColor = Color.red;
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,+bx2d.bounds.size.y/2, 0f), Vector2.left*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), Vector2.left*5.0f, rayColor);
        }
        else{
            isTouchingLeft = false;
            rayColor = Color.green;
            Debug.DrawRay(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, 0f, 0f), Vector2.left*dist.distance, rayColor);
        }
    }
    public void isTouchingRIGHT(){
        Color rayColor;
        RaycastHit2D dist = Physics2D.Raycast(bx2d.bounds.center, Vector2.right);
        RaycastHit2D rightUP = Physics2D.Raycast(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, +bx2d.bounds.size.y/2, 0f), Vector2.right, 0.025f);
        RaycastHit2D rightDOWN = Physics2D.Raycast(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, -bx2d.bounds.size.y/2,0f), Vector2.right, 0.025f);
        if (rightUP.collider != null || rightDOWN.collider != null ){
            isTouchingRight = true;
            rayColor = Color.red;
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,+bx2d.bounds.size.y/2, 0f), Vector2.right*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), Vector2.right*5.0f, rayColor);
        }
        else{
            isTouchingRight = false;
            rayColor = Color.green;
            Debug.DrawRay(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, 0f, 0f), Vector2.right*dist.distance, rayColor);
        }
    }
    
}
