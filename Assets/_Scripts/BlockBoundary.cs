using UnityEngine;
using System.Collections;

public class BlockBoundary : MonoBehaviour
{
    // Remove blocks from play when they leave the stage
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Block")
        {
            Destroy(other.gameObject);
        }
    }
}
