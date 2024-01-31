using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    bool gamePaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject map;
    [SerializeField] GameObject chunkMap;
    [SerializeField] GameObject inventory;
    [SerializeField] Object startScene;
    [SerializeField] Object mainMenuScene;

    bool inventoryActive;
    bool mapActive;
    bool chunkMapActive;
    bool escActive;
    void Update()
    {
        if (inventoryActive && Input.GetKeyDown(KeyCode.Escape)) Inventory();
        else if (mapActive && Input.GetKeyDown(KeyCode.Escape)) MapMode();
        else if (chunkMapActive && Input.GetKeyDown(KeyCode.Escape)) ChunkMapMode();
        else if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if (!escActive && !inventoryActive && !chunkMapActive && Input.GetKeyDown(KeyCode.M)) MapMode();
        if (!escActive && !inventoryActive && !mapActive && Input.GetKeyDown(KeyCode.N)) ChunkMapMode();
        if (!escActive && !mapActive && Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) Inventory();
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            escActive = true;
            gamePaused = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else ResumeGame();
    }

    public void MapMode()
    {
        if (map.activeSelf) { map.SetActive(false); mapActive = false; }
        else { map.SetActive(true); mapActive = true; }
    }

    public void ChunkMapMode()
    {
        if (chunkMap.activeSelf) { chunkMap.SetActive(false); chunkMapActive = false; }
        else { chunkMap.SetActive(true); chunkMapActive = true; }
    }

    public void Inventory()
    {
        if (inventory.activeSelf)
        {
            Time.timeScale = 1f;
            inventory.SetActive(false);
            inventoryActive = false;
        }
        else
        {
            Time.timeScale = 0f;
            inventory.SetActive(true);
            inventoryActive = true;
        }

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gamePaused = false;
        escActive = false;
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
