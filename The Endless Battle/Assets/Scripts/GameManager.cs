using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // references
    public GameObject player;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;
    public GameObject endLevelUI;
    public GameObject inPlayUI;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI continueTimerText;

    // variables
    public static int level = 1;
    public static int score = 0;
    public static bool isGameActive = true;
    public float timer;
    public float continueTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().OnLevelLoaded();
        player.GetComponent<PlayerStatHandler>().OnLevelLoaded();
        timer = 120;
        Time.timeScale = 1f;
        isGameActive = true;
        continueTimer = 6;
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        // Display the amount of time we have left on the level
        DisplayTime();

        // Display what level we are currently on
        DisplayLevel();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isGameActive)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                
            }
            else
            {
                // If the player survived for the entire level then call this method
                EndOfLevel();
            }
        }
    }

    // Public method to add score
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void DisplayLevel()
    {
        levelText.text = "Level " + level;
    }

    public void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Called when the player is dead
    public void GameOver()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        inPlayUI.SetActive(false);
        endLevelUI.SetActive(false);
        gameOverUI.SetActive(true);
        finalScoreText.text = "Your Score: " + score;
        timer = 0;
        level = 1;
        score = 0;
    }

    public void EndOfLevel()
    {
        timer = 0;
        inPlayUI.SetActive(false);
        endLevelUI.SetActive(true);
        Time.timeScale = 0f;
        if (continueTimer > 0)
        {
            continueTimer -= Time.unscaledDeltaTime;
            float time = Mathf.Max(0, Mathf.FloorToInt(continueTimer % 60));
            continueTimerText.text = time.ToString();
        }
        else
        {
            Debug.Log("Working");
            Replay();
            isGameActive = false;
            
        }
    }

    // Reloads the scene to make a new level
    public void Replay()
    {
        level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Returns the player back to the main menu
    public void MainMenu()
    {
        Destroy(player);
        Time.timeScale = 1f;
        timer = 0;
        level = 1;
        score = 0;
        isGameActive = false;
        SceneManager.LoadScene("Main Menu");
    }
}
