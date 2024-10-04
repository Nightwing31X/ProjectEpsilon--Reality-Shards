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
    public Animator bookAnim;
    public GameObject bookOBJ;
    [SerializeField] float _secondsOfDelay;

    [SerializeField] private bool _isReading = false;

    [SerializeField] private bool _fireOnce = false;
    [SerializeField] private bool _deylayBeforeText = false;

    public AudioSource openedBook;
    public AudioSource closedBook;
    public AudioSource pageTurn;
    // Start is called before the first frame update
    void Start()
    {
        _isReading = false;
        textContainer.SetActive(false);
        if (bookAnim != null )
        {
            _deylayBeforeText = true;
        }
    }

    void openBook()
    {
        _isReading = true;
        if (GameManager.instance.state != GameStates.Menu)
        {
            GameManager.instance.OnMenu();
        }
        if (bookAnim != null)
        {
            Debug.Log("I have a animation...Need to finish coding this section...");
        }
        else
        {
            textOBJ.text = string.Format(_readingContent);
            textContainer.SetActive(_isReading);
        }
        if (bookAnim != null)
        {
            openedBook.Play();
        }
        if (_fireOnce)
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Default");
            bookOBJ.layer = LayerIgnoreRaycast;
        }
    }

    public void nextPage()
    {
        Debug.Log("Right side button...");
        if (bookAnim != null)
        {
            pageTurn.Play();
        }
    }

    public void backPage()
    {
        Debug.Log("Left side button");
        if (bookAnim != null)
        {
            pageTurn.Play();
        }
    }

    public void exitBook()
    {
        Debug.Log("Close book button");
        _isReading = false;
        textContainer.SetActive(_isReading);
        if (bookAnim != null)
        {
            closedBook.Play();
        }
        if (GameManager.instance.state != GameStates.Play)
        {
            GameManager.instance.OnPlay();
        }
    }

    private IEnumerator DelayOpenBook()
    {
        yield return new WaitForSeconds(_secondsOfDelay);
    }

    public void Interact()
    {
        if (_deylayBeforeText)
        {
            StartCoroutine(DelayOpenBook());
        }
        else
        {
            openBook();
        }
    }
}
