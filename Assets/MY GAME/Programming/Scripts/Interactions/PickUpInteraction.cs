using System.Collections;
using UnityEngine;

public class PickUpInteraction : MonoBehaviour, IInteractable
{
    public GameObject objInINV;
    public AudioSource pickUP_AS;
    IEnumerator Delay()
    {
        if (pickUP_AS != null)
        {
            pickUP_AS.Play();
        }
        this.gameObject.transform.position = new Vector3(0.0f, 50.0f, 0.0f); //# Moves the object interacted with upwards (to hide & allow audio to play)
        yield return new WaitForSeconds(5); //# Waits 5 seconds (so the audio can play)
        this.gameObject.SetActive(false); //# Then deactivates the object
    }

    public void PickUp_Obj()
    {
        Debug.Log("Picked up Obj...");
        objInINV.SetActive(true);
        StartCoroutine(Delay());
    }

    public void Interact()
    {
        PickUp_Obj();
    }
}
