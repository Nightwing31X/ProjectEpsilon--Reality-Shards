using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;


namespace GameDev
{
    [AddComponentMenu("GameDev/FPS Updater")]
    public class FPSUpdater : MonoBehaviour
    {
        float fps;
        [SerializeField] float updateFrequency = 0.2f;
        float updateTimer;

        [SerializeField] bool isDisplayed = false;


        [SerializeField] Text fpsText;

        private void UpdateFPSDisplay()
        {
            updateTimer -= Time.deltaTime;
            if (updateTimer <= 0f)
            {
                fps = 1f / Time.unscaledDeltaTime;
                fpsText.text = "FPS: " + Mathf.RoundToInt(fps);
                updateTimer = updateFrequency;
            }
        }

        private void ToggleFPSDisplay()
        {
            if (Input.GetKeyDown(KeyCode.Period) && Input.GetKeyDown(KeyCode.Period))
            {
                if (!isDisplayed)
                {
                    fpsText.enabled = true;
                    isDisplayed = true;
                }
                else
                {
                    fpsText.enabled = false;
                    isDisplayed = false;
                }

            }
        }

        void Update()
        {
            ToggleFPSDisplay();
            if (isDisplayed)
            {
                UpdateFPSDisplay();
            }
        }

        void Start()
        {
            updateTimer = updateFrequency;
            fpsText.enabled = isDisplayed;
        }
    }
}