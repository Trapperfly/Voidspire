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
        ApplyComResult();
    }
    
    void ApplyComResult()
    {
        int i = 0;
        foreach (ComResultTotal result in currentCom.result)
        {
            switch (currentCom.result[i].result)
            {
                case ComResultEnum.None:
                    Debug.Log("Please set the result to nothing if that is intended");
                    break;
                case ComResultEnum.Nothing:
                    break;
                case ComResultEnum.DisengageCombat:
                    break;
                case ComResultEnum.EngageCombat:
                    break;
                case ComResultEnum.ResourceChange:
                    GlobalRefs.Instance.wallet += Mathf.RoundToInt(currentCom.result[i].resultValue.x);
                    break;
                case ComResultEnum.LootChange:
                    Equipment newEquipment = null;
                    if (currentCom.result[i].resultValue.x < 0)
                    {
                        //remove equipment or inventory items
                    }
                    else
                    {
                        for (int j = 0; j < Mathf.RoundToInt(currentCom.result[i].resultValue.x); j++)
                        {
                            switch (currentCom.result[i].resultValue.y)
                            {
                                case 0:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeRandom(currentCom.level);
                                    break;
                                case 2:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeGun(currentCom.level);
                                    break;
                                case 3:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeShield(currentCom.level);
                                    break;
                                case 4:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeThruster(currentCom.level);
                                    break;
                                case 5:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeHull(currentCom.level);
                                    break;
                                case 6:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeScanner(currentCom.level);
                                    break;
                                case 8:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeCollector(currentCom.level);
                                    break;
                                case 9:
                                    newEquipment = RandomizeEquipment.Instance.RandomizeRelic();
                                    break;
                                default:
                                    break;
                            }
                            Inventory.Instance.Add(newEquipment);
                        }
                    }
                    break;
                case ComResultEnum.PlayerHealthChange:
                    GlobalRefs.Instance.player.GetComponent<PlayerHealth>().currentHealth += currentCom.result[i].resultValue.x;
                    GlobalRefs.Instance.player.GetComponent<PlayerHealth>().UpdateHealth();
                    break;
                case ComResultEnum.PlayerFuelChange:
                    GlobalRefs.Instance.player.GetComponent<ShipControl>().ftl.fuelCurrent += currentCom.result[i].resultValue.x;
                    GlobalRefs.Instance.player.GetComponent<ShipControl>().UpdateFuel();
                    break;
                case ComResultEnum.ShipHealthChange:
                    break;
                case ComResultEnum.OpenShop:
                    break;
                case ComResultEnum.GiveEventPing:
                    break;
                case ComResultEnum.GiveBossClue:
                    //add event or ping, or add to a value and when that reaches max, add the boss event.
                    break;
                default:
                    break;
            }
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
