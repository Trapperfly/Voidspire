using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipSelectionController : MonoBehaviour
{
    [SerializeField] float flipSpeedInSeconds;
    [SerializeField] List<GameObject> shipPrefabs;

    [SerializeField] ShipManager manager;
    [SerializeField] Transform ShipSelectionCanvas;
    [SerializeField] Transform MainMenuCanvas;
    [SerializeField] Object startScene;
    [SerializeField] Transform shipRotunda;
    [SerializeField] float shipOffset = 1f;
    bool ready = true;
    int selectedShip = 0;
    GameObject SelectedShip;
    GameObject ShipBack;
    GameObject ShipFor;
    GameObject ShipTempSlot;
    bool initFinished;

    [SerializeField] Vector3 shipPos;

    private void Awake()
    {
        selectedShip = manager.selectedShip;
    }
    private void OnEnable()
    {
        if (!initFinished) InitShips();
    }

    void InitShips()
    {
        int initShip = selectedShip - 1;
        if (initShip < 0) initShip = shipPrefabs.Count - 1;
        ShipBack = Instantiate(shipPrefabs[initShip], shipRotunda.position - new Vector3(shipRotunda.position.x + shipOffset, shipRotunda.position.y, 0), new Quaternion(0, 0, 0, 0), shipRotunda);
        if (initShip != 0) ShipBack.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.05f);

        SelectedShip = Instantiate(shipPrefabs[selectedShip], shipRotunda.position, new Quaternion(0, 0, 0, 0), shipRotunda);
        if (selectedShip != 0) SelectedShip.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.05f);

        initShip = selectedShip + 1;
        if (initShip > shipPrefabs.Count - 1) initShip = 0;
        ShipFor = Instantiate(shipPrefabs[initShip], shipRotunda.position + new Vector3(shipRotunda.position.x + shipOffset, shipRotunda.position.y, 0), new Quaternion(0, 0, 0, 0), shipRotunda);
        if (initShip != 0) ShipFor.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.05f);
        initFinished = true;
    }

    public void SwapShip(int way)
    {
        if (way == 1 && ready) StartCoroutine(FlipShips(true));
        else if (way == -1 && ready) StartCoroutine(FlipShips(false));
        else Debug.Log("Not ready");
    }

    IEnumerator FlipShips(bool forward)
    {
        ready = false;
        int prepareShip;
        int direction;
        if (forward)
        {
            //Prepare values to be used in the flip
            prepareShip = selectedShip + 2;
            Debug.Log("Preparing ship nr " + prepareShip);
            if (prepareShip > shipPrefabs.Count - 1)
            {
                prepareShip -= shipPrefabs.Count;
                Debug.Log("Altered to " + prepareShip);
            }
            direction = -1;
            ShipTempSlot = 
                Instantiate
                (
                    shipPrefabs[prepareShip],
                    new Vector3(shipRotunda.position.x + (shipOffset * 2),
                    shipRotunda.position.y, 0),
                    new Quaternion(0, 0, 0, 0),
                    shipRotunda);
            if (prepareShip != 0) ShipTempSlot.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.05f);
        }
        else
        {
            //Prepare values to be used in the flip
            prepareShip = selectedShip - 2;
            Debug.Log("Preparing ship nr " + prepareShip);
            if (prepareShip < 0)
            {
                prepareShip += shipPrefabs.Count;
                Debug.Log("Altered to " + prepareShip);
            } 
            direction = 1;
            ShipTempSlot = 
                Instantiate
                (
                    shipPrefabs[prepareShip],
                    new Vector3(shipRotunda.position.x - (shipOffset * 2),
                    shipRotunda.position.y, 0),
                    new Quaternion(0, 0, 0, 0),
                    shipRotunda);
            if (prepareShip != 0) ShipTempSlot.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.05f);
        }
        //Perform flip
        for (int i = 0; i < flipSpeedInSeconds * 60; i++)
        {
            SelectedShip.transform.position += new Vector3(shipRotunda.position.x + (shipOffset / (flipSpeedInSeconds * 60) * direction), 0, 0);
            ShipFor.transform.position += new Vector3(shipRotunda.position.x + (shipOffset / (flipSpeedInSeconds * 60) * direction), 0, 0);
            ShipBack.transform.position += new Vector3(shipRotunda.position.x + (shipOffset / (flipSpeedInSeconds * 60) * direction), 0, 0);
            ShipTempSlot.transform.position += new Vector3(shipRotunda.position.x + (shipOffset / (flipSpeedInSeconds * 60) * direction), 0, 0);
            yield return new WaitForFixedUpdate();
        }
        //Force ships to right location
        /*
        SelectedShip.transform.position = new Vector3(0,0,0);
        ShipFor.transform.position = new Vector3(shipOffset,0,0);
        ShipBack.transform.position = new Vector3(-shipOffset,0,0);
        ShipTempSlot.transform.position = new Vector3(2 * shipOffset * direction,0,0);
        ^*/
        yield return null;
        //Set proper slots
        if (forward)
        {
            Destroy(ShipBack);
            ShipBack = SelectedShip;
            SelectedShip = ShipFor;
            ShipFor = ShipTempSlot;
            ShipTempSlot = null;
            selectedShip += 1;
            if (selectedShip > shipPrefabs.Count - 1) selectedShip = 0;

            SelectedShip.transform.position = new Vector3(shipRotunda.position.x, shipRotunda.position.y, 0);
            ShipFor.transform.position = new Vector3(shipRotunda.position.x + shipOffset, shipRotunda.position.y, 0);
            ShipBack.transform.position = new Vector3(shipRotunda.position.x - shipOffset, shipRotunda.position.y, 0);
            yield return null;
        }
        else
        {
            Destroy(ShipFor);
            ShipFor = SelectedShip;
            SelectedShip = ShipBack;
            ShipBack = ShipTempSlot;
            ShipTempSlot = null;
            selectedShip -= 1;
            if (selectedShip < 0) selectedShip = shipPrefabs.Count-1;

            SelectedShip.transform.position = new Vector3(shipRotunda.position.x, shipRotunda.position.y, 0);
            ShipFor.transform.position = new Vector3(shipRotunda.position.x + shipOffset, shipRotunda.position.y, 0);
            ShipBack.transform.position = new Vector3(shipRotunda.position.x - shipOffset, shipRotunda.position.y, 0);
            yield return null;
        }
        ready = true;
        yield return null;
    }

    public void StartMoveShips()
    {
        StartCoroutine(nameof(MoveShips));
    }
    IEnumerator MoveShips()
    {
        SimpleMove moveShipBack = ShipBack.GetComponent<SimpleMove>();
        if (!moveShipBack.active)
        {
            if (!moveShipBack.back) moveShipBack.positions[1] = ShipBack.transform.position + new Vector3(0, 8, 0);
            moveShipBack.StartMove(0,1);
        }
        SimpleMove moveSelectedShip = SelectedShip.GetComponent<SimpleMove>();
        if (!moveSelectedShip.active)
        {
            if (!moveSelectedShip.back) moveSelectedShip.positions[1] = SelectedShip.transform.position + shipPos;
            moveSelectedShip.StartMove(0, 1);
        }
        SimpleMove moveShipFor = ShipFor.GetComponent<SimpleMove>();
        if (!moveShipFor.active)
        {
            if (!moveShipFor.back) moveShipFor.positions[1] = ShipFor.transform.position + new Vector3(0, 8, 0);
            moveShipFor.StartMove(0, 1);
        }
        yield return new WaitForEndOfFrame();
    }

    void UpdatePreviewedShip()
    {

    }

    public void ExitToMainMenu()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        ShipSelectionCanvas.gameObject.SetActive(false);
        foreach (Transform child in shipRotunda)
        {
            Destroy(child.gameObject);
        }
        initFinished = false;
    }

    public void StartGame()
    {
        //Set selected ship to global settings for the scene to use
        manager.selectedShip = selectedShip;
        if (selectedShip != 0)
        {

        }else SceneManager.LoadScene(2);
    }
}
