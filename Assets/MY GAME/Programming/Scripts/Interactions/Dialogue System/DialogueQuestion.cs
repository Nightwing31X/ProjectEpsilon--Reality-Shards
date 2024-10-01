using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestion : MonoBehaviour, IInteractable
{
    [SerializeField] string[] _lines;
    //[Tooltip("questionLine, starts from 1 - Plus 1 from the element")]
    [SerializeField] int _QuestionLine;
    //'[Tooltip("leaveLine is what the element is.")]
    public int _leaveLine;
    public bool _inDialogue = false;


    public void Interact()
    {
        GameManager.instance.inDialogue = _inDialogue;
        if (_inDialogue)
        {
            DialogueManager.instance.CheckInput();
        }
        else
        {
            _inDialogue = true;
            GameManager.instance.inDialogue = _inDialogue;
            DialogueManager.instance.OnActive(_lines, _QuestionLine, _leaveLine);
        }
    }
}
