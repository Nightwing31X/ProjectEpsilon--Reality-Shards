using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state = GameStates.Play;
    public bool isPlayerDead = false;
    public bool inQuest = false;
    public bool inDialogue = false;
    public bool inPause = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }
        OnPlay();
    }
    public void OnPlay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        state = GameStates.Play;
    }
    public void OnPause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        state = GameStates.Pause;
    }
    public void OnMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        state = GameStates.Menu;
    }
    public void OnDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        state = GameStates.Death;
    }
}

public enum GameStates
{
    Play, Pause, Menu, Death
}