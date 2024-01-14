using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    bool gamePaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject map;
    [SerializeField] GameObject inventory;
    [SerializeField] Object startScene;
    [SerializeField] Object mainMenuScene;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if (Input.GetKeyDown(KeyCode.M)) MapMode();
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) Inventory();
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

    public void Inventory()
    {
        if (inventory.activeSelf)
        {
            Time.timeScale = 1f;
            inventory.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            inventory.SetActive(true);
        }

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
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
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        yield return null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
}
