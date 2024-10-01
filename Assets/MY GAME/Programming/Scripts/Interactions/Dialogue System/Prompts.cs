using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Prompts : MonoBehaviour
{

    public GameObject TextOBJ;
    public Text SubTxt;
    [Tooltip("If no number is put in then 6 will be the default value")]
    public float SecondsOfReading;

    [SerializeField][TextArea] private string textContent;
    //[SerializeField] private string[] textContent; //? Could make an array
    bool textOn = false;
    [SerializeField] bool _inDialogue = false;
    [SerializeField] bool _removeTrigger = false;

    [SerializeField] bool stayInPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        TextOBJ.SetActive(textOn);
        if (SecondsOfReading == 0)
        {
            SecondsOfReading = 6f;
        }
    }

    void Update()
    {
        if (textOn)
        {
            TextOBJ.SetActive(textOn);
            //Debug.Log("Prompts...");
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
        textOn = true;
        StartCoroutine(ShowPromptText());
        if (stayInPlace)
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
        SubTxt.text = string.Format(textContent);
        yield return new WaitForSeconds(SecondsOfReading);
        HideTextPrompt();
        if (GameManager.instance.state != GameStates.Play)
        {
            GameManager.instance.OnPlay();
        }
        _inDialogue = false;
        GameManager.instance.inDialogue = _inDialogue;
    }
}
