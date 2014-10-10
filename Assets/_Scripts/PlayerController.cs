using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Movement speed
    public float Speed = 300f;
    // Spawn point on game start
    public Vector2 SpawnPoint = new Vector2(0, -2);
    public float BottomBoundary;

    bool IsBeingPushed = false;



    // Use this for initialization
    void Start()
    {
        rigidbody2D.position = SpawnPoint;
    }

    void LateUpdate()
    {

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
        if (transform.position.y < BottomBoundary && !IsBeingPushed)
        {
            transform.position = new Vector2(rigidbody2D.position.x, BottomBoundary);
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

    /*
    void OnTriggerStay2D(Collider2D other)
    {        
        if (!IsBeingPushed)
        {
            Debug.Log("enter");
            rigidbody2D.velocity -= new Vector2(0, rigidbody2D.velocity.y * 1000);
        }
    }
     * */
}
