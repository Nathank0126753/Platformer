using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using KeyEventArgs.KeyEventArgs;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D bx2d;
    private SpriteRenderer sp2d;
    [Range(0, 10f)] [SerializeField] public float moveSpeed = 4f;
    [Range(0, 100f)] [SerializeField] public float jumpForce = 10f;
    [Range(0, 100f)] [SerializeField] public float dashForceHorizontal = 10f;
    [Range(0, 100f)] [SerializeField] public float dashForceVertical = 10f;
   // private float jumpForce;
    private bool isJumping = false; 
    private bool isTouchingRight = false;
    private bool isTouchingLeft = false;
  //  private bool isDashing = false;
    private float moveHorizontal;
    private float moveVertical;
   // private float moveDashing;
    private float counterAction;
    private bool isJumpingUpdate = true;
  //  private bool isDashingUpdate = true;
  //  private bool isDashing2 = false;
  //  private float timeLeft = 3f;
    //private float groundDistance;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .25f;
    private bool m_FacingRight = true;
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite1;
    public Sprite newSprite2;
    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 7;
    [SerializeField] private float dashingTime = 0.2f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    const float Move_Delta = 0.1f; 
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
        sp2d = gameObject.GetComponent<SpriteRenderer>();
        counterAction = 3f;

    }

    void Update()
    {
        
        //if (!Input.GetKey("z")){
          //  isJumpingUpdate = true;
        //}
        var dashInput = Input.GetButtonDown("Dash");
        moveHorizontal = Input.GetAxisRaw("Horizontal"); 
        moveVertical = Input.GetAxisRaw("Vertical");
        moveHorizontal = Input.GetAxisRaw("Horizontal"); 
        //moveVertical = Input.GetAxisRaw("Vertical");
        var jumpInput = Input.GetButton("Jump");
        //moveDashing = Input.GetAxisRaw("Dash");
        //rb2d.gravityScale = 2.4f;
        if (Mathf.Abs(moveHorizontal) > 0.1f)
         {
             // this true false may need to be reversed - If its flipping the wrong way, change them the other way.
             sp2d.flipX = moveHorizontal > Move_Delta ? true : false;
         }
        if (m_FacingRight){
            sp2d.flipX = true;
        }
        if (moveHorizontal > 0.1f){
                if (!isTouchingRight){
                    rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);     
                }   
                else if (isJumping){
                    rb2d.velocity = new Vector2(0f, rb2d.velocity.y-rb2d.velocity.y/10); 
                } 
				//if (!m_FacingRight){
              //      Flip();
               // } 
               m_FacingRight = true;
               //sp2d.flipX = true;
                //Vector3 theScale = transform.localScale;
                //theScale = new Vector3(theScale.x*=-1, theScale.y, 0f);// theScale.x *= -1;
                //transform.localScale = theScale;

        }
        if (moveHorizontal < 0.1f){
            if (!isTouchingLeft){
                rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);  
            }
            else if (isJumping){
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y-rb2d.velocity.y/10); 
            }
            m_FacingRight = false;
            //sp2d.flipX = false;
            //Vector3 theScale = transform.localScale;
            //theScale = new Vector3(theScale.x*=-1, theScale.y, 0f);
            //theScale.x *= -1;
            //transform.localScale = theScale;
                //if (m_FacingRight){
            //    Flip();
            //} 

        }
        if ((isTouchingLeft || isTouchingRight) && isJumping){
            isJumping = false;
        }
        if (jumpInput && !isJumping){
            if (isTouchingLeft && moveHorizontal > 0){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            else if (isTouchingRight && moveHorizontal < 0){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            else if (!isTouchingLeft && !isTouchingRight){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            isJumping = true;
            
        }
        //if (moveHorizontal != 0){
       //    transform.localScale = new Vector3(Mathf.Sign(moveHorizontal)*transform.localScale.x, transform.localScale.y, 0);
        //}
       // if (!Input.GetKey("x")){
           // isDashingUpdate = true;
        //}
        //moveHorizontal = Input.GetAxisRaw("Horizontal"); 
       // moveVertical = Input.GetAxisRaw("Vertical");
        //moveDashing = Input.GetAxisRaw("Dash");
        //if (timeLeft <= 0.5 && timeLeft > 0f){
       //     timeLeft = timeLeft-Time.deltaTime;
       //     isDashing2 = true;
       // }
        //if (timeLeft <= 0.01f){
        //    isDashing2 = false;
        //}
        if (dashInput && canDash){
            isDashing = true;
            canDash = false;
            dashingDir = new Vector2(moveHorizontal, moveVertical);

            if (dashingDir == Vector2.zero){
                if (!m_FacingRight){
                    dashingDir = new Vector2(-1, 0);
                }
                else{
                    dashingDir = new Vector2(1, 0);
                }
            }    
            StartCoroutine(StopDashing());
        }
    
        if (isDashing){
            rb2d.velocity = dashingDir.normalized * dashingVelocity;
            sp2d.sprite = newSprite2;
            return;
        }
        if (!isJumping){
            canDash = true;
        }
       // if (isDashing){

       // }
        //if (!isDashing){
        //    sp2d.sprite = newSprite1;
        //}

        
        
    }
    private IEnumerator StopDashing(){
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }
    void FixedUpdate(){
        isGrounded();
        isTouchingLEFT();
        isTouchingRIGHT();
        //moveHorizontal = Input.GetAxisRaw("Horizontal"); 
        //moveVertical = Input.GetAxisRaw("Vertical");
       // var jumpInput = Input.GetButton("Jump");
        //moveDashing = Input.GetAxisRaw("Dash");
        //rb2d.gravityScale = 2.4f;
       // isGrounded();
       // isTouchingLEFT();
        //isTouchingRIGHT();
        //rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
        //if (jumpInput && !isJumping){
            //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
       // }
       // if (moveHorizontal != 0){
       //     transform.localScale = new Vector3(Mathf.Sign(moveHorizontal), 1, 1);
       // }
       // Debug.Log("isDashing2 " + isDashing2 + " Timeleft " + timeLeft);
        //if(moveHorizontal > 0.1f || moveHorizontal < -0.1f){
        //    if (moveHorizontal > 0 && !isTouchingRight)
		//	{
        //        Vector3 targetVelocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
		//	    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		//		if (!m_FacingRight){
         //           Flip();
         //       }
		//	}

		//	else if (moveHorizontal < 0 && !isTouchingLeft)
		//	{
         //       Vector3 targetVelocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
		//	    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
         //       if (m_FacingRight){
		///		    Flip();
         //       }
		//	}

        //}
       // if(moveHorizontal == 0){// && !isDashing2){
       //        Vector3 haltVelocity = new Vector2(0f, rb2d.velocity.y);
       //         rb2d.velocity = haltVelocity;
			// And then smoothing it out and applying it to the character
		//	    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, haltVelocity, ref m_Velocity, 0);// m_MovementSmoothing);
       // }
       // if (moveVertical > 0.1f && !isJumping){
       //     Vector3 targetVelocity = new Vector2(rb2d.velocity.x, moveVertical*jumpForce);
		//	rb2d.velocity = targetVelocity;//(rb2d.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            //rb2d.AddForce(new Vector2(0f, jumpForce*moveVertical), ForceMode2D.Impulse);//, ForceMode2D.Impulse);
       //     isJumping = true;
            //if (!isJumpingUpdate){
            //    Vector3 tv = new Vector2(rb2d.velocity.x, 0f);
            //    rb2d.velocity = tv;
            //}
        //}
    

        //if (moveDashing > 0.1f && !isDashing){
            //only runs if holding right/left keys
          //  if (moveVertical == 0f && moveHorizontal != 0f){
          //      Vector3 dashVelocity = new Vector2(moveHorizontal * dashForceHorizontal, 0f);
		//	    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, dashVelocity, ref m_Velocity, m_MovementSmoothing-0.02f);
          //  }
            //only runs if youre holding left and up/ right and up etc
          //  else if (moveVertical != 0f){
          //      Vector3 dashVelocity = new Vector2(rb2d.velocity.x, moveVertical*dashForceVertical);
          //      rb2d.velocity = dashVelocity;
          //  }
          //  if (moveVertical == 0f && moveHorizontal == 0f){
          //      float direction = -1;
          //      if (m_FacingRight){
           //         direction = 1;
          //      }
          //      Vector3 dashVelocity = new Vector2(direction * dashForceHorizontal, rb2d.velocity.y);
		//	    rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, dashVelocity, ref m_Velocity, m_MovementSmoothing);
          //  }
            //rb2d.gravityScale = 6f;
            //rb2d.drag = 20f;
			//rb2d.AddForce(Vector3.SmoothDamp(rb2d.velocity, dashVeloci))
            //rb2d.velocity = new Vector3(0f, 0f, 0f);
            //rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, dashVelocity, ref m_Velocity, m_MovementSmoothing);
            //rb2d.AddForce(new Vector2(dashForce*moveHorizontal, dashForce*moveVertical), ForceMode2D.Impulse);
       //     isDashing = true;
       //     timeLeft = 0.5f;
       // }
        
        
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
        //Debug.Log("groundist " + Hit2.distance);
        if ((Hit1.collider != null || Hit2.collider != null || Hit3.collider != null)  && rb2d.velocity.y == 0){ //&& shortdist.collider != null){
            rayColor = Color.green;
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(0f,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
           // if (isJumpingUpdate){
                //if (jumpTimeCounter > )
                isJumping = false;
                isDashing = false;
                canDash = true;
                sp2d.sprite = newSprite1;
                //isJumpingUpdate = false;
                //if (isDashingUpdate){
                //    isDashing = false;
               //     isDashingUpdate = false;
                //}
            //}
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
        RaycastHit2D leftDOWN = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, -bx2d.bounds.size.y/2-0.01f, 0f), Vector2.left, 0.025f);
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
        RaycastHit2D rightDOWN = Physics2D.Raycast(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, -bx2d.bounds.size.y/2-0.01f,0f), Vector2.right, 0.025f);
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

