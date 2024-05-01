using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderChunkData : MonoBehaviour
{
    [SerializeField] float renderSpeed;
    [SerializeField] float renderDelay;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float scale;

    [SerializeField] Color debrisColor;
    [SerializeField] Color voidColor;
    [SerializeField] Color chitinColor;
    [SerializeField] Color chromeColor;
    [SerializeField] Color pirateColor;
    [SerializeField] Color civilizationColor;
    [SerializeField] Color yieldColor;
    [SerializeField] Color eventColor;
    [SerializeField] Color shopColor;
    [SerializeField] Color difEasyColor;
    [SerializeField] Color difMediumColor;
    [SerializeField] Color difHardColor;

    [SerializeField] Transform buttonsParent;
    [SerializeField] CameraController zoomer;

    public float alpha = 1;
    public MapMode mapMode = MapMode.normal;
    Texture2D texture;
    public RawImage rawImage;

    [Space]
    [SerializeField] Sprite topButton;
    [SerializeField] Sprite midButton;
    [SerializeField] Sprite botButton;
    [SerializeField] Transform availableSettings;
    [SerializeField] Transform[] extraSettings;

    Vector2 pos;
    float dScale;
    float voidScale;
    float chitinScale;
    float chromeScale;
    float pirateScale;
    float civScale;
    float yScale;
    float eScale;
    float sScale;
    float difScale;

    float xSeed;
    float ySeed;
    float dSeed;
    float voidSeed;
    float chitinSeed;
    float chromeSeed;
    float pirateSeed;
    float civSeed;
    float yiSeed;
    float eSeed;
    float sSeed;
    float difSeed;

    EquipmentController equipment;
    Scanner scanner;

    private void Awake()
    {
        EquipmentController.Instance.onGunLoadComplete += CustomStart;
    }

    private void CustomStart()
    {
        //Debug.Log("CustomStartActivated");
        equipment = EquipmentController.Instance;
        SetNewScanner();

        EquipmentController.Instance.onGunLoadComplete -= CustomStart;
        EquipmentController.Instance.onGunLoadComplete += SetNewScanner;
    }
    public void SetNewScanner()
    {
        bool sameScanner = false;
        if (scanner) sameScanner = scanner == equipment.scannerSlots[0].item as Scanner;
        scanner = equipment.scannerSlots[0].item as Scanner;
        if (scanner) { 
            renderDelay = scanner.mapUpdateSpeed;
            renderSpeed = scanner.mapUpdateAmount;
            SetMapButtons();
            if (!sameScanner) SetRendering(0);
            zoomer.minZoom = scanner.zoom.x;
            zoomer.maxZoom = scanner.zoom.y;
        }
        else { renderDelay = 0.01f; renderSpeed = 10; mapMode = MapMode.normal; SetMapButtons(); }
    }
    public void SetMapButtons()
    {
        foreach (Transform child in buttonsParent)
        {
            child.TryGetComponent(out Image image);
            image.sprite = midButton;
            child.gameObject.SetActive(false);
        }
        if (!scanner) { return; }
        switch (scanner.frequency)
        {
            // 0 = Normal
            // 1 = Combined
            // 2 = Debris
            // 3 = Faction
            // 4 = Void
            // 5 = Chitin
            // 6 = Chrome
            // 7 = Pirate
            // 8 = Civ
            // 9 = Yield
            // 10 = Event
            // 11 = Shop
            //case Frequencies.Friendly:
            //    buttonsParent.GetChild(0).gameObject.SetActive(true);
            //    buttonsParent.GetChild(4).gameObject.SetActive(true);
            //    break;
            case Frequencies.General:
                SetButtonsActive(0);
                SetImageToSprite(buttonsParent, 0, topButton);
                SetButtonsActive(1);
                SetImageToSprite(buttonsParent, 1, botButton);
                break;
            case Frequencies.Factions:
                SetButtonsActive(1);
                SetImageToSprite(buttonsParent, 1, topButton);
                SetButtonsActive(2); 
                SetButtonsActive(3); 
                SetButtonsActive(4);
                SetImageToSprite(buttonsParent, 4, botButton);
                //buttonsParent.GetChild(7).gameObject.SetActive(true);
                break;
            case Frequencies.Transmitters:
                SetButtonsActive(4);
                SetImageToSprite(buttonsParent, 4, topButton);
                SetButtonsActive(5);
                SetImageToSprite(buttonsParent, 5, botButton);
                break;
            //case Frequencies.Mining:
            //    buttonsParent.GetChild(2).gameObject.SetActive(true);
            //    buttonsParent.GetChild(9).gameObject.SetActive(true);
            //    break;
            //case Frequencies.Action:
            //    buttonsParent.GetChild(3).gameObject.SetActive(true);
            //    buttonsParent.GetChild(10).gameObject.SetActive(true);
            //    break;
            case Frequencies.Diplomat:
                SetButtonsActive(1);
                SetImageToSprite(buttonsParent, 1, topButton);
                SetButtonsActive(5);
                SetImageToSprite(buttonsParent, 5, botButton);
                //buttonsParent.GetChild(11).gameObject.SetActive(true);
                //buttonsParent.GetChild(7).gameObject.SetActive(true);
                break;
            case Frequencies.Broad:
                SetButtonsActive(0);
                SetImageToSprite(buttonsParent, 0, topButton);
                SetButtonsActive(4); 
                SetButtonsActive(5);
                SetImageToSprite(buttonsParent, 5, botButton);
                //buttonsParent.GetChild(10).gameObject.SetActive(true);
                //buttonsParent.GetChild(8).gameObject.SetActive(true);
                //buttonsParent.GetChild(2).gameObject.SetActive(true);
                break;
            case Frequencies.Default:
                break;
            default:
                break;
        }
    }

    private void SetButtonsActive(int i)
    {
        buttonsParent.GetChild(i).gameObject.SetActive(true);
        availableSettings.GetChild(i).gameObject.SetActive(true);
    }

    private void SetImageToSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }
    private void SetImageToSprite(Transform parent, int child, Sprite sprite)
    {
        parent.GetChild(child).GetComponent<Image>().sprite = sprite;
    }

    private void Start()
    {
        SetSeed();
        SetNewScales();
        StartCoroutine(GenerateTexture());
    }

    void SetNewScales()
    {
        dScale = ChunkLoader.debrisScale;
        voidScale = ChunkLoader.voidScale;
        chitinScale = ChunkLoader.chitinScale;
        chromeScale = ChunkLoader.chromeScale;
        pirateScale = ChunkLoader.pirateScale;
        civScale = ChunkLoader.civScale;
        yScale = ChunkLoader.yieldScale;
        eScale = ChunkLoader.eventScale;
        sScale = ChunkLoader.shopScale;
        difScale = ChunkLoader.difScale;
    }

    void SetSeed()
    {
        xSeed = GlobalRefs.xSeed; 
        ySeed = GlobalRefs.ySeed;
        dSeed = GlobalRefs.debrisSeed;
        voidSeed = GlobalRefs.voidSeed;
        chitinSeed = GlobalRefs.chitinSeed;
        chromeSeed = GlobalRefs.chromeSeed;
        pirateSeed = GlobalRefs.pirateSeed;
        civSeed = GlobalRefs.civSeed;
        yiSeed = GlobalRefs.yieldSeed;
        eSeed = GlobalRefs.eventSeed;
        sSeed = GlobalRefs.shopSeed;
        difSeed = GlobalRefs.difSeed;
    }

    public void SetAlpha(float newAlpha)
    {
        alpha = newAlpha;
    }

    void SetMapSettingIcon(int setting)
    {
        foreach(Transform child in availableSettings)
        {
            child.GetComponent<Image>().color = new(1, 1, 1, 0.5f);
            child.gameObject.SetActive(false);
        }
        extraSettings[0].gameObject.SetActive(false);
        extraSettings[1].gameObject.SetActive(false);
        switch (setting)
        {
            case 0 or 1 or 2 or 3 or 4 or 5:
                availableSettings.GetChild(setting).gameObject.SetActive(true);
                availableSettings.GetChild(setting).GetComponent<Image>().color = new(1, 1, 1, 1);
                break;
            case 10:
                extraSettings[0].gameObject.SetActive(true);
                break;
            case 11:
                extraSettings[1].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetRendering(int mode)
    {
        MapMode tempMode = MapMode.normal;
        switch (mode)
        {
            case 0:
                tempMode = MapMode.normal;
                SetMapSettingIcon(10);
                break;
            case 1:
                tempMode = MapMode.combinedData;
                break;
            case 2:
                tempMode = MapMode.debrisMode;
                SetMapSettingIcon(0);
                break;
            case 3:
                tempMode = MapMode.factionMode;
                SetMapSettingIcon(4);
                break;
            case 4:
                tempMode = MapMode.voidMode;
                SetMapSettingIcon(3);
                break;
            case 5:
                tempMode = MapMode.chitinMode;
                SetMapSettingIcon(2);
                break;
            case 6:
                tempMode = MapMode.chromeMode;
                break;
            case 7:
                tempMode = MapMode.pirateMode;
                SetMapSettingIcon(1);
                break;
            case 8:
                tempMode = MapMode.civMode;
                break;
            case 9:
                tempMode = MapMode.yieldMode;
                break;
            case 10:
                tempMode = MapMode.eventMode;
                SetMapSettingIcon(5);
                break;
            case 11:
                tempMode = MapMode.shopMode;
                break;
            case 12:
                tempMode = MapMode.difMode;
                SetMapSettingIcon(11);
                break;
            default:
                Debug.Log("Wrong map mode");
                break;
        }    
        mapMode = tempMode;
    }

    IEnumerator GenerateTexture()
    {
        int i = 0;
        _ = new Color();
        Texture2D tempTex = new(width, height)
        {
            filterMode = FilterMode.Point
        };
        while (true)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pos = GlobalRefs.playerPos;
                    int X = Mathf.RoundToInt(x - (width / 2) + (pos.x) / scale);//ChunkLoader.Instance.chunkSize);
                    int Y = Mathf.RoundToInt(y - (height / 2) + (pos.y) / scale);//ChunkLoader.Instance.chunkSize);
                    Color color;
                    switch (mapMode)
                    {
                        case MapMode.normal:
                            color = new Color(0, 0, 0, 0);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.combinedData:
                            color = new Color(
                            new Color ((
                                GenerateColorData(X, Y, voidSeed, voidScale, voidColor, 3)
                                + GenerateColorData(X, Y, chitinSeed, chitinScale, chitinColor, 3)
                                + GenerateColorData(X, Y, chromeSeed, chromeScale, chromeColor, 3)
                                + GenerateColorData(X, Y, pirateSeed, pirateScale, pirateColor, 3)
                                ).grayscale,
                                0, 0, 1
                                ).r,
                                GenerateColorData(X, Y, yiSeed, yScale, new Vector4(0, 1, 0, 1), 3).g,
                                GenerateColorData(X, Y, dSeed, dScale, new Vector4(0, 0, 1, 1), 3).b,
                                1);
                            tempTex.SetPixel(x, y, color);

                            break;
                        case MapMode.debrisMode:
                            color = GenerateColorData(X, Y, dSeed, dScale, debrisColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.factionMode:
                            color = new Color((
                                GenerateColorData(X, Y, voidSeed, voidScale, voidColor, 3)
                                + GenerateColorData(X, Y, chitinSeed, chitinScale, chitinColor, 3)
                                + GenerateColorData(X, Y, chromeSeed, chromeScale, chromeColor, 3)
                                + GenerateColorData(X, Y, pirateSeed, pirateScale, pirateColor, 3)
                                ).grayscale,
                                0, 0, 1
                                );
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.voidMode:
                            color = GenerateColorData(X, Y, voidSeed, voidScale, voidColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.chitinMode:
                            color = GenerateColorData(X, Y, chitinSeed, chitinScale, chitinColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.chromeMode:
                            color = GenerateColorData(X, Y, chromeSeed, chromeScale, chromeColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.pirateMode:
                            color = GenerateColorData(X, Y, pirateSeed, pirateScale, pirateColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.civMode:
                            color = GenerateColorData(X, Y, civSeed, civScale, civilizationColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.yieldMode:
                            color = GenerateColorData(x, Y, yiSeed, yScale, yieldColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.eventMode:
                            color = GenerateColorData(X, Y, eSeed, eScale, ChunkLoader.eventThreshold, eventColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.shopMode:
                            color = GenerateColorData(X, Y, sSeed, sScale, ChunkLoader.shopThreshold, shopColor, 3);
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.difMode:
                            color = GenerateColorData(X, Y, difSeed, difScale, Color.white, 2);
                            float hue = Mathf.Lerp( 0.5f, -0.25f, color.grayscale);
                            color = Random.ColorHSV(hue, hue, 1, 1, 0.5f, 0.5f);
                            tempTex.SetPixel(x, y, color);
                            break;
                        default:
                            break;
                    }
                    tempTex.Apply();
                    i++;
                    if (i >= renderSpeed) { yield return new WaitForSeconds(renderDelay); i = 0; }
                    Debug.Log("Doing a thing");
                    texture = tempTex;
                    rawImage.texture = texture;
                }
            }
        }
    }

    Color GenerateColorData(int x, int y, float specificSeed, float specificScale, float limit, Vector4 whatColorChannel, int power)
    {
        float xCoord = (float)x / width;
        float yCoord = (float)y / height;

        float pValue = Mathf.PerlinNoise
            (   
            x
            * specificScale
            //* scale
            + xSeed 
            + specificSeed
            , 
            y
            * specificScale
            //* scale
            + ySeed 
            + specificSeed);
        if (pValue < limit) { return Color.black; }
        Color color = whatColorChannel * Mathf.Pow(pValue, power);
        color.a = alpha;
        return color;
    }
    Color GenerateColorData(int x, int y, float specificSeed, float specificScale, Vector4 whatColorChannel, int power)
    {
        float xCoord = (float)x / width;
        float yCoord = (float)y / height;


        float pValue = Mathf.PerlinNoise
            (
            x
            * specificScale
            //* scale
            + xSeed
            + specificSeed
            ,
            y
            * specificScale
            //* scale
            + ySeed
            + specificSeed);

        Color color = whatColorChannel * Mathf.Pow(pValue, power);
        color.a = alpha;
        return color;
    }

    public enum MapMode
    {
        normal,
        combinedData,
        debrisMode,
        factionMode,
        voidMode,
        chitinMode,
        chromeMode,
        pirateMode,
        civMode,
        yieldMode,
        eventMode,
        shopMode,
        difMode
    }
}
