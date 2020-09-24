using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // reference the animator for the scene transition
    public Animator transition;

    // Start is called before the first frame update
    void Start()
    {
        // transition = GameObject.Find("LevelLoader").transform.Find("Fade").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadAnim());
    }
    
    public IEnumerator LoadAnim()
    {
        // Call the animation then load the new scene
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("InstructionsScreen");
    }
}
