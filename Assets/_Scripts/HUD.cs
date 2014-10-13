using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    public GUIText ScoreGUI;
    public GUIText StartMsgGUI;
    public GUIText GameOverGUI;
    public string ScoreGUIText;
    public string StartGUIText;
    public string RestartGUIText;
    public SpriteRenderer[] Bombs = new SpriteRenderer[3];
    

    // Update the score GUI element
    public void UpdateScore(int newScore)
    {
        ScoreGUI.text = ScoreGUIText + newScore.ToString();
    }

    // Update the bomb counter HUD
    public void UpdateBombs(int numBombs, int maxBombs)
    {
        // Show the number of bombs the player has
        // Hide the rest of the HUD
        for (int i = 0; i < maxBombs; i++)
        {
            if (i < numBombs)
            {
                Bombs[i].enabled = true;
            }
            else
            {
                Bombs[i].enabled = false;
            }
        }
    }

    // Updates the game over and how to restart msg for each game state
    public void UpdateMessages(GameStates.States gameState)
    {
        switch (gameState)
        {
            case GameStates.States.MENU:
                UpdateScore(0);
                GameOverGUI.guiText.enabled = false;
                StartMsgGUI.guiText.enabled = true;
                StartMsgGUI.text = StartGUIText;
                break;

            case GameStates.States.PLAYING:
                GameOverGUI.guiText.enabled = false;
                StartMsgGUI.guiText.enabled = false;
                break;

            case GameStates.States.GAME_OVER:
                GameOverGUI.guiText.enabled = true;
                StartMsgGUI.guiText.enabled = true;
                StartMsgGUI.text = RestartGUIText;
                break;

            default:
                break;
        }
    }
}
