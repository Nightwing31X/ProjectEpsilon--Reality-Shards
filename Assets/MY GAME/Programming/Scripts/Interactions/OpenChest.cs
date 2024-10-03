using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour, IInteractable
{
    public Animator chestANIM;
    public GameObject chestOBJ;
    public GameObject keyOBJNeeded;
    public GameObject objInINV;
    public GameObject KeyMissingText;

    public bool isOpen = false;
    public bool isLastChest = false;
    public bool noKeyNeeded = false;
    public bool insideOBJ = false;

    public AudioSource Unlocked;
    public AudioSource Locked;
    // Start is called before the first frame update
    void Start()
    {
        if (KeyMissingText != null)
        {
            KeyMissingText.SetActive(false);
        }
        if (keyOBJNeeded != null)
        {
            keyOBJNeeded.SetActive(false);
        }
        isOpen = false;
    }

    IEnumerator Delay() // Shows the missing key test for 1 second then disappears
    {
        Locked.Play();
        KeyMissingText.SetActive(true);
        yield return new WaitForSeconds(1f); //# Waits 1 seconds (thats how long the audio is)
        KeyMissingText.SetActive(false);
    }

    public void openChest()
    {
        if (!noKeyNeeded)
        {
            if (keyOBJNeeded.activeInHierarchy)
            {
                Unlocked.Play();
                keyOBJNeeded.SetActive(false);
                if (insideOBJ)
                {
                    objInINV.SetActive(true);
                }
                if (isLastChest)
                {
                    chestANIM.SetBool("openLast", true);
                }
                else
                {
                    chestANIM.SetBool("open", true);
                }
                KeyMissingText.SetActive(false);
                isOpen = true;
            }
            else if (!keyOBJNeeded.activeInHierarchy)
            {
                StartCoroutine(Delay()); // To display the missing key text for a few seconds.
            }
        }
        else 
        {
            Unlocked.Play();
            keyOBJNeeded.SetActive(false);
            if (insideOBJ)
            {
                objInINV.SetActive(true);
            }
            if (isLastChest)
            {
                chestANIM.SetBool("openLast", true);
            }
            else
            {
                chestANIM.SetBool("open", true);
            }
            KeyMissingText.SetActive(false);
            isOpen = true;
        }

        if (isOpen)
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Default");
            chestOBJ.layer = LayerIgnoreRaycast;
            //Debug.Log("Current layer: " + chestOBJ.layer);
        }
    }

    public void Interact()
    {
        openChest();
        Debug.Log("Opened Chest...");
    }
}
