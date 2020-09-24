using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Powerup
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

        // Get the gameManager
        GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // Add 500 points
        gameManager.UpdateScore(500);

        // Destroy the powerup
        Destroy(gameObject);
        
    }
}
