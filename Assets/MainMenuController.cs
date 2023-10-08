using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    bool savedRun;
    GameObject MainCanvas;
    [SerializeField] Button[] mainButtons;
    [SerializeField] Transform ShipSelectionCanvas;
    [SerializeField] Transform MainMenuCanvas;

    void Awake()
    {
        //find saved run and swap savedRun to true
        //then enable the button if it finds a saved run, else disable button
        if (!savedRun) mainButtons[0].interactable = false;
    }

    public void NewGame()
    {
        MainMenuCanvas.gameObject.SetActive(false);
        ShipSelectionCanvas.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
