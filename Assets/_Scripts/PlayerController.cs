using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed = 300f;

    public Vector2 SpawnPoint = new Vector2(0, -2);

    // Use this for initialization
    void Start()
    {
        rigidbody2D.position = SpawnPoint;
    }

    // Update is called once per frame
    void Update()
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
    }
}
