using System.Collections;
using System.Collections.Generic;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameDev
{
    [AddComponentMenu("GameDev/Game Manger")]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameStates state = GameStates.Play;
        public bool isPlayerDead = false;
        public bool inQuest = false;
        public bool inCutscene = false;
        public bool inDialogue = false;
        public bool inDialoguePlace = false;
        public bool inPause = false;
        public bool inBook = false;
        public string currentGameState;


        //[SerializeField] private float gameOverStateDuration = 3f;
        [SerializeField] private float timeToCredit = 3f;
        [SerializeField] private float CreditDuration = 3f;
        [SerializeField] private GameObject CreditContainer;


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
            if (CreditContainer == null)
            {
                Debug.LogWarning("NEED TO PUT IN THE CREDITCONTAINER MENU!!!");
            }
            else
            {
                CreditContainer.SetActive(false);
            }
            OnPlay();
        }

        public void CheckCurrentStat()
        {
            if (state == GameStates.Play)
            {
                OnPlay();
            }
            else if (state == GameStates.Pause)
            {
                OnPause();
            }
            else if (state == GameStates.Menu)
            {
                OnMenu();
            }
            else if (state == GameStates.Death)
            {
                OnDeath();
            }
            else if (state == GameStates.EndGame)
            {
                OnEndGame();
            }
            else 
            {
                Debug.LogWarning("GAME MANAGER - THIS SHOULD NOT SHOW AS CURRENT STATE!!!");
            }
        }

        public void OnPlay()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            state = GameStates.Play;
            currentGameState = "Play";
        }
        public void OnPause()
        {
            Cursor.lockState = CursorLockMode.None;
            if (InputHandler.instance.forceController)
            {
                Cursor.visible = false;
            }
            if (InputHandler.instance.onController)
            {
                Cursor.visible = false;
            }
            if (InputHandler.instance.onKeyboard)
            {
                Cursor.visible = true;
            }
            state = GameStates.Pause;
            currentGameState = "Pause";
        }
        public void OnMenu()
        {
            Cursor.lockState = CursorLockMode.Confined;
            if (InputHandler.instance.forceController)
            {
                Cursor.visible = false;
            }
            if (InputHandler.instance.onController)
            {
                Cursor.visible = false;
            }
            if (InputHandler.instance.onKeyboard)
            {
                Cursor.visible = true;
            }
            state = GameStates.Menu;
            currentGameState = "Menu";
        }
        public void OnDeath()
        {
            Cursor.lockState = CursorLockMode.None;
            if (!InputHandler.instance.forceController)
            {
                Cursor.visible = false;
            }
            if (InputHandler.instance.onController)
            {
                Cursor.visible = false;
            }
            if (InputHandler.instance.onKeyboard)
            {
                Cursor.visible = true;
            }
            state = GameStates.Death;
            currentGameState = "Death";
        }

        public void OnEndGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            state = GameStates.EndGame;
            currentGameState = "EndGame";
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
            StartCoroutine(GameWonSequence(restartLevel));
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
}