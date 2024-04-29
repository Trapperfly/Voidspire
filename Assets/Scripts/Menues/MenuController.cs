using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    bool gamePaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject map;
    [SerializeField] GameObject chunkMap;
    [SerializeField] GameObject inventory;
    [SerializeField] Object startScene;
    [SerializeField] Object mainMenuScene;

    public Transform mapSprites;
    public Transform bigMapHolder;
    public Transform smallMapHolder;

    bool inventoryActive;
    bool mapActive;
    //bool chunkMapActive;
    bool escActive;

    public Image newItemIcon;
    void Update()
    {
        if (inventoryActive && Input.GetKeyDown(KeyCode.Escape)) Inventory();
        else if (mapActive && Input.GetKeyDown(KeyCode.Escape)) MapMode();
        else if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if (!escActive && !inventoryActive && Input.GetKeyDown(KeyCode.M)) MapMode();
        if (!escActive && !mapActive && Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) Inventory();
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
        if (map.activeSelf) {
            mapSprites.SetParent(smallMapHolder);
            mapSprites.localScale = new(1, 1);
            mapSprites.localPosition = new(0, 0, 0);
            map.SetActive(false); 
            mapActive = false; 
        }
        else {
            mapSprites.SetParent(bigMapHolder);
            mapSprites.localScale = new(1, 1);
            mapSprites.localPosition = new(0, 0, 0);
            map.SetActive(true); 
            mapActive = true; 
        }
    }

    //public void ChunkMapMode()
    //{
    //    if (chunkMap.activeSelf) { chunkMap.SetActive(false); chunkMapActive = false; }
    //    else { chunkMap.SetActive(true); chunkMapActive = true; }
    //}

    public void Inventory()
    {
        newItemIcon.color = new(1, 1, 1, 0);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
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
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
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
