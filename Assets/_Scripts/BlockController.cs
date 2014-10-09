using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    // Block prefab to spawn
    public GameObject blockPrefab;

    // Available block images
    Sprite[] blockSprites;

    void Awake()
    {
        // Load the block images
        blockSprites = Resources.LoadAll<Sprite>("food");
    }

    void Start()
    {
        SpawnBlock();
    }

    void SpawnBlock()
    {
        GameObject newBlock = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        // Set a random image (ignore last image, used for bomb)
        int imageID = Random.Range(0, blockSprites.Length - 2);
        newBlock.GetComponent<Block>().SetImage(blockSprites[imageID]);
    }
}
