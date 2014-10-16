using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Movement speed
    public float Speed = 300f;

    // Spawn point on game start
    public Vector2 SpawnPoint = new Vector2(0, -2);
    // Y of the bottom of the stage
    public float BottomBoundary = 0;
    // The y position of the player in the previous frame
    float previousTransformY = 0;

    // Bombs available
    int BombsLeft = 0;
    // Number of bombs player is allowed to have at once
    public int MaxBombs = 3;
    // How long the bomb lasts in seconds
    public float BombLength = 0;
    bool isUsingBomb = false;
    // Used to update score counter after each frame
    public int scoreToAdd = 0;
    // Enabled when using bombs
    public SpriteRenderer Eyebrows;

    // HUD reference
    HUD hud;

    // Is the player being pushed by a block
    bool IsBeingPushed = false;
    
        // Use this for initialization
    void Start()
    {
        transform.position = SpawnPoint;
        BombsLeft = MaxBombs;
        isUsingBomb = false;
        Eyebrows.enabled = false;

        if (hud != null)
        {
            hud.UpdateBombs(BombsLeft, MaxBombs);
        }
    }

    void Update()
    {
        // Use bomb when space is hit
        if (Input.GetButtonDown("Bomb"))
        {
            StartCoroutine(UseBomb());
        }

        // Prevent player leaving stage area
        if (!AllowedThroughBoundary)
        {
            transform.position = new Vector2(transform.position.x, BottomBoundary);
        }

        previousTransformY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Grab input
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

    // Detect when player drops off the stage and game over
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DeathBoundary")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "BombPickUp")
        {
            PickUpBomb(other.gameObject);
        }
    }

    // Detect when blocks are pushing the player off the stage
    // Eat blocks during bomb use
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Block")
        {
            if (isUsingBomb)
            {
                EatBlock(other.gameObject);
            }
            else
            {
                IsBeingPushed = true;
            }
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

    IEnumerator UseBomb()
    {
        // Only if the player has a bomb to use
        if (BombsLeft > 0 && !isUsingBomb)
        {
            BombsLeft--;

            // Update bomb counter
            if (hud != null)
            {
                hud.UpdateBombs(BombsLeft, MaxBombs);
            }

            IsBeingPushed = false;
            Eyebrows.enabled = true;

            isUsingBomb = true;
            //Debug.Log("BOMB ON");

            yield return new WaitForSeconds(BombLength);

            isUsingBomb = false;
            Eyebrows.enabled = false;
            //Debug.Log("BOMB OFF");
        }
    }

    // Remove block and award points
    void EatBlock(GameObject block)
    {
        Destroy(block);
        scoreToAdd += block.GetComponent<Block>().PointsValue;
        audio.PlayOneShot(audio.clip);
    }

    public void SetHUD(HUD newHud)
    {
        hud = newHud;
    }

    // Pick up a bomb power up from the stage
    void PickUpBomb(GameObject bomb)
    {
        // Remove bomb power up from stage
        Destroy(bomb);

        // If the player has room to carry the bomb
        if (BombsLeft < MaxBombs)
        {
            // Add bomb to inventory
            BombsLeft++;

            // Update bomb counter
            if (hud != null)
            {
                hud.UpdateBombs(BombsLeft, MaxBombs);
            }
        }

        Debug.Log(BombsLeft);
    }
}
