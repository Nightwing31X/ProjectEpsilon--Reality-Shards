using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteraction : MonoBehaviour, IInteractable
{
    public GameObject keyInINV;


    IEnumerator Delay()
    {
        this.gameObject.transform.position = new Vector3(0.0f, 50.0f, 0.0f); //# Moves the object interacted with upwards (to hide & allow audio to play)
        yield return new WaitForSeconds(5); //# Waits 3 seconds (that is how long the audio is)
        this.gameObject.SetActive(false); //# Then deactivates the object
    }

    public void PickUp_Key()
    {
        Debug.Log("Picked up key...");
        keyInINV.SetActive(true);
        StartCoroutine(Delay());
    }

    public void Interact()
    {
        Debug.Log("Picked Up key...");
    }
}
