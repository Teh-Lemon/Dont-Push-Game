using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    // Block prefab to spawn
    public GameObject blockPrefab;
    // Time before the blocks spawn at the start of each game
    public int SpawnStartDelay;
    // Time between each block spawn
    public float SpawnInterval;
    // Minimum point the block can spawn
    public Vector2 SpawnMinPosition;
    // How far the block can spawn from the min
    public float SpawnXRange;

    // Game Controller reference
    GameController gameC;
    // Available block images
    Sprite[] blockSprites;


    void Awake()
    {
        // Load the block images
        blockSprites = Resources.LoadAll<Sprite>("food");
    }

    void Start()
    {
        // Find the game controller script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameC = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        //StartCoroutine(SpawnBlockWaves());
        SpawnBlock();
    }

    // Continually spawn blocks while the game is PLAYING
    IEnumerator SpawnBlockWaves()
    {
        // Wait a bit before starting the game
        yield return new WaitForSeconds(SpawnStartDelay);

        // Spawn blocks when the game is playing
        while (gameC.CurrentState == GameStates.States.PLAYING)
        {
            SpawnBlock();

            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    // Spawns a new block at the top of the stage at a random position and image
    void SpawnBlock()
    {
        // Random spawn point
        float spawnX = Random.Range(0, SpawnXRange);
        Vector2 spawnPoint = SpawnMinPosition + new Vector2(spawnX, 0);

        // Spawn new Block
        GameObject newBlock = Instantiate(blockPrefab, spawnPoint, Quaternion.identity) as GameObject;

        // Set a random image (ignore last image, used for bomb)
        int imageID = Random.Range(0, blockSprites.Length - 2);
        newBlock.GetComponent<Block>().SetImage(blockSprites[imageID]);
    }
}
