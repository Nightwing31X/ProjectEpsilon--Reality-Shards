using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using GameDev;
using UnityEngine.AI;
using UnityEngine.EventSystems;


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
        [SerializeField] private float damageTimer;
        [SerializeField] private float FireDamage = 15f;
        [SerializeField] private float environmentalDamagePerSeconds = 1.5f;
        [SerializeField] private bool canHeal = true;
        [SerializeField] public bool isHurt = false;
        [SerializeField] private bool safeZone = false;
        private GameObject HUD;
        [SerializeField] private Image healthImpact;
        private GameObject gameoverContainer;
        public GameObject gameoverSelect;
        private GameObject mainCamera;
        [SerializeField] public bool isPlayerDead = false;
        public GameObject particalAura;

        [Tooltip("The audio clips for talking damage. (Will be randomly selected from.)")]
        [SerializeField] private AudioClip[] _damageClips;
        [SerializeField] private AudioSource _AudioSourceREF;

        GameObject[] enemies;
        GameObject[] enemy;

        public void DamagePlayer(float damageValue)
        {
            if (_AudioSourceREF != null && _damageClips.Length > 0)
            {
                Debug.Log("Play hit sound...");
                _AudioSourceREF.PlayOneShot(_damageClips[Random.Range(0, _damageClips.Length)]);
            }
            timerValue = 0;
            damageTimer = 0;
            canHeal = false;
            isHurt = true;
            currentHealth -= damageValue;
            //UpdateUI();
        }
        //void UpdateUI()
        //{
        //    displayImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        //    displayImage.color = gradientHealth.Evaluate(displayImage.fillAmount);
        //}

        void HealthDamageImpact()
        {
            float transparency = 1f - (currentHealth / 100f);
            Color imageColor = Color.white;
            imageColor.a = transparency;
            healthImpact.color = imageColor;
        }
        void Death()
        {
            if (currentHealth <= 0)
            {
                GameManager.instance.OnDeath();
                isPlayerDead = true;
                // Turn on GameOver Menu
                gameoverContainer.SetActive(true);
                // Clear selected object
                EventSystem.current.SetSelectedGameObject(null);
                // Set a new selected object
                EventSystem.current.SetSelectedGameObject(gameoverSelect);
                // Turn off Health Menu
                HUD.SetActive(false);
            }
        }
        //public IEnumerator WaitForPlay(float waitTime)
        //{
        //    yield return new WaitForSeconds(waitTime);
        //    //Debug.Log("Waited...");
        //    GameManager.instance.OnPlay();
        //}


        //public void Respawn()
        //{
        //    // Player is back to life
        //    isPlayerDead = false;
        //    // Fix health
        //    currentHealth = maxHealth;
        //    // Fix the Stamina
        //    GetComponent<Movement>()._stamina = GetComponent<Movement>()._maxStamina;
        //    // Turn on Health Menu
        //    HUD.SetActive(true);
        //    // Turn on GameOver Menu
        //    gameoverContainer.SetActive(false);
        //    // Move the player
        //    transform.position = spawnPoint.position;
        //    transform.rotation = spawnPoint.rotation;
        //    // Fix display of health
        //    //UpdateUI();
        //    StartCoroutine(WaitForPlay(0.2f));
        //}

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
                        //Debug.Log(currentRegenValue);
                        currentHealth += currentRegenValue * Time.deltaTime;
                        //UpdateUI();
                        particalAura.SetActive(true);
                    }
                    else
                    {
                        particalAura.SetActive(false);
                    }
                }
            }
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                particalAura.SetActive(false);
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
            if (isHurt)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= environmentalDamagePerSeconds)
                {
                    isHurt = false;
                    // Reset Timer - so player can be hurt again
                    damageTimer = 0;
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
        }
        private void Start()
        {
            _AudioSourceREF = GetComponent<AudioSource>();
            currentHealth = maxHealth;
            currentRegenValue = regenValue;
            displayImage.fillAmount = 1;
        }
        private void Update()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                HealthDamageImpact();
                HealthOverTime();
                Death();
                Timer();
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Damage")
            {
                DamagePlayer(10);
            }
            else if (collision.gameObject.tag == "FireDamage")
            {
                DamagePlayer(FireDamage);
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Damage")
            {
                if (!isHurt)
                {
                    DamagePlayer(10);
                }
            }
            else if (collision.gameObject.tag == "FireDamage")
            {
                if (!isHurt)
                {
                    DamagePlayer(15);
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


