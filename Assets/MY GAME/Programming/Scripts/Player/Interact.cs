using System.Collections;
using System.Reflection.Emit;
using Menu;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Player
{
    [AddComponentMenu("GameDev/Player/First Person Interact")]
    public class Interact : MonoBehaviour
    {
        //public GUIStyle crossHair, tooltip;
        public LayerMask Layers;
        public string interactionLayer;
        public string attackLayer;
        [Tooltip("Toggle on to print console messages from this component.")]
        [SerializeField] private bool debug;
        [SerializeField] private bool hasRan;
        // [Tooltip("The distance that the player can reach interactions.")]
        [Tooltip("The distance that the player can reach interactions."), SerializeField, Range(0, 100)] private float distance = 10f;

        public bool showToolTip = false;
        //public string action, button, instruction;
        public bool pickUpObj;
        public bool attackToolTip;
        public GameObject PickUpText; //# Text to pickup things
        public GameObject AttackText;
        private void Update()
        {
            // create a ray (a Ray is ?? a beam, line that comes into contact with colliders)
            Ray interactRay;
            // this ray shoots forward from the center of the camera
            interactRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (debug)
            {
                Debug.DrawRay(interactRay.origin, transform.forward * distance, Color.green);
            }
            // create hit info (this holds the info for the stuff we interact with) 
            RaycastHit hitInfo;
            // if this physics ray that gets cast in a direction hits a object within our distance and or layer
            if (Physics.Raycast(interactRay, out hitInfo, distance, Layers /*This part here is the layer its optional*/ ))
            {
                if (debug)
                {
                    Debug.DrawRay(transform.position, transform.forward * distance, Color.yellow, 0.5f);
                }

                # region Detect the interact layer (Interaction Layer)
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(interactionLayer))
                {
                    if (debug)
                    {
                        if (!hasRan)
                        {
                            Debug.Log($"Hit Layer = {interactionLayer}");
                            hasRan = true;
                        }
                    }
                    showToolTip = true;
                    attackToolTip = false;
                    OnGUI(); // Displays out ToolTip
                    if (KeyBindManager.Keys.Count <= 0)
                    {
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
                        if (Input.GetKeyDown(KeyBindManager.Keys["Interact"]))
                        {
                            if (hitInfo.collider.TryGetComponent(out IInteractable interactableObject))
                            {
                                interactableObject.Interact();
                            }
                        }
                    }
                }
                # endregion
                # region Detect the attack layer (Enemy Layer)
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                {
                    if (debug)
                    {
                        if (!hasRan)
                        {
                            Debug.Log($"Hit Layer = {attackLayer}");
                            hasRan = true;
                        }
                    }
                    showToolTip = true;
                    attackToolTip = true;
                    OnGUI(); // Displays out ToolTip
                    // if (KeyBindManager.Keys.Count <= 0)
                    // {
                    if (Input.GetButtonDown("Attack"))
                    {
                        if (hitInfo.collider.TryGetComponent(out IInteractable interactableObject))
                        {
                            interactableObject.Interact();
                            if (debug)
                            {
                                hasRan = false;
                                if (!hasRan)
                                {
                                    Debug.Log("I have hit the enemy");
                                    hasRan = true;
                                }
                            }
                        }
                    }
                    // }
                    // else
                    // {
                    //     if (Input.GetKey(KeyBindManager.Keys["Attack"]))
                    //     {
                    //         if (hitInfo.collider.TryGetComponent(out IInteractable interactableObject))
                    //         {
                    //             interactableObject.Interact();
                    //             if (debug)
                    //             {
                    //                 hasRan = false;
                    //                 if (!hasRan)
                    //                 {
                    //                     Debug.Log("I have hit the enemy");
                    //                     hasRan = true;
                    //                 }
                    //             }
                    //         }
                    //     }
                    // }
                }
                # endregion
            }
            else
            {
                showToolTip = false;
                attackToolTip = false;
                PickUpText.SetActive(false); //# Pickup text turns off
                AttackText.SetActive(false);
                hasRan = false;
            }
        }
        void OnGUI()
        {
            if (showToolTip)
            {
                if (pickUpObj)
                {
                    if (!attackToolTip)
                    {
                        AttackText.SetActive(false);
                        PickUpText.SetActive(true); //# Pickup text turns on
                    }
                    else
                    {
                        PickUpText.SetActive(false);
                        AttackText.SetActive(true);
                    }
                }
            }
        }
    }
}