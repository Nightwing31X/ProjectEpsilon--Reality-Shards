using GameDev;
using UnityEngine;

namespace Player
{
    [AddComponentMenu("GameDev/Player/First Person Mouse Look")]
    public class MouseLook : MonoBehaviour
    {
        #region Rotational axis
        /*
            We are going to use something know as a state value
            variable, also know as a comma seperated list of identifiers
            also known as the Enueration type enum.
         */
        public enum RotationalAxis
        {
            MouseMovementX, // This contols the players axis (Moves left and right)
            MouseMovementY // This controls the camera axis (Moves up and down)
        }
        #endregion
        #region Variables
        // Reference to the enum RotationalAxis so we can use it
        [SerializeField] RotationalAxis _axis = RotationalAxis.MouseMovementX;
        // A way to control the mouse speed/sensitivity
        [SerializeField] float _sensitivity = 15;
        public float Sensitivity
        {
            get { return _sensitivity; }
            set { _sensitivity = value; }
        }
        // Max/Min Rotation range so we don't have a head that looks up and ends up doing 360s
        [SerializeField] Vector2 _rotationRangeY = new Vector2(-60.0f, 60.0f);
        // Placeholder Temp value for the yrotation so we can invert it if needed
        float _rotationY;
        bool _invert = false;
        public bool Invert
        {
            get { return _invert; }
            set { _invert = value; }
        }
        #endregion
        #region Functions
        private void Awake()
        {
            if (GetComponent<Rigidbody>())
            {
                //GetComponent<Rigidbody>().freezeRotation = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            if (GetComponent<Camera>())
            {
                _axis = RotationalAxis.MouseMovementY;
            }
        }
        private void Update()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                #region Horizontal Mouse Movement
                if (_axis == RotationalAxis.MouseMovementX)
                {
                    transform.Rotate(0, Input.GetAxis("Mouse X") * _sensitivity, 0);
                    transform.Rotate(0, Input.GetAxis("Mouse X - RightStick") * _sensitivity, 0);
                }
                #endregion
                #region Vertical Mouse Movement
                else
                {
                    // Get the rotation direction and speed
                    _rotationY += Input.GetAxis("Mouse Y") * _sensitivity;
                    _rotationY += Input.GetAxis("Mouse Y - RightStick") * _sensitivity;
                    // Clamp
                    _rotationY = Mathf.Clamp(_rotationY, _rotationRangeY.x, _rotationRangeY.y);
                    // Apply
                    if (!_invert) // Normal
                    {
                        transform.localEulerAngles = new Vector3(-_rotationY, 0, 0);
                    }
                    else // Plane
                    {
                        transform.localEulerAngles = new Vector3(_rotationY, 0, 0);
                    }
                }
                #endregion
            }
        }
        #endregion
    }
}

