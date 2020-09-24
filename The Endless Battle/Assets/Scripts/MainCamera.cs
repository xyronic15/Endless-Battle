using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;
    private int maxHeight;
    public int minHeight;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        maxHeight = (LevelGenerator.levelHeight * LevelGenerator.tileY) + 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y > maxHeight)
        {
            transform.position = new Vector3(player.position.x, maxHeight, -25);
        }
        if (player.position.y < minHeight)
        {
            transform.position = new Vector3(player.position.x, minHeight, -25);
        }
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y + 3, -25);
        }
    }
}
