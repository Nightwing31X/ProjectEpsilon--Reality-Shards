using System.Collections;
using System.Collections.Generic;
using GameDev;
using UnityEngine;

namespace Player
{
    [AddComponentMenu("GameDev/Player/First Person Footsteps")]
    public class Footsteps : MonoBehaviour
    {
        [SerializeField] CharacterController _characterController;
        [SerializeField] private bool _playerMoving = false;
        private Vector3 _lastPos;
        [SerializeField] private float timer = 0f;
        [SerializeField] private AudioSource _AudioSourceREF;
        [SerializeField] private float regular_timeBetweenSteps = 0.5f;
        [SerializeField] private float timeBetweenSteps = 0.5f;
        [SerializeField] private float crouchTimeBetweenSteps = 0.7f;
        [SerializeField] private float runTimeBetweenSteps = 0.3f;
        public float TimeBetweenStepsMultiplier = 1f;
        // [Tooltip("A list to the footstep audio clips. (The list will be randomly selected from each time a footstep is triggered.)")]
        [SerializeField] private AudioClip[] _footstepClips;
        [SerializeField] private bool isRun;
        [SerializeField] private bool isCrouch;


        void Awake()
        {
            _playerMoving = false;
            _characterController = GetComponent<CharacterController>();
            _AudioSourceREF = GetComponent<AudioSource>();
            regular_timeBetweenSteps = timeBetweenSteps;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_characterController.transform.position != _lastPos)
                {
                    //Player has moved
                    _playerMoving = true;
                    //Debug.Log("Yep moving...");
                }
                else
                {
                    //Player has not moved
                    _playerMoving = false;
                    //Debug.Log("Nope not moving...");
                }
                _lastPos = _characterController.transform.position;

                isCrouch = _characterController.GetComponent<Movement>().isCrouch;
                if (!isRun)
                {
                    if (isCrouch)
                    {
                        timeBetweenSteps = crouchTimeBetweenSteps;
                    }
                    else
                    {
                        timeBetweenSteps = regular_timeBetweenSteps;
                    }
                }
                isRun = _characterController.GetComponent<Movement>().isRun;
                if (!isCrouch)
                {
                    if (isRun)
                    {
                        timeBetweenSteps = runTimeBetweenSteps;
                    }
                    else
                    {
                        timeBetweenSteps = regular_timeBetweenSteps;
                    }
                }
                //? To check if the player is moving in any direction and if so then player a sound effect.
                if (_characterController != null && _characterController.isGrounded == true && _playerMoving == true)
                {
                    if (timer >= 0f)
                    {
                        timer += Time.deltaTime;
                        if (timer > timeBetweenSteps * TimeBetweenStepsMultiplier)
                        {
                            if (_AudioSourceREF != null && _footstepClips.Length > 0)
                            {
                                _AudioSourceREF.PlayOneShot(_footstepClips[Random.Range(0, _footstepClips.Length)]);
                            }
                            timer = 0f;
                        }
                    }
                }
            }
        }
    }
}
