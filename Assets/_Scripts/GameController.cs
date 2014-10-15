using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region Variables
    public GameStates.States CurrentState = GameStates.States.MENU;

    // Time between each score increase
    public float ScoreIncreaseIntevalSecs;
    // How much score is awarded per increase
    public int ScorePerIncrease;

    // Player object to spawn
    public GameObject PlayerPrefab;
    public BlockController blockController;
    public HUD hud;
	public AudioSource MenuConfirmSound;
    #endregion

    // Current player object
    PlayerController playerController;

    // Current score
    int score;

    // Use this for initialization
    void Start()
    {
        MainMenu();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case GameStates.States.MENU:
                // Wait for the player to press the button before starting game
                if (Input.GetButtonDown("Bomb"))
                {
                    StartGame();
                }
                break;

            case GameStates.States.PLAYING:
                // When the player dies, game over
                if (playerController == null)
                {
                    GameOver();
                }
                else
                {
                    // Add the score from eating blocks
                    AddScore(playerController.scoreToAdd);
                    playerController.scoreToAdd = 0;
                }
                break;

            case GameStates.States.GAME_OVER:
                if (Input.GetButtonDown("Bomb"))
                {
                    MainMenu();
                    StartGame();
                }
                break;

            default:
                break;
        }
    }

    // Execute when changing game states
    #region States
    void MainMenu()
    {
        // Update state
        CurrentState = GameStates.States.MENU;
        // Reset the score from the previous play session
        score = 0;
        // Remove blocks from previous play sessions 
        blockController.RemoveAllBlocks();
        // Show the start up message
        hud.UpdateMessages(CurrentState);
    }

    void StartGame()
    {
        // Update state
        CurrentState = GameStates.States.PLAYING;
        // Spawn the player
        GameObject playerInstance = Instantiate(PlayerPrefab, Vector2.one, Quaternion.identity) as GameObject;
        playerController = playerInstance.GetComponent<PlayerController>();
        playerController.SetHUD(hud);
        // Start counting the score
        StartCoroutine(BuildScore());
        // Reset the wave spawner timer
        blockController.Restart();
        // Remove the start message
        hud.UpdateMessages(CurrentState);
        // Play ok sound effect
        MenuConfirmSound.Play();
    }

    void GameOver()
    {
        // Update state
        CurrentState = GameStates.States.GAME_OVER;
        // Show the game over and restart message
        hud.UpdateMessages(CurrentState);
        // Play dying sound
		audio.Play();
    }
    #endregion

    // Regularly increase the score as the game is running
    IEnumerator BuildScore()
    {
        while (CurrentState == GameStates.States.PLAYING)
        {
            yield return new WaitForSeconds(ScoreIncreaseIntevalSecs);

            AddScore(ScorePerIncrease);
        }
    }

    // Used to increase the score value and make sure the HUD is updated
    public void AddScore(int addScore)
    {
        score += addScore;
        hud.UpdateScore(score);
    }
}
