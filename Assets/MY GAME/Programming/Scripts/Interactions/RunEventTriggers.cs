using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RunEventTriggers : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private bool _autoClose;
    [SerializeField] private bool _haveAnim = false;
    [SerializeField] private bool _events = false;
    //[SerializeField] private bool _customAnim = false;
    [SerializeField] private GameObject Room_Keep;
    [SerializeField] private GameObject Room_Hide;

    [SerializeField] private UnityEvent _customEvents;

    // Start is called before the first frame update
    void Awake()
    {
        if (_haveAnim)
        {
            _anim = GetComponentInParent<Animator>();
        }
        //if (_customAnim)
        //{
        //    playCustomAnim();
        //}
    }

    public void playCustomAnim()
    {
        _anim.SetBool("open", false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Left the box");
            if (_autoClose)
            {
                if (Room_Keep.activeSelf)
                {
                    Room_Hide.SetActive(false);
                }
                else if (Room_Hide.activeSelf)
                {
                    Room_Keep.SetActive(false);
                }
                if (_anim != null)
                {
                    _anim.SetBool("open", false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!_events)
            {
                if (!_autoClose)
                {
                    if (_anim != null)
                    {
                        _anim.SetBool("play", true);
                    }
                }
            }
            else
            {
                _customEvents.Invoke();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_autoClose)
        {
            if (Room_Keep.activeSelf)
            {
                Room_Hide.SetActive(true);
            }
            else if (Room_Hide.activeSelf)
            {
                Room_Keep.SetActive(true);
            }
        }
        // if (_customPlay)
        // {
        //     _anim.SetBool("play", false);
        // }
    }
}
