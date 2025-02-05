using System.Collections;
using System.Collections.Generic;
using GameDev;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Player
{
    [AddComponentMenu("GameDev/Player/Pause Game")]
    public class PauseGame : MonoBehaviour
    {
        [SerializeField] GameObject MenuPaused;
        [SerializeField] GameObject PlayerHUD;
        [SerializeField] Button restartPauseBTN;
        GameObject player;

        [SerializeField] bool PausedMenu = false;
        //[SerializeField] bool _inDialogue;
        [SerializeField] bool _inCutscene;

        public GameObject pausedmenuFirstObject;

        // Start is called before the first frame update
        void Start()
        {
            //Time.timeScale = 1;
            PausedMenu = false;
            MenuPaused.SetActive(PausedMenu);

            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.instance.inBook)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                var pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
                EventSystem.current.RaycastAll(pointerEventData, results);
                foreach (var r in results)
                {
                    if (r.gameObject && r.gameObject.TryGetComponent(out Selectable sel))
                    {
                        if (sel.interactable)
                        {
                            EventSystem.current.SetSelectedGameObject(r.gameObject);
                            break;
                        }
                    }
                }
                if (!PausedMenu && Input.GetButtonUp("Pause"))
                {
                    SelectObjectUI();
                    //Time.timeScale = 0;
                    GameManager.instance.OnPause();
                    GameManager.instance.inPause = true;
                    PausedMenu = true;
                    MenuPaused.SetActive(PausedMenu);
                    PlayerHUD.SetActive(false);

                    // Cursor.lockState = CursorLockMode.None;
                    // Cursor.visible = true;
                    _inCutscene = GameManager.instance.inCutscene;
                    if (_inCutscene)
                    {
                        restartPauseBTN.interactable = false; //? This leaves the button on the screen with the disabled color and the user can’t click it
                        //restartPauseBTN.enabled = false; //? This leaves the button on the screen, the user can’t click it, but does NOT use the disabled color
                        //restartPauseBTN.gameObject.SetActive(false); //? This removes the button from the UI entirely:
                    }
                    else
                    {
                        restartPauseBTN.interactable = true;
                    }
                }
                else if (PausedMenu && Input.GetButtonUp("Pause"))
                {
                    //Time.timeScale = 1;
                    CheckState();
                    PausedMenu = false;
                    MenuPaused.SetActive(PausedMenu);
                    PlayerHUD.SetActive(true);
                }
            }
        }

        public void SelectObjectUI()
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(pausedmenuFirstObject);
        }

        private void CheckState()
        {
            GameManager.instance.inPause = false;
            _inCutscene = GameManager.instance.inCutscene;
            if (_inCutscene)
            {
                GameManager.instance.OnMenu();
            }
            else
            {
                GameManager.instance.OnPlay();
            }
        }

        public void Resume()
        {
            //Time.timeScale = 1;
            CheckState();
            PausedMenu = false;
            MenuPaused.SetActive(PausedMenu);
            PlayerHUD.SetActive(true);
        }

        public void Restart()
        {
            // player.GetComponent<Health>().Respawn();
            // PausedMenu = false;
            // MenuPaused.SetActive(PausedMenu);
            // PlayerHUD.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Exit(string LevelName)
        {
            SceneManager.LoadScene(LevelName);
            //Application.Quit();
        }
    }
}