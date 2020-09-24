using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed : Powerup
{
    public int speedMultiplier;
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

        // Get playerhandler and animator
        PlayerStatHandler stats = player.GetComponent<PlayerStatHandler>();
        Animator anim = player.GetComponent<Animator>();

        // Set speed multiplier and adjust animation speed
        stats.speed = Mathf.Min(speedMultiplier * stats.speed, statCap);
        stats.speedIndicator.SetActive(true);
        anim.speed *= speedMultiplier;

        // Wait for x seconds
        yield return new WaitForSeconds(duration);

        // undo speed multiplier and animation speed
        stats.speed = baseStat;
        stats.speedIndicator.SetActive(false);
        anim.speed /= speedMultiplier;

        // Destroy the powerup
        Destroy(gameObject);
    }
}
