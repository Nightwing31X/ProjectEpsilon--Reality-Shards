using System.Collections;
using System.Collections.Generic;
using GameDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Menu;


namespace Interactions
{
    [AddComponentMenu("GameDev/Interactions/Open Book")]

    public class OpenBook : MonoBehaviour, IInteractable
    {
        public GameObject textContainer;
        public Text textOBJ;
        public GameObject customTextContainer;
        public GameObject KeyboardText;
        public GameObject ControllerText;
        [Tooltip("The text that will be displayed when the player interacts with the book.")]
        [SerializeField][TextArea] private string _readingContent;
        //public Animator bookAnim;
        public GameObject bookOBJ;
        public GameObject closeButton;
        [SerializeField] private bool _isReading = false;
        [SerializeField] private bool _customText = false;
        [SerializeField] private bool _fireOnce = false;
        [SerializeField] private bool _events = false;
        [SerializeField] private UnityEvent _customEvents;

        public AudioSource openedBook;
        public AudioSource closedBook;
        public AudioSource pageTurn;
        // Start is called before the first frame update
        void Start()
        {
            _isReading = false;
            textContainer.SetActive(false);
            if (!_customText)
            {
                textOBJ.enabled = false;
                customTextContainer.SetActive(false);
            }
        }

        void Update()
        {
            if (_isReading)
            {
                if (!InputHandler.instance.forceController)
                {
                    if (InputHandler.instance.onController)
                    {
                        ControllerText.SetActive(true);
                        KeyboardText.SetActive(false);
                    }
                    else if (InputHandler.instance.onKeyboard)
                    {
                        KeyboardText.SetActive(true);
                        ControllerText.SetActive(false);
                    }
                }
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
            }
        }

        void openBook()
        {
            if (InputHandler.instance.forceController)
            {
                ControllerText.SetActive(true);
                KeyboardText.SetActive(false);
            }
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(closeButton);
            _isReading = true;
            if (GameManager.instance.state != GameStates.Menu)
            {
                GameManager.instance.OnMenu();
                GameManager.instance.inBook = _isReading;
            }
            if (!_customText)
            {
                customTextContainer.SetActive(false);
                textOBJ.enabled = true;
                textOBJ.text = string.Format(_readingContent);
            }
            else
            {
                if (textOBJ != null)
                {
                    textOBJ.enabled = false;
                }
                customTextContainer.SetActive(true);
            }
            textContainer.SetActive(_isReading);
            openedBook.Play();
            if (_fireOnce)
            {
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Default");
                bookOBJ.layer = LayerIgnoreRaycast;
            }
        }

        public void nextPage()
        {
            Debug.Log("Right side button...");
            pageTurn.Play();
        }

        public void backPage()
        {
            Debug.Log("Left side button");
            pageTurn.Play();
        }

        public void exitBook()
        {
            Debug.Log("Close book button");
            _isReading = false;
            textContainer.SetActive(_isReading);
            closedBook.Play();
            if (GameManager.instance.state != GameStates.Play)
            {
                GameManager.instance.OnPlay();
                GameManager.instance.inBook = _isReading;
            }
            if (_events)
            {
                _customEvents.Invoke();
            }
        }

        public void Interact()
        {
            if (!_isReading)
            {
                openBook();
            }
        }
    }
}
