using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;


namespace GameDev
{
    [AddComponentMenu("GameDev/Portal Fake")]
    public class portalFake : MonoBehaviour
    {
        [SerializeField] private GameObject portal;
        [SerializeField] bool portalActive;

        void Awake()
        {
            portal.SetActive(portalActive);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                portalActive = false;
                portal.SetActive(portalActive);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                portalActive = true;
                portal.SetActive(portalActive);
            }
        }
    }
}