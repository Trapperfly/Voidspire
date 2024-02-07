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

    public float alpha = 1;
    public MapMode mapMode = MapMode.normal;
    Texture2D texture;
    public RawImage rawImage;

    Vector2 pos;
    float dScale;
    float fScale;
    float yScale;
    float eScale;
    float sScale;

    float xSeed;
    float ySeed;
    float dSeed;
    float fSeed;
    float yiSeed;
    float eSeed;
    float sSeed;


    private void Start()
    {
        SetSeed();
        SetNewScales();
        StartCoroutine(GenerateTexture());
    }

    void SetNewScales()
    {
        dScale = ChunkLoader.debrisScale;
        fScale = ChunkLoader.factionScale;
        yScale = ChunkLoader.yieldScale;
        eScale = ChunkLoader.eventScale;
        sScale = ChunkLoader.shopScale;
    }

    void SetSeed()
    {
        xSeed = GlobalRefs.xSeed; 
        ySeed = GlobalRefs.ySeed;
        dSeed = GlobalRefs.debrisSeed;
        fSeed = GlobalRefs.factionSeed;
        yiSeed = GlobalRefs.yieldSeed;
        eSeed = GlobalRefs.eventSeed;
        sSeed = GlobalRefs.shopSeed;
    }

    public void SetAlpha(float newAlpha)
    {
        alpha = newAlpha;
    }

    public void SetRendering(int mode)
    {
        MapMode tempMode = MapMode.normal;
        switch (mode)
        {
            case 0:
                tempMode = MapMode.normal;
                break;
            case 1:
                tempMode = MapMode.combinedData;
                break;
            case 2:
                tempMode = MapMode.debrisMode;
                break;
            case 3:
                tempMode = MapMode.factionMode;
                break;
            case 4:
                tempMode = MapMode.yieldMode;
                break;
            case 5:
                tempMode = MapMode.eventMode;
                break;
            case 6:
                tempMode = MapMode.shopMode;
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
                                GenerateColorData(X, Y, fSeed, fScale, new Vector4(1, 0, 0, 1)).r,
                                GenerateColorData(X, Y, yiSeed, yScale, new Vector4(0, 1, 0, 1)).g,
                                GenerateColorData(X, Y, dSeed, dScale, new Vector4(0, 0, 1, 1)).b,
                                1);
                            tempTex.SetPixel(x, y, color);

                            break;
                        case MapMode.debrisMode:
                            color = GenerateColorData(X, Y, dSeed, dScale, new Vector4(0, 0, 1, 1));
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.factionMode:
                            color = GenerateColorData(X, Y, fSeed, fScale, new Vector4(1, 0, 0, 1));
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.yieldMode:
                            color = GenerateColorData(x, Y, yiSeed, yScale, new Vector4(0, 1, 0, 1));
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.eventMode:
                            color = GenerateColorData(X, Y, eSeed, eScale, ChunkLoader.eventThreshold, new Vector4(1, 0, 0, 1));
                            tempTex.SetPixel(x, y, color);
                            break;
                        case MapMode.shopMode:
                            color = GenerateColorData(X, Y, sSeed, sScale, ChunkLoader.shopThreshold, new Vector4(0, 1, 0, 1));
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

    Color GenerateColorData(int x, int y, float specificSeed, float specificScale, float limit, Vector4 whatColorChannel)
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
        Color color = whatColorChannel * Mathf.Pow(pValue, 3);
        color.a = alpha;
        return color;
    }
    Color GenerateColorData(int x, int y, float specificSeed, float specificScale, Vector4 whatColorChannel)
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

        Color color = whatColorChannel * Mathf.Pow(pValue, 3);
        color.a = alpha;
        return color;
    }

    public enum MapMode
    {
        normal,
        combinedData,
        debrisMode,
        factionMode,
        yieldMode,
        eventMode,
        shopMode
    }
}
