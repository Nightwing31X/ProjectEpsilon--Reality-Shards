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
        textOn = true;
        StartCoroutine(ShowPromptText());
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
    }
}
