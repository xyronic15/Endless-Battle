using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatHandler : MonoBehaviour
{
    // reference
    public Slider healthSlider;
    public EnemyController enemyController;

    // Stat values for the enemy
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public int strength;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthSlider.fillRect.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            enemyController.Death();
        }

    }
}
