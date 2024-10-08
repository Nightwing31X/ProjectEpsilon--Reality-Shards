using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Prompts : MonoBehaviour
{

    public GameObject TextOBJ;
    public Text SubTxt;
    [Tooltip("If no number is put in, then 6 will be the default value")]
    public float SecondsOfReading;
    [Tooltip("If delay is true and no number is put in, then 2 will be the default value")]
    public float SecondsOfDelay;

    [SerializeField][TextArea] private string _textContent;
    //[SerializeField] private string[] _textContent; //? Could make an array
    bool textOn = false;
    [SerializeField] bool _inDialogue = false;
    [SerializeField] bool _removeTrigger = false;
    [SerializeField] bool _delayBeforeText = false;
    [SerializeField] bool _stayInPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        TextOBJ.SetActive(textOn);
        if (SecondsOfReading == 0)
        {
            SecondsOfReading = 6f;
        }
        if (_delayBeforeText)
        {
            if (SecondsOfDelay == 0)
            {
                SecondsOfDelay = 2f;
            }
        }
    }

    void Update()
    {
        if (textOn)
        {
            TextOBJ.SetActive(textOn);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _inDialogue = true;
            GameManager.instance.inDialogue = _inDialogue;
            TextPrompt();
        }
    }

    public void TextPrompt()
    {
        if (_delayBeforeText)
        {
            StartCoroutine(DelayShowPromptText());
        }
        else
        {
            textOn = true;
            StartCoroutine(ShowPromptText());
        }
        if (_stayInPlace)
        {
            if (GameManager.instance.state != GameStates.Menu)
            {
                GameManager.instance.OnMenu();
            }
        }
        if (_removeTrigger)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void HideTextPrompt()
    {
        textOn = false;
        TextOBJ.SetActive(textOn);
    }
    private IEnumerator ShowPromptText()
    {
        SubTxt.text = string.Format(_textContent);
        yield return new WaitForSeconds(SecondsOfReading);
        HideTextPrompt();
        _inDialogue = false;
        GameManager.instance.inDialogue = _inDialogue;
    }
    private IEnumerator DelayShowPromptText()
    {
        yield return new WaitForSeconds(SecondsOfDelay);
        textOn = true;
        StartCoroutine(ShowPromptText());
    }
}
