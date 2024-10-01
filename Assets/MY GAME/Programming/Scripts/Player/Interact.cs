using System.Collections;
using System.Reflection.Emit;
using UnityEngine;

namespace Player
{
    public class Interact : MonoBehaviour
    {
        //public GUIStyle crossHair, tooltip;
        public LayerMask Layers;
        public string targetLayer;
        [Tooltip("Toggle on to print console messages from this component.")]
        [SerializeField] private bool debug;
        // [Tooltip("The distance that the player can reach interactions.")]
        [Tooltip("The distance that the player can reach interactions."), SerializeField, Range(0,100)] private float distance = 10f;
        public bool showToolTip = false;
        //public string action, button, instruction;
        public bool pickUpObj;
        public GameObject PickUpText; //# Text to pickup things
        private void Update()
        {
            // create a ray (a Ray is ?? a beam, line that comes into contact with colliders)
            Ray interactRay;
            // this ray shoots forward from the center of the camera
            interactRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            Debug.DrawRay(interactRay.origin, transform.forward * distance, Color.green);
            // create hit info (this holds the info for the stuff we interact with) 
            RaycastHit hitInfo;
            // if this physics ray that gets cast in a direction hits a object within our distance and or layer
            if (Physics.Raycast(interactRay, out hitInfo, distance, Layers /*This part here is the layer its optional*/ ))
            {
                if (debug)
                {
                    Debug.DrawRay(transform.position, transform.forward * distance, Color.yellow, 0.5f);
                }
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(targetLayer))
                {
                    Debug.Log("Hit the right thing...");
                    showToolTip = true;
                    OnGUI();
                    if (Input.GetButtonDown("Interaction"))
                    {
                        if (hitInfo.collider.TryGetComponent(out IInteractable interactableObject))
                        {
                            interactableObject.Interact();
                        }
                    }
                }
                else
                {
                    showToolTip = false;
                    PickUpText.SetActive(false); //# Pickup text turns off
                }
                // if our interaction button or key is pressed
            }
            //else
            //{
            //    showToolTip = false;
            //    PickUpText.SetActive(false); //# Pickup text turns off
            //}
        }
        void OnGUI()
        {
            if (showToolTip)
            {
                if (pickUpObj)
                {
                    PickUpText.SetActive(true); //# Pickup text turns on
                }
            }
        }
    }
}