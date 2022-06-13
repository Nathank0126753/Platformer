using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D bx2d;
    private SpriteRenderer sp2d;
    [Range(0, 10f)] [SerializeField] public float moveSpeed = 4f;
    [Range(0, 100f)] [SerializeField] public float jumpForce = 10f;

    private bool isTouchingRight = false;
    private bool isTouchingLeft = false;

    private float moveHorizontal;
    private float moveVertical;

    private bool m_FacingRight = true;
    public Sprite newSprite1;
    public Sprite newSprite2;
    
    [Header("Dashing")]
    [SerializeField] private float dashingVelocity = 7;
    [SerializeField] private float dashingTime = 0.2f;
    
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    
    public bool isFloored = true;
    const float Move_Delta = 0f; 
    public bool jumpInput = false;
    public bool midJump = false;

    private float side;
    private bool wallGrab = false;
    private bool wallSlide;
    private float slidePower = 1;
    private bool Switch = false;
    public bool isJumping = false; 
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bx2d = gameObject.GetComponent<BoxCollider2D>();
        sp2d = gameObject.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        
        var dashInput = Input.GetButtonDown("Dash");
        moveHorizontal = Input.GetAxisRaw("Horizontal"); 
        moveVertical = Input.GetAxisRaw("Vertical");
        var wallInput = Input.GetButton("Grab");
        if (Input.GetKey("z")){
            if (!Switch){
                jumpInput = true;
            }
            if (Switch){
                jumpInput = false;
            }
        }
        if (!Input.GetKey("z")  && !midJump){
            Switch = false;
        }
        if (Mathf.Abs(moveHorizontal) > 0.1f)
         {

             sp2d.flipX = moveHorizontal > 0f ? true : false;
         }
        
        if (!wallGrab){
            if (moveHorizontal >= 0f){
                    if (!isTouchingRight){
                        rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);  
                        if (moveHorizontal > 0f)
                            m_FacingRight = true;   
                    }   
                    else if (isTouchingRight && !isFloored && !midJump && moveHorizontal > 0f){
                        rb2d.velocity = new Vector2(0f, rb2d.velocity.y-rb2d.velocity.y/10); 
                        m_FacingRight = true;
                    } 
                    else{
                        rb2d.velocity = new Vector2(0f, rb2d.velocity.y);  
                    }
        
                

            }
            if (moveHorizontal <= 0f){
                if (!isTouchingLeft){
                    rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);  
                    if (moveHorizontal < 0f)
                        m_FacingRight = false;
                }
                else if (isTouchingLeft && !isFloored && !midJump && moveHorizontal < 0f){
                    rb2d.velocity = new Vector2(0f, rb2d.velocity.y-rb2d.velocity.y/10); 
                    m_FacingRight = false;
                }
                else{
                    rb2d.velocity = new Vector2(0f, rb2d.velocity.y);  
                }
                

            }
        }
        if (!isJumping && !jumpInput){
            if (isTouchingLeft && moveHorizontal >= 0){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                isJumping = true;
                jumpInput = false;
                midJump = true;
            }
            else if (isTouchingRight && moveHorizontal <= 0){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                isJumping = true;
                jumpInput = false;
                midJump = true;
            }
            else if (!isTouchingLeft && !isTouchingRight){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                isJumping = true;
                jumpInput = false;
                midJump = true;
            }
        
        }
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
            //return;
        }
        if (isFloored){
            canDash = true;
        }
        if (!isDashing){
            sp2d.sprite = newSprite1;
        }
        if (isFloored){
            canDash = true;
        }

       // if ((isTouchingLeft || isTouchingRight) && wallInput){
       ///     wallGrab = true;
       //     wallSlide = false;
       // }
       // if ((!isTouchingLeft && !isTouchingRight) || Input.GetButtonUp("Grab")){
       //     wallGrab = false;
       //     wallSlide = false;
       // }
        //if (wallGrab){
        //    rb2d.gravityScale = 0f;
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        //    float speedModifier = moveVertical > 0? 0.35f : 1;
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, moveVertical * (moveSpeed * speedModifier));
         //}
        //else{
        ///    rb2d.gravityScale = 2.35f;
       // }
        //if ((isTouchingLeft || isTouchingRight) && !isFloored){
            //if (!wallGrab){
                //wallSlide = true;
                //WallSlide();
           // }
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
        
        
    }
    public void WallSlide(){
        bool pushingWall = false;
        if ((isTouchingRight && rb2d.velocity.x>0) || (isTouchingLeft && rb2d.velocity.x<0)){
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb2d.velocity.x;
        rb2d.velocity = new Vector2(push, -slidePower);
    }
    

    public void isGrounded(){
        Color rayColor;
        RaycastHit2D dist = Physics2D.Raycast(bx2d.bounds.center, -Vector2.up);
        RaycastHit2D Hit1 = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up, 0.5f);
        RaycastHit2D Hit2 = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-0f,-bx2d.bounds.size.y/2, 0f), -Vector2.up, 0.5f);
        RaycastHit2D Hit3 = Physics2D.Raycast(bx2d.bounds.center + new Vector3(+bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up, 0.5f);

        if ((Hit1.collider != null || Hit2.collider != null || Hit3.collider != null)  && rb2d.velocity.y == 0){ 
            rayColor = Color.green;
            //comment out the Debug.DrawRay to remove the weird lines
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(0f,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), -Vector2.up*5.0f, rayColor);
            if (jumpInput){
                jumpInput = false;
                Switch = true;
                isJumping = false;
                sp2d.sprite = newSprite1;
            }
            midJump = false;
            isFloored = true;
        }
        else{
            isJumping = true;
            isFloored = false;
            rayColor = Color.red;
            //comment out the Debug.DrawRay to remove the weird lines
            Debug.DrawRay(bx2d.bounds.center + new Vector3(0f, -bx2d.bounds.size.y/2, 0f), -Vector2.up*dist.distance, rayColor); 
            
        }
    }
    public void isTouchingLEFT(){
        Color rayColor;
        RaycastHit2D dist = Physics2D.Raycast(bx2d.bounds.center, Vector2.left);
        RaycastHit2D leftUP = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, +bx2d.bounds.size.y/2, 0f), Vector2.left, 0.025f);
        RaycastHit2D leftDOWN = Physics2D.Raycast(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, -bx2d.bounds.size.y/2-0.01f, 0f), Vector2.left, 0.025f);
        if (leftUP.collider != null || leftDOWN.collider != null ){
            if (isJumping && !isFloored && moveHorizontal != 0 && rb2d.velocity.y != 0 && jumpInput){
                isJumping = false;
                jumpInput = false;
                Switch = true;
            }
            
            midJump = false;
            isTouchingLeft = true;
            rayColor = Color.red;
            //comment out the Debug.DrawRay to remove the weird lines
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,+bx2d.bounds.size.y/2, 0f), Vector2.left*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(-bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), Vector2.left*5.0f, rayColor);
        }
        else{
            isTouchingLeft = false;
            rayColor = Color.green;
            //comment out the Debug.DrawRay to remove the weird lines
            Debug.DrawRay(bx2d.bounds.center + new Vector3(-bx2d.bounds.size.x/2, 0f, 0f), Vector2.left*dist.distance, rayColor);
        }
    }
    public void isTouchingRIGHT(){
        Color rayColor;
        RaycastHit2D dist = Physics2D.Raycast(bx2d.bounds.center, Vector2.right);
        RaycastHit2D rightUP = Physics2D.Raycast(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, +bx2d.bounds.size.y/2, 0f), Vector2.right, 0.025f);
        RaycastHit2D rightDOWN = Physics2D.Raycast(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, -bx2d.bounds.size.y/2-0.01f,0f), Vector2.right, 0.025f);
        if (rightUP.collider != null || rightDOWN.collider != null ){
            if (isJumping && !isFloored && moveHorizontal != 0 && rb2d.velocity.y != 0 && jumpInput){
                isJumping = false;
                jumpInput = false;
                Switch = true;
            }
            midJump = false;
            isTouchingRight = true;
            rayColor = Color.red;
            //comment out the Debug.DrawRay to remove the weird lines
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,+bx2d.bounds.size.y/2, 0f), Vector2.right*5.0f, rayColor);
            Debug.DrawRay(bx2d.bounds.center  + new Vector3(bx2d.bounds.size.x/2,-bx2d.bounds.size.y/2, 0f), Vector2.right*5.0f, rayColor);
        }
        else{
            isTouchingRight = false;
            rayColor = Color.green;
            //comment out the Debug.DrawRay to remove the weird lines
            Debug.DrawRay(bx2d.bounds.center + new Vector3(bx2d.bounds.size.x/2, 0f, 0f), Vector2.right*dist.distance, rayColor);
        }
    }
    
}
