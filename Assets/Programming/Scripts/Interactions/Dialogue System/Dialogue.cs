using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour, IInteractable
{
    [SerializeField] string[] _lines;
    public bool _inDialogue = false;
    public void Interact()
    {
        GameManager.instance.inDialogue = _inDialogue;
        Debug.Log(_inDialogue);
        if (_inDialogue)
        {
            DialogueManager.instance.CheckInput();
        }
        else
        {
            DialogueManager.instance.OnActive(_lines);
            _inDialogue = true;
            GameManager.instance.inDialogue = _inDialogue;
        }
    }

    public void talkToSelf()
    {
        GameManager.instance.inDialogue = _inDialogue;
        Debug.Log(_inDialogue);
        if (_inDialogue)
        {
            DialogueManager.instance.CheckInput();
        }
        else
        {
            _inDialogue = true;
            GameManager.instance.inDialogue = _inDialogue;
            DialogueManager.instance.OnActiveAuto(_lines);
        }
    }
}