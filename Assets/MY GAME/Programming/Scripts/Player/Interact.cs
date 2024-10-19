using System;
using System.Collections;
using System.Reflection.Emit;
using GameDev;
using Menu;
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
        [SerializeField] private bool _debug;
        [SerializeField] private bool _hasRan;
        [SerializeField] private bool _checkControllerInput;
        [SerializeField] private bool _forceControllerInput;
        [Tooltip("The distance that the player can reach interactions."), SerializeField, Range(0, 100)] private float distance = 10f;

        public bool showToolTip = false;
        //public string action, button, instruction;
        public bool pickUpObj;
        public bool attackToolTip;
        public GameObject keyboardPickUpText; //# Text to pickup things
        public GameObject keyboardAttackText;
        public GameObject controllerPickUpText; //# Text to pickup things
        public GameObject controllerAttackText;
        private void Update()
        {
            _forceControllerInput = InputHandler.instance.forceController;
            _checkControllerInput = InputHandler.instance.onController;
            // create a ray (a Ray is ?? a beam, line that comes into contact with colliders)
            Ray interactRay;
            // this ray shoots forward from the center of the camera
            interactRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (_debug)
            {
                Debug.DrawRay(interactRay.origin, transform.forward * distance, Color.green);
            }
            // create hit info (this holds the info for the stuff we interact with) 
            RaycastHit hitInfo;
            // if this physics ray that gets cast in a direction hits a object within our distance and or layer
            if (Physics.Raycast(interactRay, out hitInfo, distance, Layers /*This part here is the layer its optional*/ ))
            {
                if (_debug)
                {
                    Debug.DrawRay(transform.position, transform.forward * distance, Color.yellow, 0.5f);
                }

                # region Detect the interact layer (Interaction Layer)
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(interactionLayer))
                {
                    if (_debug)
                    {
                        if (!_hasRan)
                        {
                            Debug.Log($"Hit Layer = {interactionLayer}");
                            _hasRan = true;
                        }
                    }
                    showToolTip = true;
                    attackToolTip = false;
                    OnGUI(); // Displays out ToolTip
                    //if (KeyBindManager.Keys.Count <= 0)
                    //{
                    if (Input.GetButtonDown("Interaction"))
                    {
                        if (hitInfo.collider.TryGetComponent(out IInteractable interactableObject))
                        {
                            interactableObject.Interact();
                        }
                    }
                    //}
                    //else
                    //{
                    //    if (Input.GetKeyDown(KeyBindManager.Keys["Interact"]))
                    //    {
                    //        if (hitInfo.collider.TryGetComponent(out IInteractable interactableObject))
                    //        {
                    //            interactableObject.Interact();
                    //        }
                    //    }
                    //}
                }
                # endregion
                # region Detect the attack layer (Enemy Layer)
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(attackLayer))
                {
                    if (_debug)
                    {
                        if (!_hasRan)
                        {
                            Debug.Log($"Hit Layer = {attackLayer}");
                            _hasRan = true;
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
                            if (_debug)
                            {
                                _hasRan = false;
                                if (!_hasRan)
                                {
                                    Debug.Log("I have hit the enemy");
                                    _hasRan = true;
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
                    //             if (_debug)
                    //             {
                    //                 _hasRan = false;
                    //                 if (!_hasRan)
                    //                 {
                    //                     Debug.Log("I have hit the enemy");
                    //                     _hasRan = true;
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
                keyboardPickUpText.SetActive(false); //# Pickup text turns off
                keyboardAttackText.SetActive(false);
                controllerPickUpText.SetActive(false);
                controllerAttackText.SetActive(false);
                _hasRan = false;
            }
        }
        void OnGUI()
        {
            if (showToolTip)
            {
                if (pickUpObj)
                {
                    if (_forceControllerInput)
                    {
                        keyboardAttackText.SetActive(false);
                        keyboardPickUpText.SetActive(false);
                        if (!attackToolTip)
                        {
                            controllerAttackText.SetActive(false);
                            controllerPickUpText.SetActive(true);
                        }
                        else
                        {
                            controllerPickUpText.SetActive(false);
                            controllerAttackText.SetActive(true);
                        }
                    }
                    else
                    {
                        if (_checkControllerInput)
                        {
                            if (!attackToolTip)
                            {
                                keyboardAttackText.SetActive(false);
                                keyboardPickUpText.SetActive(false);

                                controllerAttackText.SetActive(false);
                                controllerPickUpText.SetActive(true);
                            }
                            else
                            {
                                keyboardAttackText.SetActive(false);
                                keyboardPickUpText.SetActive(false);

                                controllerPickUpText.SetActive(false);
                                controllerAttackText.SetActive(true);
                            }
                        }
                        else
                        {
                            if (!attackToolTip)
                            {
                                controllerPickUpText.SetActive(false);
                                controllerAttackText.SetActive(false);

                                keyboardAttackText.SetActive(false);
                                keyboardPickUpText.SetActive(true); //# Pickup text turns on
                            }
                            else
                            {
                                controllerPickUpText.SetActive(false);
                                controllerAttackText.SetActive(false);

                                keyboardPickUpText.SetActive(false);
                                keyboardAttackText.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }
}