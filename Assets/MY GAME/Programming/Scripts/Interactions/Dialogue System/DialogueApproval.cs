using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueApproval : MonoBehaviour, IInteractable
{
     [SerializeField] ApprovalDialogueLines _approvalDialogueLines;
    [Tooltip("Question line must be the same on all elements")]
    [SerializeField] int _QuestionLine;
    public int _leaveLine;
    public bool _inDialogue = false;
    public int approvalValue;
    public void Interact()
    {
        DialogueManager.instance.OnActive(_approvalDialogueLines, _QuestionLine, _leaveLine, this);
    }
}
