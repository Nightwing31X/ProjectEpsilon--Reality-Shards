using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDoor : MonoBehaviour, IInteractable
{
    public Animator animator;
    public bool isOpen = false;
    public bool lockedDoor = false;
    public bool autoCloseDoor = false;
    public GameObject keyOBJNeeded;
    public GameObject LockedDoorText;


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

    IEnumerator Delay() // Shows the missing key test for 1 second then disappears
    {
        //Locked.Play();
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
            animator.SetBool("open", isOpen);
        }
    }
}
