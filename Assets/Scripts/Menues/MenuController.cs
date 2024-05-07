using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    bool gamePaused;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject map;
    [SerializeField] GameObject chunkMap;
    [SerializeField] GameObject inventory;
    [SerializeField] Object startScene;
    [SerializeField] Object mainMenuScene;
    [SerializeField] GameObject tutorial;

    public Transform mapSprites;
    public Transform bigMapHolder;
    public Transform smallMapHolder;

    bool inventoryActive;
    bool settingsActive;
    bool mapActive;
    //bool chunkMapActive;
    bool escActive;
    bool tutorialActive = true;

    public bool comActive;
    public bool screenActive;
    public Image newItemIcon;

    public bool isChangingScene = false;

    public static MenuController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        OpenTutorial();
    }
    void Update()
    {
        if (inventoryActive && Input.GetKeyDown(KeyCode.Escape)) Inventory();
        else if (mapActive && Input.GetKeyDown(KeyCode.Escape)) MapMode();
        else if (settingsActive && Input.GetKeyDown(KeyCode.Escape)) OpenSettings();
        else if (tutorialActive && Input.GetKeyDown(KeyCode.Escape)) OpenTutorial();
        else if (!comActive && Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if (!tutorialActive && !comActive && !escActive && !inventoryActive && !settingsActive && Input.GetKeyDown(KeyCode.M)) MapMode();
        if (!tutorialActive && !comActive && !escActive && !mapActive && !settingsActive && Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) Inventory();
        if (!comActive && !escActive && !mapActive && !settingsActive && Input.GetKeyDown(KeyCode.H)) OpenTutorial();
        if (!tutorialActive && !comActive && !escActive && !mapActive && Input.GetKeyDown(KeyCode.O)) OpenSettings();
    }

    public void OpenTutorial()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
        if (tutorial.activeSelf)
        {
            Time.timeScale = 1f;
            tutorial.SetActive(false);
            tutorialActive = false;
        }
        else
        {
            Time.timeScale = 0f;
            tutorial.SetActive(true);
            tutorialActive = true;
        }
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive || tutorialActive;
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
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive || tutorialActive;
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
        if (settingsActive)
        {
            settingsActive = false;
            settingsMenu.SetActive(false);
            if (escActive)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                gamePaused = false;
                Time.timeScale = 1f;
            }
            screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive || tutorialActive;
            return;
        }
        if (escActive)
        {
            pauseMenu.SetActive(false);
        }
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0f;
        }
        settingsActive = true;
        settingsMenu.SetActive(true);
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive || tutorialActive;
    }

    public void MapMode()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
        if (map.activeSelf) {
            mapSprites.SetParent(smallMapHolder);
            mapSprites.localScale = new(1, 1, 1);
            mapSprites.localPosition = new(0, 0, 0);
            map.SetActive(false); 
            mapActive = false; 
        }
        else {
            mapSprites.SetParent(bigMapHolder);
            mapSprites.localScale = new(1, 1, 1);
            mapSprites.localPosition = new(0, 0, 0);
            map.SetActive(true); 
            mapActive = true; 
        }
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive || tutorialActive;
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
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive;
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openInventory, transform.position);
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gamePaused = false;
        escActive = false;
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive;
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
        isChangingScene = true;
        SceneManager.LoadScene(1);
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive;
        yield return null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
        screenActive = comActive || escActive || inventoryActive || mapActive || settingsActive;
    }
}
