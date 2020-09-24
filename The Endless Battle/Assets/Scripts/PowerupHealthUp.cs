using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerupHealthUp : Powerup
{
    public int healthIncrease;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.transform.root.GetComponent<PlayerController>();
            // StartCoroutine(Pickup(player));
            Pickup(player);
        }
    }

    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);

        // Play the pickup sound
        powerupAudio.PlayOneShot(pickupSound);

        // Get the stathandler
        PlayerStatHandler playerHandler = player.GetComponent<PlayerStatHandler>();

        // increase amx health and refill health
        PlayerStatHandler.maxHealth += healthIncrease;
        playerHandler.healthSlider.maxValue = PlayerStatHandler.maxHealth;
        playerHandler.currentHealth = PlayerStatHandler.maxHealth;

        // Destroy the powerup
        Destroy(gameObject);
        
    }
}
