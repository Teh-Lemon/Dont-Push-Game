using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    #region Variables
    // Block prefab to spawn
    public GameObject BlockPrefab;
    // Bomb prefab to spawn
    public GameObject BombPrefab;
    // Time before the blocks spawn at the start of each game
    public int SpawnStartDelay;
    // Time between each block spawn
    public float SpawnInterval;
    // Minimum point the block can spawn
    public Vector2 SpawnMinPosition;
    // How far the block can spawn from the min
    public float SpawnXRange;
    // Range of random block sizes
    public float MinBlockScale;
    public float MaxBlockScale;
    // Percentage range of speed
    public float BlockSpeedRange;
    // Number of blocks to spawn at once
    public int BlocksPerWave;
    // Chance of bomb spawning per second
    public float BombPickUpSpawnChance;
    // How long the bomb stays on the stage for before expiring
    public float BombPickUpLifeTime;
    public Vector2 BombMinSpawnPoint;
    public Vector2 BombMaxSpawnPoint;
    #endregion

    // Game Controller reference
    GameController gameC;
    // Available block images
    Sprite[] blockSprites;
    // Used for cycling through the block images
    int blocksSpawned;
    GameObject bombInstance;

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

        Restart();
    }

    public void Restart()
    {
        // Continually spawn blocks if game is running
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnBombWaves());
        //SpawnBlock();
    }

    // Continually spawn blocks while the game is PLAYING
    IEnumerator SpawnWaves()
    {
        // Wait a bit before starting the game
        yield return new WaitForSeconds(SpawnStartDelay);

        // Spawn blocks when the game is playing
        while (gameC.CurrentState == GameStates.States.PLAYING)
        {
            // Spawn block wave
            for (int i = 0; i < BlocksPerWave; i++)
            {
                SpawnBlock();
            }

            // Wait between each block spawn
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    // Continually spawn blocks while the game is PLAYING
    IEnumerator SpawnBombWaves()
    {
        // Wait a bit before starting the game
        yield return new WaitForSeconds(SpawnStartDelay);

        // Spawn blocks when the game is playing
        while (gameC.CurrentState == GameStates.States.PLAYING)
        {

            // Spawn bomb pick up
            if (Random.value < BombPickUpSpawnChance)
            {
                StartCoroutine(SpawnBomb());
            }

            // Wait between each block spawn
            yield return new WaitForSeconds(1);
        }
    }

    // Spawns a new block at the top of the stage at a random position and image
    void SpawnBlock()
    {
        // Random spawn point
        float spawnX = Random.Range(0, SpawnXRange);
        Vector2 spawnPoint = SpawnMinPosition + new Vector2(spawnX, 0);

        // Spawn new Block
        GameObject newBlock = Instantiate(BlockPrefab, spawnPoint, Quaternion.identity) as GameObject;

        // Set a random image (ignore last image which is used for the bomb)
        //int imageID = Random.Range(0, blockSprites.Length - 2);
        int imageID = blocksSpawned % (blockSprites.Length - 2);
        newBlock.GetComponent<Block>().SetImage(blockSprites[imageID]);

        // Randomize scale of block
        newBlock.GetComponent<Block>().RandomizeScale(MinBlockScale, MaxBlockScale);

        // Randomize block speed
        newBlock.GetComponent<Block>().RandomizeSpeed(BlockSpeedRange);

        blocksSpawned++;
    }

    // Remove all the active blocks and bombs from the game
    public void RemoveAllBlocks()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blocks.Length; i++)
        {
            Destroy(blocks[i]);
        }

        blocks = GameObject.FindGameObjectsWithTag("BombPickUp");
        for (int i = 0; i < blocks.Length; i++)
        {
            Destroy(blocks[i]);
        }
    }

    // Add a bomb to the stage if there isn't already one
    IEnumerator SpawnBomb()
    {
        // Only if there isn't already a bomb on stage
        if (bombInstance == null)
        {
            // Random spawn point
            float spawnX = Random.Range(BombMinSpawnPoint.x, BombMaxSpawnPoint.x);
            float spawnY = Random.Range(BombMinSpawnPoint.y, BombMaxSpawnPoint.y);
            Vector2 spawnPoint = new Vector2(spawnX, spawnY);

            // Spawn new Block
            bombInstance = Instantiate(BombPrefab, spawnPoint, Quaternion.identity) as GameObject;

            // If the bomb hasn't been picked up after the expire time, remove it
            yield return new WaitForSeconds(BombPickUpLifeTime);
            if (bombInstance != null)
            {
                Destroy(bombInstance);
            }
        }
    }
}
