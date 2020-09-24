using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // references
    public GameObject player;
    public GameObject platform;
    public GameObject plaformParent;
    public GameObject[] enemies;
    public GameObject[] powerups;
    
    // constants
    public static int tileX = 6;
    public static int tileY = 10;
    public static int levelWidth = 10;
    public static int levelHeight = 5;

    // level data
    public static bool[,] levelData;
    // Start is called before the first frame update
    void Start()
    {
        // Get the character controller
        player = GameObject.Find("Player");

        // initialize leveldatae with the level size
        levelData = new bool[levelWidth,levelHeight];

        // Make local int variable to count the stretch of a platform
        int stretchCount = 0;

        // Create maze
        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                // Create the first platform
                if (x == 0 && y == 0)
                {
                    stretchCount = Random.Range(2, 7);
                    CreateChildPrefab(platform, plaformParent, x, y, 0, stretchCount);

                    // Update the level data
                    levelData[x, y] = true;

                    // decrement strechCount
                    stretchCount--;

                    // Instantiate the player
                    player.transform.SetPositionAndRotation(
                        new Vector3(x + 3, y, 0), player.transform.rotation
                    );
                }
                // Create the rest of the bottom floor
                else
                {
                    // Check the stretchCount
                    if (stretchCount > 0)
                    {
                        // Update level data for this point
                        // decrement strecthCount
                        levelData[x, y] = true;
                        stretchCount--;
                    }
                    else if (stretchCount == 0)
                    {
                        // Set level data at that spot to false if stretch count is 0
                        levelData[x, y] = false;
                        // Decrement stretch count again
                        stretchCount--;
                    }
                    else
                    {
                        // Create another platform
                        int randomStretch = Random.Range(2, 7);
                        stretchCount = Mathf.Min(randomStretch, levelWidth - x);
                        CreateChildPrefab(platform, plaformParent, x * tileX, y * tileY, 0, stretchCount);

                        // Update level data
                        levelData[x, y] = true;

                        // decrement strechCount
                        stretchCount--;
                    }
                }

            }
        }

        // Spawning logic
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }

    // Instantiate a child platform under a parent
    // Alter its scale based on the stretch value
    void CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z, int stretch)
    {
        var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
        myPrefab.transform.localScale = new Vector3(stretch, 1, 1);
        myPrefab.transform.parent = parent.transform;
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        // infinite loop
        // TBC
        while (GameManager.isGameActive)
        {
            int enemyX = Random.Range(0, levelWidth - 1);
            int enemyY = Random.Range(0, levelHeight - 1);

            if (levelData[enemyX, enemyY])
            {
                // Instantiate an enemy if there is a platform on it
                // Randomize chance of what difficulty enemy appears
                int randomChoice = Random.Range(0,6);

                if (randomChoice < 3)
                {
                    // Spawn the easy enemy
                    Instantiate(enemies[0], new Vector3((enemyX * tileX) + 3, enemyY * tileY, 0), enemies[0].transform.rotation);
                }
                else if (randomChoice >= 3 && randomChoice < 5)
                {
                    // Spawn the normal enemy
                    Instantiate(enemies[1], new Vector3((enemyX * tileX) + 3, enemyY * tileY, 0), enemies[1].transform.rotation);
                }
                else
                {
                    // Spawn the hard enemy
                    Instantiate(enemies[2], new Vector3((enemyX * tileX) + 3, enemyY * tileY, 0), enemies[2].transform.rotation);
                }
            }
            
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (GameManager.isGameActive)
        {
            int powerupX = Random.Range(0, levelWidth - 1);
            int powerupY = Random.Range(0, levelHeight - 1);
            var powerup = powerups[Random.Range(0, 3)];

            if (levelData[powerupX, powerupY])
            {
                // Instantiate a powerup if there is a platform on it
                // Randomize chance of what powerup appears
                int randomChoice = Random.Range(0,6);

                if (randomChoice < 3)
                {
                    // Spawn either the extra strength, speed or invincibility
                    Instantiate(powerup, new Vector3((powerupX * tileX) + 3, (powerupY * tileY) + 2, 0), powerup.transform.rotation);
                }
                else if (randomChoice >= 3 && randomChoice < 5)
                {
                    // Spawn the health up
                    Instantiate(powerups[3], new Vector3((powerupX * tileX) + 3, (powerupY * tileY) + 2, 0), powerups[3].transform.rotation);
                }
                else
                {
                    // Spawn the gem
                    Instantiate(powerups[4], new Vector3((powerupX * tileX) + 3, (powerupY * tileY) + 2, 0), powerups[4].transform.rotation);
                }
            }
            
            yield return new WaitForSeconds(Random.Range(15, 20));
        }
    }
}
