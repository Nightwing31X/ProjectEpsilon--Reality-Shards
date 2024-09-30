using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    //there can be only one!!!
    public static DialogueManager instance;
    private void Awake()
    {
        //if its empty
        if (instance == null)
        {
            //fill the empty spot
            instance = this;
        }
        //if the instance reference has something there and its not the reference 
        else if (instance != null && instance != this)
        {
            //Destroy the imposter
            Destroy(this);
        }
    }
    #endregion
    #region Dialogue UI Variables
    [SerializeField] GameObject _dialogueBox;//what we turn on and off
    [SerializeField] Text _dialogueText;//the text box
    [SerializeField] GameObject _buttonsObject;//what we turn on and off
    #endregion
    #region Dialogue Variables
    //lines to read
    [SerializeField] string[] dialogueLines;
    //current line
    [SerializeField] int currentIndex = 0;
    [SerializeField] int questionIndex = -1;
    [SerializeField] int leaveIndex = -1;
    [SerializeField] bool _inDialogue = false;
    [SerializeField] bool _inCutscene = false;
    [SerializeField] bool _inChoice = false;
    #region Approval
    [SerializeField] ApprovalDialogueLines _approvalDialogueLines;
    [SerializeField] DialogueApproval _currentApproval;
    #endregion
    #endregion
    void Start()
    {
        _dialogueBox.SetActive(false);
        _buttonsObject.SetActive(false);
    }
    void Update()
    {
        if (_inDialogue && _inCutscene)
        {
            if (Input.GetButtonDown("Interaction"))
            {
                CheckInput();
            }
        }

    }
    public void OnActiveAuto(string[] lines)
    {
        _inCutscene = true;
        _dialogueBox.SetActive(true);
        _buttonsObject.SetActive(false);

        dialogueLines = lines;
        currentIndex = 0;

        GameManager.instance.OnMenu();
        _dialogueText.text = dialogueLines[currentIndex];
        questionIndex = -1;
        _inDialogue = GameManager.instance.inDialogue;
    }
    public void OnActive(string[] lines)
    {
        _dialogueBox.SetActive(true);
        _buttonsObject.SetActive(false);

        dialogueLines = lines;
        currentIndex = 0;

        GameManager.instance.OnMenu();
        _dialogueText.text = dialogueLines[currentIndex];
        questionIndex = -1;
        _inDialogue = GameManager.instance.inDialogue;
    }
    public void OnActive(string[] lines, int questionLine, int leaveLine)
    {
        _dialogueBox.SetActive(true);
        _buttonsObject.SetActive(false);

        dialogueLines = lines;
        currentIndex = 0;
        questionIndex = questionLine;
        leaveIndex = leaveLine;

        GameManager.instance.OnMenu();
        _dialogueText.text = dialogueLines[currentIndex];
        _inDialogue = GameManager.instance.inDialogue;
    }
    public void OnActive(ApprovalDialogueLines lines, int questionLine, int leaveLine, DialogueApproval approval)
    {
        _dialogueBox.SetActive(true);
        _buttonsObject.SetActive(false);

        _approvalDialogueLines = lines;
        currentIndex = 0;
        questionIndex = questionLine;
        leaveIndex = leaveLine;

        _currentApproval = approval;
        GameManager.instance.OnMenu();

        //We need a Switch Statement Function to select the correct shiz
        //that will go here
        ChangeApproval();
        _dialogueText.text = dialogueLines[currentIndex];
    }
    void ChangeApproval()
    {
        if (_currentApproval == null)
        {
            return;
        }
        switch (_currentApproval.approvalValue)
        {
            case -1:
                dialogueLines = _approvalDialogueLines.dislikeLines;
                break;
            case 0:
                dialogueLines = _approvalDialogueLines.neutralLines;
                break;
            case 1:
                dialogueLines = _approvalDialogueLines.likedLines;
                break;
            default:
                Debug.Log("APPROVAL IS BROKEN...I REPEAT APPROVAL IS BROKEN!!");
                break;
        }
    }
    void OnDeActive()
    {
        _dialogueBox.SetActive(false);
        _buttonsObject.SetActive(false);
        GameManager.instance.OnPlay();
        _inDialogue = false;
        GameManager.instance.inDialogue = _inDialogue;
    }
    void btnChoice()
    {
        _buttonsObject.SetActive(true);
    }
    public void YesBTN()
    {
        _inChoice = false;
        Debug.Log("Player said yes...");
        GameManager.instance.inQuest = true;
        _buttonsObject.SetActive(false);
        CheckInput();
    }
    public void NoBTN()
    {
        _inChoice = false;
        Debug.Log("Player said no...");
        Leave();
    }
    public void Leave()
    {
        currentIndex = leaveIndex;
        _buttonsObject.SetActive(false);
        _dialogueText.text = dialogueLines[currentIndex];
    }
    public void CheckInput()
    {
        if (!_inChoice)
        {
            if (currentIndex < dialogueLines.Length - 1)
            {
                currentIndex++;
                Debug.Log(currentIndex);
                if (currentIndex == questionIndex)
                {
                    _inChoice = true;
                    btnChoice();
                }
            }
            else
            {
                currentIndex = 0;
                OnDeActive();
            }
            _dialogueText.text = dialogueLines[currentIndex];
        }
    }
}
[System.Serializable]
public struct ApprovalDialogueLines
{
    public string[] dislikeLines;
    public string[] neutralLines;
    public string[] likedLines;
}