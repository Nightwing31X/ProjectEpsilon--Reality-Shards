using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OpenBook : MonoBehaviour, IInteractable
{
    public GameObject textContainer;
    public Text textOBJ;
    [Tooltip("The text that will be displayed when the player interacts with the book.")]
    [SerializeField][TextArea] private string _readingContent;
    //public Animator bookAnim;
    public GameObject bookOBJ;
    [SerializeField] private bool _isReading = false;
    [SerializeField] private bool _customText = false;
    [SerializeField] private bool _fireOnce = false;

    public AudioSource openedBook;
    public AudioSource closedBook;
    public AudioSource pageTurn;
    // Start is called before the first frame update
    void Start()
    {
        _isReading = false;
        textContainer.SetActive(false);
    }

    void openBook()
    {
        _isReading = true;
        if (GameManager.instance.state != GameStates.Menu)
        {
            GameManager.instance.OnMenu();
        }
        if (!_customText)
        {
            textOBJ.text = string.Format(_readingContent);
            textContainer.SetActive(_isReading);
        }
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
        }
    }

    public void Interact()
    {
        openBook();
    }
}
