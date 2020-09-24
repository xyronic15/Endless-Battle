using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatHandler : MonoBehaviour
{
    // references
    public GameObject inPlayUI;
    public Slider healthSlider;
    public PlayerController playerController;
    public GameObject invincibilityIndicator;
    public GameObject speedIndicator;
    public GameObject strengthIndicator;

    // Stat values
    public static int maxHealth = 10;
    public int currentHealth;
    public static int baseSpeed = 10;
    public int speed;
    public static int baseStrength = 1;
    public int strength;
    public bool isInvincible;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Call this method whenever a new level is loaded
    public void OnLevelLoaded()
    {
        Debug.Log("Stats: New Level Loaded");
        inPlayUI = GameObject.Find("In Play UI");
        invincibilityIndicator = inPlayUI.transform.Find("Invincibility Indicator").gameObject;
        invincibilityIndicator.SetActive(false);
        speedIndicator = inPlayUI.transform.Find("Speed Indicator").gameObject;
        speedIndicator.SetActive(false);
        strengthIndicator = inPlayUI.transform.Find("Strength Indicator").gameObject;
        strengthIndicator.SetActive(false);
        healthSlider = inPlayUI.transform.Find("Player Health Slider").GetComponent<Slider>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthSlider.fillRect.gameObject.SetActive(true);
        speed = baseSpeed;
        strength = baseStrength;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Debug.Log(currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log("Dead");
                StartCoroutine(playerController.Death());
                
            }
        }
    }

    private void OnDestroy()
    {
        maxHealth = 10;
    }
}
