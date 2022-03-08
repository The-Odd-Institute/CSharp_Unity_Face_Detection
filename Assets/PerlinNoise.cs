using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
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

    void Start ()
    {
        setFactors();
    }

    void setFactors ()
    {
        curTextureZoomFactor = newTextureZoomFactor;
        curOffsetX = newOffsetX;
        curOffsetY = newOffsetY;
        curHeightMult = newHeightMult;
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
            newHeightMult != curHeightMult)
        {
            changed = true;
            setFactors();
        }
        else
            changed = false;
    }

    void calcuateBlockSize ()
    {
        blockSize = (float)sceneSize / (float)texturesize;
        xOrig = -((float)sceneSize / 2f) + blockSize / 2f;
        yOrig = -((float)sceneSize / 2f) + blockSize / 2f;
    }

    void execute()
    {
        calcuateBlockSize();


        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        // you better try this on an unlit material
        Renderer renderer = textureObject.GetComponent<Renderer>();
        Texture2D noiseTexture = new Texture2D(texturesize, texturesize);
        for (int x = 0; x < texturesize; x++)
        {
            for (int z = 0; z < texturesize; z++)
            {
                // texture matters
                float height = Mathf.PerlinNoise((float)x / (float)texturesize * curTextureZoomFactor + curOffsetX,
                    (float)z / (float)texturesize * curTextureZoomFactor + curOffsetY);
                Color color = new Color(height, height, height);
                noiseTexture.SetPixel(x, z, color);

                // objects matters
                GameObject copy = GameObject.Instantiate(main);
                copy.transform.localScale = new Vector3(blockSize, .1f, blockSize);
                float xLoc = xOrig + (x * blockSize);
                float zLoc = yOrig + (z * blockSize);

                copy.transform.position = new Vector3(xLoc, height * curHeightMult, zLoc);
                copy.transform.parent = gameObject.transform;
            }
        }

        noiseTexture.Apply();
        renderer.material.mainTexture = noiseTexture;
    }
}