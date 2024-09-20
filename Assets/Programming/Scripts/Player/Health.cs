using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


namespace Player
{
    [AddComponentMenu("GameDev/Player/Health")]
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100, currentHealth, regenValue, currentRegenValue;
        [SerializeField] Image displayImage;
        [SerializeField] Gradient gradientHealth;
        [SerializeField] public Transform spawnPoint;
        [SerializeField] private float timerValue;
        [SerializeField] private bool canHeal = true;
        [SerializeField] private bool safeZone = false;
        private GameObject HUD;
        [SerializeField] private Image healthIMG_One;
        [SerializeField] private Image healthIMG_Two;
        [SerializeField] private Image healthIMG_Three;
        private GameObject gameoverContainer;
        private GameObject mainCamera;
        [SerializeField] public bool isPlayerDead = false;


        public void DamagePlayer(float damageValue)
        {
            timerValue = 0;
            canHeal = false;
            currentHealth -= damageValue;
            UpdateUI();
        }
        void UpdateUI()
        {
            displayImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            displayImage.color = gradientHealth.Evaluate(displayImage.fillAmount);
        }
        void Death()
        {
            if (currentHealth <= 0)
            {
                GameManager.instance.OnDeath();
                isPlayerDead = true;
                // Turn on GameOver Menu
                gameoverContainer.SetActive(true);
                // Turn off Health Menu
                HUD.SetActive(false);
            }
        }

        public IEnumerator WaitForPlay(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("Waited...");
            GameManager.instance.OnPlay();
        }


        public void Respawn()
        {
            // Player is back to life
            isPlayerDead = false;
            // Fix health
            currentHealth = maxHealth;
            // Fix the Stamina
            GetComponent<Movement>()._stamina = GetComponent<Movement>()._maxStamina;
            // Turn on Health Menu
            HUD.SetActive(true);
            // Turn on GameOver Menu
            gameoverContainer.SetActive(false);
            // Move the player
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            // Fix display of health
            UpdateUI();
            StartCoroutine(WaitForPlay(0.2f));
        }



        void HealthOverTime()
        {
            if (canHeal)
            {
                if (currentHealth < maxHealth && currentHealth > 0)
                {
                    if (safeZone)
                    {
                        if (currentRegenValue == regenValue)
                        {
                            currentRegenValue = regenValue * 2;
                        }
                        Debug.Log(currentRegenValue);
                        currentHealth += currentRegenValue * Time.deltaTime;
                        UpdateUI();
                        //Debug.Log("In safeZone...");
                    }
                    else
                    {
                        // Current health to increase by a value over time
                        currentHealth += currentRegenValue * Time.deltaTime;
                        UpdateUI();
                        //Debug.Log("Out of safeZone...");
                    }
                }
            }
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        public void Timer()
        {
            if (!canHeal)
            {
                timerValue += Time.deltaTime;
                if (timerValue >= 1.5f)
                {
                    // Allow healing
                    canHeal = true;
                    // Reset timer
                    timerValue = 0;
                }
            }
        }

        #region Unity Event Functions
        void Awake()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            HUD = GameObject.Find("HUD");
            // Turn on HUD Menu
            HUD.SetActive(true);
            gameoverContainer = GameObject.Find("GameoverContainer");
            // Turn off GameOver Menu
            gameoverContainer.SetActive(false);

            healthIMG_One.enabled = false;
            healthIMG_Two.enabled = false;
            healthIMG_Three.enabled = false;
        }
        private void Start()
        {
            currentHealth = maxHealth;
            currentRegenValue = regenValue;
            displayImage.fillAmount = 1;
        }
        private void Update()
        {
            HealthOverTime();
            Death();
            Timer();
        }
        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.tag == "Damage")
            {
                // Do a Thing!!
                Debug.Log("Hit!");
                DamagePlayer(10);
                if (currentHealth <= 99)
                {
                    healthIMG_One.enabled = true;
                    if (currentHealth <= 50)
                    {
                        healthIMG_Two.enabled = true;
                        if (currentHealth <= 10)
                        {
                            healthIMG_Three.enabled = true;
                        }
                    }
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!safeZone) // Did this so it only ran once...
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
                currentRegenValue = regenValue;
            }
        }
        #endregion
    }
}


