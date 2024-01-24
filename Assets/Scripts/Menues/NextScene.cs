using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] float delay;
    float time = 0;

    void Update()
    {
        if (Input.anyKeyDown) LoadScene(1);

        time += 1 * Time.deltaTime;
        if (time >= delay) LoadScene(1);
    }

    void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
