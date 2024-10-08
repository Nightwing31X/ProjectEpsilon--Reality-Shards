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
    public bool inCutscene = false;
    public bool inDialogue = false;
    public bool inPause = false;


    //[SerializeField] private float gameOverStateDuration = 3f;
    [SerializeField] private float timeToCredit = 3f;
    [SerializeField] private float CreditDuration = 3f;
    [SerializeField] private GameObject CreditContainer;
    private int gameState = 0; // To be used so I know when the player has lost or won.




    private void Awake()
    {
        if (CreditContainer == null)
        {
            Debug.LogWarning("NEED TO PUT IN THE CREDITCONTAINER MENU!!!");
        }
        else
        {
            CreditContainer.SetActive(false);
        }
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

    public void OnEndGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        state = GameStates.EndGame;
    }

    private IEnumerator GameWonSequence(bool restartLevel)
    {
        instance.OnEndGame();
        yield return new WaitForSeconds(timeToCredit);
        CreditContainer.SetActive(true);
        yield return new WaitForSeconds(CreditDuration);
        if (restartLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void TriggerWinState(bool restartLevel)
    {
        if (gameState == 0)
        {
            gameState = 1;
            StartCoroutine(GameWonSequence(restartLevel));
        }
    }

    public void TriggerNextLevelState()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

public enum GameStates
{
    Play, Pause, Menu, Death, EndGame
}