using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractions : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Test Interaction has worked...");
    }
}
