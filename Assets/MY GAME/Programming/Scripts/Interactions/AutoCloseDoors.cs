using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCloseDoors : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject Room_Keep;
    [SerializeField] private GameObject Room_Hide;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Left the box");
            if (Room_Keep.activeSelf)
            {
                Room_Hide.SetActive(false);
            }
            else if (Room_Hide.activeSelf)
            {
                Room_Keep.SetActive(false);
            }
            _anim.SetBool("open", false);
        }
    }
    private void OnTriggerStay(Collider other)
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
}
