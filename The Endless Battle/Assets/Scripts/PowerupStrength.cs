using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupStrength : Powerup
{
    public int strengthMultiplier;
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

        // Set strength multiplier
        stats.strength = Mathf.Min(strengthMultiplier * stats.strength, statCap);
        stats.strengthIndicator.SetActive(true);

        // Wait for x seconds
        yield return new WaitForSeconds(duration);

        // undo strength multiplier
        stats.strength = baseStat;
        stats.strengthIndicator.SetActive(false);

        // Destroy the powerup
        Destroy(gameObject);
    }
}
