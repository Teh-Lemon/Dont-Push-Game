using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Movement speed
    public float Speed = 300f;
    // Spawn point on game start
    public Vector2 SpawnPoint = new Vector2(0, -2);
    public float BottomBoundary = 0;

    // Is the player being pushed by a block
    bool IsBeingPushed = false;
    // The y position of the player in the previous frame
    float previousTransformY = 0;

    // Use this for initialization
    void Start()
    {
        rigidbody2D.position = SpawnPoint;
    }

    void Update()
    {
        // Prevent player leaving stage area
        if (!AllowedThroughBoundary)
        {
            transform.position = new Vector2(transform.position.x, BottomBoundary);
        }

        previousTransformY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Grab controller input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Move player
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Set speed cap, stops diagonal movement being too fast
        if (movement.magnitude > Speed)
        {
            movement = movement.normalized * Speed;
        }

        // Move the player
        rigidbody2D.velocity = movement * Speed * Time.deltaTime;

        // Prevent player leaving stage area
        // Stops player leaning through boundary bug
        if (!AllowedThroughBoundary)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            IsBeingPushed = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            IsBeingPushed = false;
        }
    }

    // Prevent player leaving stage area
    // Only if they're not being pushed down by a block
    // Stop player bouncing back up if they stop touching a block
    bool AllowedThroughBoundary
    {
        get
        {
            if (rigidbody2D.position.y < BottomBoundary
                && !IsBeingPushed
                && previousTransformY >= BottomBoundary)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
