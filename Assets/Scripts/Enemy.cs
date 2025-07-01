using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; // Speed of the enemy
    public bool ground = true; // Is the enemy on the ground?
    public Transform GroundCheck; // Position to check for ground
    public LayerMask groundLayer; // Layer of the ground
    public bool facingRight = true; // Is the enemy facing right?
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        ground = Physics2D.Linecast(GroundCheck.position, transform.position, groundLayer);
        Debug.Log(ground);

        if (ground == false)
        {
            speed *= -1; // Reverse direction if not on ground
        }

        if (speed > 0 && !facingRight)
        {
            Flip(); // Flip the enemy if moving right and facing left
        }
        else if (speed < 0 && facingRight)
        {
            Flip(); // Flip the enemy if moving left and facing right
        }
    }

    void Flip()
    {
        facingRight = !facingRight; // Flip the enemy's direction
        Vector3 Scale = transform.localScale;
        Scale.x *= -1; // Invert the x scale
        transform.localScale = Scale; // Apply the new scale
    }
    
    
}


