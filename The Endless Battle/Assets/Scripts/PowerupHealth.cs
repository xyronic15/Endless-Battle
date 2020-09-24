using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHealth : Powerup
{
    public int healthAdd;
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

        // Add health but make sure it doesn't exceed max health
        playerHandler.currentHealth = Mathf.Min(PlayerStatHandler.maxHealth, playerHandler.currentHealth + healthAdd);

        // Destroy the powerup
        Destroy(gameObject);
        
    }
}
