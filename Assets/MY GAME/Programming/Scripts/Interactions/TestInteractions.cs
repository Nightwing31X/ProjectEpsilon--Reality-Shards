using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    [AddComponentMenu("GameDev/Interactions/Test Interaction Function")]
    public class TestInteractions : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Test Interaction has worked...");
        }
    }
}