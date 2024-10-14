using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Interactions
{
    [AddComponentMenu("GameDev/Interactions/Raycast Door")]
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

        public AIController enemyRef;
        public bool enemyAtDoor;

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

            enemyRef = GetComponent<AIController>();
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


        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Knight")
            {
                Debug.Log("In front door");
                enemyAtDoor = true;
                enemyRef.nearDoor = enemyAtDoor;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Knight")
            {
                Debug.Log("Left the door");
                enemyAtDoor = false;
                enemyRef.nearDoor = enemyAtDoor;
            }
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
}