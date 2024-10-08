using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class RayDoor : MonoBehaviour, IInteractable
{
    public Animator animator;
    public bool isOpen = false;
    public bool lockedDoor = false;
    public GameObject keyOBJNeeded;
    public GameObject LockedDoorText;


    public AudioSource openDoorAS;
    public AudioSource closeDoorAS;
    public AudioSource lockedDoorAS;

    // Start is called before the first frame update
    void Start()
    {
        if (lockedDoor)
        {
            LockedDoorText.SetActive(false);
            keyOBJNeeded.SetActive(false);
        }
        animator = GetComponentInParent<Animator>();
        isOpen = false;
        animator.SetBool("open", isOpen);
    }

    public void closeDoorAuto()
    {
        if (isOpen)
        {
            closeDoorAS.Play();
            isOpen = false;
            animator.SetBool("open", isOpen);
        }
    }

    IEnumerator Delay() // Shows the missing key test for 1 second then disappears
    {
        lockedDoorAS.Play();
        LockedDoorText.SetActive(true);
        yield return new WaitForSeconds(1f); //# Waits 1 seconds (thats how long the audio is)
        LockedDoorText.SetActive(false);
    }

    public void Interact()
    {
        if (lockedDoor)
        {
            if (keyOBJNeeded.activeInHierarchy)
            {
                isOpen = !isOpen;
                if (isOpen)
                {
                    openDoorAS.Play();
                }
                else
                {
                    closeDoorAS.Play();
                }
                animator.SetBool("open", isOpen);
                LockedDoorText.SetActive(false);
            }
            else
            {
                StartCoroutine(Delay());
            }
        }
        else
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                openDoorAS.Play();
            }
            else
            {
                closeDoorAS.Play();
            }
            animator.SetBool("open", isOpen);
        }
    }
}
