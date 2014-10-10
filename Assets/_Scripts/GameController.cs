using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public GameStates.States CurrentState = GameStates.States.MENU;

	// Use this for initialization
	void Start () 
	{
        CurrentState = GameStates.States.PLAYING;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
