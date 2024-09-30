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
    public float SecondsOfReading;
    public bool showText;

    [SerializeField][TextArea] private string textContent;
    // [SerializeField][TextArea] private string textContent; //? Could make an array
    bool textOn;

    // Start is called before the first frame update
    void Start()
    {
        TextOBJ.SetActive(false);
    }

    void Update()
    {
        if (textOn)
        {
            TextOBJ.SetActive(true);
            Debug.Log("Prompts...");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TextPrompt();
        }
    }
    
    public void TextPrompt()
    {
        if (showText)
        {
            textOn = true;
        }
        StartCoroutine(ShowPromptText());
    }
    public void HideTextPrompt()
    {
        textOn = false;
        if (textOn==false)
        {
            TextOBJ.SetActive(false);
            Debug.Log("now...");
        }
        Debug.Log("this...");
    }
    private IEnumerator ShowPromptText()
    {
        if (showText)
        {
            SubTxt.text = string.Format(textContent);
            //TextOBJ.SetActive(false);
            Debug.Log("Showing...");
            if (SecondsOfReading == 0)
            {
                SecondsOfReading = 6f;
            }
            yield return new WaitForSeconds(SecondsOfReading);
            textOn = false;
            TextOBJ.SetActive(false);
            Debug.Log("Hiding...");
        }
    }
}
