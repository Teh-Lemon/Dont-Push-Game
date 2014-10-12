using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region Variables
    public GameStates.States CurrentState = GameStates.States.MENU;
    // Player object to spawn
    public GameObject PlayerPrefab;
    // Score
    public float ScoreIncreaseIntevalSecs;
    public int ScorePerIncrease;
    public BlockController blockController;
    public HUD hud;
    #endregion

    // Current player object
    GameObject playerInstance;

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
                if (Input.GetButtonDown("Bomb"))
                {
                    StartGame();
                }
                break;

            case GameStates.States.PLAYING:
                // When the player dies, game over
                if (playerInstance == null)
                {
                    GameOver();
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

    #region States
    void MainMenu()
    {
        CurrentState = GameStates.States.MENU;
        score = 0;
        blockController.RemoveAllBlocks();
        hud.UpdateMessages(CurrentState);

        //StartGame();
    }

    void StartGame()
    {
        CurrentState = GameStates.States.PLAYING;
        playerInstance = Instantiate(PlayerPrefab, Vector2.zero, Quaternion.identity) as GameObject;
        StartCoroutine(IncreaseScore());
        blockController.Restart();
        playerInstance.GetComponent<PlayerController>().hud = hud;
        hud.UpdateMessages(CurrentState);
    }

    void GameOver()
    {
        CurrentState = GameStates.States.GAME_OVER;
        hud.UpdateMessages(CurrentState);
    }
    #endregion

    // Regularly increase the score as the game is running
    IEnumerator IncreaseScore()
    {
        while (CurrentState == GameStates.States.PLAYING)
        {
            yield return new WaitForSeconds(ScoreIncreaseIntevalSecs);

            score += ScorePerIncrease;

            hud.UpdateScore(score);
        }
    }
}
