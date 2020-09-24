using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScreen : MonoBehaviour
{
    // References to the pages and the animator for the scene transition
    public GameObject[] pages;
    public Animator transition;

    // index to track what page we are on
    public int currentPageIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentPageIndex = 0;
        pages[currentPageIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentPageIndex);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            pages[currentPageIndex].SetActive(false);

            currentPageIndex++;

            pages[currentPageIndex].SetActive(true);
        }
        else
        {
            StartGame();
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            pages[currentPageIndex].SetActive(false);

            currentPageIndex--;

            pages[currentPageIndex].SetActive(true);
        }
    }

    public void StartGame()
    {
        StartCoroutine(LoadAnim());
    }

    public IEnumerator LoadAnim()
    {
        // Plays transition animation and then load  the new scene
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("PlayScreen");
    }
}
