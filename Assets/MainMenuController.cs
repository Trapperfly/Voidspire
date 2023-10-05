using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    bool savedRun;
    [SerializeField] Scene startScene;
    GameObject MainCanvas;
    [SerializeField] Button[] mainButtons;
    // Start is called before the first frame update
    void Awake()
    {
        //find saved run and swap savedRun to true
        //then enable the button if it finds a saved run, else disable button
        if (!savedRun) mainButtons[0].interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(startScene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
