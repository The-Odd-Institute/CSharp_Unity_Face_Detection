using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perlin_Feed : MonoBehaviour
{
    public Texture2D myFaceTexture;

    [Range(0, 11)]
    public int newTextureNumber = 0;
    public Texture2D[] textureSequence;

    public GameObject parent;
    public GameObject main;
    public GameObject textureObject;


    public int sceneSize = 512;
    float blockSize = 1f;
    public int texturesize = 32;
    float xOrig = 0f, yOrig = 0f;


    float curTextureZoomFactor = 1.0f;
    float curOffsetX = 0.0f;
    float curOffsetY = 0.0f;
    float curHeightMult = 0.0f;


    public float newTextureZoomFactor = 1.0f;
    public float newOffsetX = 0.0f;
    public float newOffsetY = 0.0f;
    public float newHeightMult = 0.0f;
    public int curTextureNumber = 0;


    WebCamTexture camTex;

   

    void Start()
    {
        setFactors();
    }

    void setFactors()
    {
        curTextureZoomFactor = newTextureZoomFactor;
        curOffsetX = newOffsetX;
        curOffsetY = newOffsetY;
        curHeightMult = newHeightMult;
        curTextureNumber = newTextureNumber;
    }


    bool changed = true;

    void Update()
    {
        if (changed)
        {
            main.SetActive(true);
            execute();
            main.SetActive(false);
        }

        if (newOffsetY != curOffsetY ||
            newOffsetX != curOffsetX ||
            newTextureZoomFactor != curTextureZoomFactor ||
            newHeightMult != curHeightMult ||
            newTextureNumber != curTextureNumber)
        {
            changed = true;
            setFactors();
        }
        else
            changed = false;

    }


    void calcuateBlockSize()
    {
        blockSize = (float)sceneSize / (float)texturesize;
        xOrig = -((float)sceneSize / 2f) + blockSize / 2f;
        yOrig = -((float)sceneSize / 2f) + blockSize / 2f;
    }


    void execute()
    {
        calcuateBlockSize();


        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        // you better try this on an unlit material
        Renderer renderer = textureObject.GetComponent<Renderer>();

        int xSample = 0;
        int ySample = 0;

        for (int x = 0; x < texturesize; x++)
        {
            for (int z = 0; z < texturesize; z++)
            {
                // texture matters
                xSample = Mathf.RoundToInt(blockSize * x);
                ySample = Mathf.RoundToInt(blockSize * z);

                //Color pixelColor = myFaceTexture.GetPixel(xSample, ySample);
                Color pixelColor = textureSequence[curTextureNumber].GetPixel(xSample, ySample);

                float height = pixelColor.g;

                // objects matters
                GameObject copy = GameObject.Instantiate(main);
                copy.transform.localScale = new Vector3(blockSize, 10f, blockSize);
                float xLoc = xOrig + (x * blockSize);
                float zLoc = yOrig + (z * blockSize);

                copy.transform.position = new Vector3(xLoc, height * curHeightMult, zLoc);
                copy.transform.parent = parent.transform;
            }
        }

        //renderer.material.mainTexture = myFaceTexture;
        renderer.material.mainTexture = textureSequence[curTextureNumber];
    }
}
