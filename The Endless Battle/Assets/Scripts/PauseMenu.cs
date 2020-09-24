using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Boolean to track if the game is paused
    public static bool isPaused = false;

    // references
    public GameObject pauseMenu;
    public GameObject inPlayUI;
    public GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if left shift is pressed
        if(GameManager.isGameActive && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume the game
    public void Resume()
    {
        pauseMenu.SetActive(false);
        inPlayUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Pause the game
    private void Pause()
    {
        pauseMenu.SetActive(true);
        inPlayUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
