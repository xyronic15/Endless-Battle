using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameObject pickupEffect;
    public float duration;
    public int statCap;
    public int baseStat;
    public AudioSource powerupAudio;
    public AudioClip pickupSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.transform.root.GetComponent<PlayerController>();
            StartCoroutine(Pickup(player, duration));
            // Pickup(player);
        }
    }

    public virtual IEnumerator Pickup(PlayerController player, float duration)
    {
        Debug.Log("working 2");
        // Apply the cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        // Disable the mesh renderer ad collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        // Play the pickup sound
        powerupAudio.PlayOneShot(pickupSound);

        yield return new WaitForSeconds(0.1f);
    }

    public virtual void Pickup(PlayerController player)
    {
        // Apply the cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        // Disable the mesh renderer ad collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

    }
}
