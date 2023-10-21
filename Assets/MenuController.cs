using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    bool gamePaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject map;
    [SerializeField] Object startScene;
    [SerializeField] Object mainMenuScene;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if (Input.GetKeyDown(KeyCode.M)) MapMode();
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else ResumeGame();
    }

    public void MapMode()
    {
        if (map.activeSelf) map.SetActive(false);
        else map.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void ExitGame()
    {
        StartCoroutine(SaveGame());
        //Then quit after
    }

    IEnumerator SaveGame()
    {
        //SaveHere
        StartCoroutine(QuitOut());
        yield return null;
    }

    IEnumerator QuitOut()
    {
        SceneManager.LoadScene(mainMenuScene.name);
        yield return null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(startScene.name);
        Time.timeScale = 1;
    }
}
