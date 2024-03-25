using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComHandler : MonoBehaviour
{
    public Transform comMenu;
    public GameObject comOption;
    public Com currentCom;

    public void StartCom(Com com)
    {
        currentCom = com;
        CopyComData();
        comMenu.gameObject.SetActive(true);
    }

    public void EndComMenu()
    {
        foreach (Transform child in comMenu.GetChild(3))
        {
            Destroy(child.gameObject);
        }
        currentCom = null;
        comMenu.gameObject.SetActive(false);
    }

    void LoadNextCom(Com com)
    {
        foreach (Transform child in comMenu.GetChild(3))
        {
            Destroy(child.gameObject);
        }
        currentCom = com;
        if (currentCom == null) { Debug.LogError("Somethings fucked yo"); return; }
        CopyComData();
    }

    void CopyComData()
    {
        comMenu.GetChild(1).GetComponent<TMP_Text>().text = currentCom.storyTitle;
        comMenu.GetChild(2).GetComponent<TMP_Text>().text = currentCom.storyText;
        int i = 0;
        foreach (ComResponse r in currentCom.responses) {
            //if(dependance == true) {
            int i2 = i;
            Transform responseOptionButton = Instantiate(comOption, comMenu.GetChild(3)).transform;
            responseOptionButton.GetChild(0).GetComponent<TMP_Text>().text = i + 1 + ". " + r.responseText;
            if (r.isExit) responseOptionButton.GetComponent<Button>().onClick.AddListener(() => EndComMenu());
            else responseOptionButton.GetComponent<Button>().onClick.AddListener(() => DoOption(i2));
            i++;
        }
    }

    void DoOption(int option)
    {
        Debug.LogError(option);
        //Debug.LogError(currentCom.responses[option - 1].resultsBetween.Count);
        int i = Random.Range(0, currentCom.responses[option].resultsBetween.Count);
        LoadNextCom(currentCom.responses[option].resultsBetween[i]);
    }
}
