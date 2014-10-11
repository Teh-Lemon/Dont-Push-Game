using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public float Speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.position += (new Vector3(0, -Speed, 0) * Time.deltaTime);
    }

    // Set the block's image to the provided sprite
    public void SetImage(Sprite newImage)
    {
        GameObject blockImage = new GameObject();
        blockImage.AddComponent<SpriteRenderer>();
        blockImage.GetComponent<SpriteRenderer>().sprite = newImage;
        blockImage.transform.position = transform.position;
        blockImage.transform.parent = transform;
    }

    // Set the scale to a random value between the given range
    public void RandomizeScale(float min, float max)
    {
        transform.localScale = Vector3.one * Random.Range(min, max);
    }

    // Set the speed to a random percentage from the original speed
    // e.g + or - 25% from the original 100% speed
    public void RandomizeSpeed(float percentRange)
    {
        float difference = Random.value * percentRange;
        Speed += ((Speed * difference) * RandomSign());
    }

    // Returns either a 1 or -1
    int RandomSign()
    {
        if (Random.value < 0.5)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
