using System.Collections;
using System.Collections.Generic;
using GameDev;
using UnityEngine;
using UnityEngine.UI;


namespace Menu
{
    [AddComponentMenu("GameDev/Menu/Resolution")]
    public class MyResolution : MonoBehaviour
    {
        public Toggle fullscreenToggle;
        public Toggle controllerToggle;
        public GameObject keyboardLayout, controllerLayout;

        public void FullscreenToggle(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            if (GetComponent<saveFullscreen>().refFullscreen != isFullscreen)
            {
                GetComponent<saveFullscreen>().refFullscreen = isFullscreen;
            }
            else
            {
                fullscreenToggle.isOn = isFullscreen;
            }
            //Debug.Log(isFullscreen);
        }

        public void ControllerToggle(bool useController)
        {
            if (useController)
            {
                // Hide Keyboard Layout
                keyboardLayout.SetActive(false);
                // Show the controller Layout
                controllerLayout.SetActive(true);
            } 
            else
            {
                keyboardLayout.SetActive(true);
                controllerLayout.SetActive(false);
            }
            if (GetComponent<saveController>().refUsingController != useController)
            {
                GetComponent<saveController>().refUsingController = useController;
            }
            else
            {
                controllerToggle.isOn = useController;
            }
            //Debug.Log(useController);
        }
        


        //public UnityEngine.Resolution[] resolutions;
        // public Dropdown resolutionDropdown;

        // private void Start()
        // {
        //     if (resolutionDropdown != null)
        //     {
        //         // Getting all resolutions for this computers screen settings
        //         resolutions = Screen.resolutions;
        //         // reset and empty the dropdown
        //         resolutionDropdown.ClearOptions();
        //         // get ready to make new dropdown list
        //         List<string> options = new List<string>();
        //         // Get ready to se the current resolution when found
        //         int currentResolutionIndex = 0;
        //         // loop through all options the computer has
        //         for (int i = 0; i < resolutions.Length; i++)
        //         {
        //             // hold formatted option
        //             string option = $"{resolutions[i].width} x {resolutions[i].height}";
        //             // Add option to list
        //             options.Add(option);
        //             // If option matches resolution then set this as current
        //             if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
        //             {
        //                 currentResolutionIndex = i;
        //             }
        //         }
        //         resolutionDropdown.AddOptions(options);
        //         resolutionDropdown.value = currentResolutionIndex;
        //         resolutionDropdown.RefreshShownValue();
        //     }
        // }
        // public void SetResolution(int resIndex)
        // {
        //     UnityEngine.Resolution res = resolutions[resIndex];
        //     Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        // }
    }
}