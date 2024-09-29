using UnityEngine;
using UnityEngine.UI;


// This is our family name for the scripts
namespace Player
{
    [AddComponentMenu("GameDev/Player/First Person Movement")]
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        #region Variables
        // The direction we are moving 
        [SerializeField] Vector3 _moveDirection;
        // The reference to the CharacterController
        [SerializeField] CharacterController _characterController;
        // Walk, Crouch, Sprint, Jump, Gravity
        [SerializeField] float _movementSpeed, _walk = 5, _run = 10, _crouch = 2.5f, _jump = 8, _gravity = 20;

        public GameObject staminaContainerOBJ;
        public Image staminaBar;
        [SerializeField] public float _stamina, _maxStamina = 100, _currentStaminaCost;
        //private float _stamina, _maxStamina = 100, _currentStaminaCost;
        [SerializeField] private float _staminaCost = 20;
        [SerializeField] private bool _isRunning;
        [SerializeField] private bool _canRegenStamina = true;
        [SerializeField] private bool safeZone = false;
        [SerializeField] public float timerDuration = 3.0f;
        [SerializeField] public float timerValue;
        private Vector3 _lastPos;
        [SerializeField] private bool playerMoving = false;
        //[SerializeField] bool isPlayerDead;



        #endregion
        #region Functions

        void Sprint()
        {
            if (_stamina > 0)
            {
                _movementSpeed = _run;

                _stamina -= _staminaCost * Time.deltaTime;
                if (_stamina < 0)
                {
                    _stamina = 0;
                }
                staminaBar.fillAmount = _stamina / _maxStamina;
            }
        }

        void Timer()
        {
            if (!_isRunning)
            {
                if (!_canRegenStamina)
                { 
                    timerValue += Time.deltaTime;
                    if(timerValue >= timerDuration) 
                    {
                        // Allow Regen
                        _canRegenStamina = true;
                        // Reset timer
                        timerValue = 0;
                        // Reset player's movement speed back to walking speed
                    }
                }
            }
            else if (_isRunning && _movementSpeed == _run && _stamina == 0)
            {
                //Debug.Log("Hey I should be out of running and be walking right now!!!");
                _movementSpeed = _walk;
            }
        }
        void regenStamina()
        {
            if (_canRegenStamina) 
            {
                if (safeZone)
                {
                    if (_currentStaminaCost == _staminaCost)
                    {
                        _currentStaminaCost = _staminaCost*2;
                    }
                    _stamina += _staminaCost * Time.deltaTime;
                    staminaBar.fillAmount = _stamina / _maxStamina;
                }
                else
                {
                    _stamina += _staminaCost * Time.deltaTime;
                    staminaBar.fillAmount = _stamina / _maxStamina;
                }
            }
            if (_stamina > _maxStamina)
            {
                _stamina = _maxStamina;
            }
        }

        private void Awake()
        {
            playerMoving = false;
            _characterController = GetComponent<CharacterController>();

            _stamina = _maxStamina;
            staminaBar.fillAmount = _stamina / _maxStamina;
            _currentStaminaCost = _staminaCost;
            staminaContainerOBJ.SetActive(false);

            //isPlayerDead = GetComponent<Health>().isPlayerDead;
        }
        private void Update()
        {
            Timer();
            //if (true)
            //isPlayerDead = GetComponent<Health>().isPlayerDead;
            //if (!isPlayerDead)
            // Singleton Pattern 
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_characterController.transform.position != _lastPos)
                {
                    //Player has moved
                    playerMoving = true;
                    //Debug.Log("Yep moving...");
                }
                else
                {
                    //Player has not moved
                    playerMoving = false;
                    //Debug.Log("Nope not moving...");
                }
                _lastPos = _characterController.transform.position;
                // Speed change
                // _movementSpeed, _walk, _run, _crouch
                // Left Shift and Left Control
                if (Input.GetKey(KeyCode.LeftShift) && playerMoving)
                {
                    //_movementSpeed = _run;
                    staminaContainerOBJ.SetActive(true);
                    Sprint();
                    _isRunning = true;
                    _canRegenStamina = false;
                    timerValue = 0;
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    _movementSpeed = _crouch;
                    if (!_isRunning)
                    {
                        regenStamina();
                    }
                }
                else
                {
                    _movementSpeed = _walk;
                    _isRunning = false;
                    if (!_isRunning)
                    {
                        regenStamina();
                    }
                    if (_stamina == 100)
                    {
                        staminaContainerOBJ.SetActive(false);
                    }
                }
                // Moving the character 
                // If our reference to the character controll`er has a value aka we connected it yay!!! woop
                if (_characterController != null)
                {
                    // Check if we are on the ground so we can move coz that's how people work
                    if (_characterController.isGrounded)
                    {
                        // What is our direction? Set the move direction based off inputs
                        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                        // Make sure that the direction forward is according to the player's forward and not the world North
                        _moveDirection = transform.TransformDirection(_moveDirection);
                        // Apply speed to the movement direction 
                        _moveDirection *= _movementSpeed;

                        //_moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _movementSpeed);

                        // If we jump
                        if (Input.GetButton("Jump"))
                        {
                            // Move up
                            _moveDirection.y = _jump;
                        }
                    }
                    // Add gravity to direction
                    _moveDirection.y -= _gravity * Time.deltaTime;
                    // Apply movement
                    _characterController.Move(_moveDirection * Time.deltaTime);
                }
                else
                {
                    Debug.LogWarning("!!!MISSING CHARACTER CONTROLLER CONNECTION FOR THE PLAYER!!!");
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!safeZone) 
            {
                if (other.gameObject.tag == "Safezone")
                {
                    safeZone = true;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Safezone")
            {
                safeZone = false;
                _currentStaminaCost = _staminaCost;
            }
        }
        #endregion
    }
}
