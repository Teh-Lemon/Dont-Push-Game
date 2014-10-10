using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
    public float Speed = 5f;
	
	// Update is called once per frame
	void Update () 
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
}
