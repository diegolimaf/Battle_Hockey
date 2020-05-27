using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public static bool pausedGame = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausedGame)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        pausedGame = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pausedGame = false;
    }
    public void Restart()
    {
        pausedGame = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        pausedGame = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
}
