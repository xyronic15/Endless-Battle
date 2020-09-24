using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInvincible : Powerup
{
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
        base.OnTriggerEnter(other);
    }

    public override IEnumerator Pickup(PlayerController player, float duration)
    {
        // Apply the cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        // Disable the mesh renderer ad collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        
        // Play the pickup sound
        powerupAudio.PlayOneShot(pickupSound);

        // Get playerhandler
        PlayerStatHandler stats = player.GetComponent<PlayerStatHandler>();

        // Set invincible
        stats.isInvincible = true;
        stats.invincibilityIndicator.SetActive(true);

        // Wait for x seconds
        yield return new WaitForSeconds(duration);

        // Set invincible to false
        stats.isInvincible = false;
        stats.invincibilityIndicator.SetActive(false);

        // Destroy the powerup
        Destroy(gameObject);
    }
}
