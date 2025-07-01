using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; // Reference to the ground check transform
    [SerializeField] private float groundDist; // Layer mask for the ground
    [SerializeField] private LayerMask groundLayer; // Layer mask for the ground
    [SerializeField] private int totalJumps; // Total number of jumps allowed
    [SerializeField] public Animator anim;

    private int jumpLess; // Number of jumps left
    private bool isGroundedCheck; // Flag to check if the player is grounded
    private bool canJump; // Flag to check if the player can jump



    

    [SerializeField] private float moveSpeed = 5f; // Speed of the player
    [SerializeField] private float jumpForce = 5f; // Jump force of the player
    private float inputDirection; // Direction of the player input
    private bool isDirectionRight = true; // Flag to check if the player is facing right
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    public float KBForce;
    public float KBTime;
    public float KBCount;

    public bool isKnockRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        jumpLess = totalJumps; // Initialize the number of jumps left
    }

    // Update is called once per frame
    void Update()
    {   
        
        GetInputMove(); // Get the input direction
        DirectionCheck(); // Check the direction of the player
        CanJump(); // Check if the player can jump
        MoveAnim(); // Update the animation based on the player's movement
        JumpAnim(); // Update the animation based on the player's jump
        AttackB();
        



    }

    private void FixedUpdate()
    {

        MoveLogic(); // Call the Move function in FixedUpdate for physics calculations
        CheckArea(); // Check if the player is grounded
        KnockLogic(); // Apply knockback logic
        Attack(); // Check for attack input
        
    }

    void KnockLogic()
    {
        

        if (KBCount < 0)
        {
            MoveLogic(); // Call the Move function in FixedUpdate for physics calculations
        }
        else
        {
            if (isKnockRight == true)
            {
                rb.linearVelocity = new Vector2(-KBForce, KBForce); // Apply knockback force to the right  
            }
            if (isKnockRight == false)
            {
                rb.linearVelocity = new Vector2(KBForce, KBForce); // Apply knockback force to the right  
            }
        }

        KBCount -= Time.deltaTime; // Decrease the knockback count
    }

    void CanJump()
    {
        if (isGroundedCheck && rb.linearVelocity.y <= 0) // If the player is grounded
        {
            jumpLess = totalJumps;
        }

        if (jumpLess <= 0)
        {
            canJump = false; // If no jumps left, set canJump to false
        }
        else
        {
            canJump = true; // If jumps are left, set canJump to true
        }
    }

    void CheckArea()
    {
        isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, groundDist, groundLayer); // Check if the player is grounded using a circle overlap

    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(groundCheck.position, groundDist); // Draw a wire sphere at the ground check position
    }

    void DirectionCheck()
    {
        if (isDirectionRight && inputDirection < 0) // If moving left and not facing left
        {
            Flip(); // Flip the player to face  left
        }
        else if (!isDirectionRight && inputDirection > 0) // If moving right and facing right
        {
            Flip(); // Flip the player to face right
        }
    }

    void GetInputMove()
    {
        inputDirection = Input.GetAxisRaw("Horizontal"); // Get the horizontal input (A/D or Left/Right arrow keys)

        if (Input.GetButtonDown("Jump")) // If the jump button is pressed
        {
            jump(); // Call the jump function
        }
    }

    void MoveLogic()
    {
        rb.linearVelocity = new Vector2(inputDirection * moveSpeed, rb.linearVelocity.y); // Set the velocity of the player based on input and speed
    }

    void MoveAnim()
    {
        anim.SetFloat("HorizontalAnim", rb.linearVelocity.x); // Set the animation speed based on the input direction
    }

    void jump()
    {
        if (canJump) // If the player can jump
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply the jump force to the player
            jumpLess--; // Decrease the number of jumps left
        }

    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1")) // If the attack button is pressed
        {
            anim.Play("AttackA", -1); // Trigger the attack animation
        }
    }

    void AttackB()
    {
        if (Input.GetButtonDown("Fire2")) // If the attack button is pressed
        {
            anim.SetTrigger("Atacar"); // Trigger the attack animation
        }
    }


    void JumpAnim()
    {
        anim.SetFloat("VerticalAnim", rb.linearVelocity.y); // Set the jumping animation
        anim.SetBool("groundCheck", isGroundedCheck); // Set the grounded animation
    }
   

    void Flip()
    {
        isDirectionRight = !isDirectionRight; // Toggle the direction flag
        transform.Rotate(0.0f, 180.0f, 0.0f); // Apply the new scale to the player
    }
    
}
