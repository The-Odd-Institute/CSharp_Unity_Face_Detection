using UnityEngine;

public class MyPerlin : MonoBehaviour
{
    public GameObject sample;
    public GameObject texturedObject;
    public float curHeightMult = 1f;



    // 3
    public float newheightMult = 1f;


    // 5
    public Texture2D[] textureSequence;
    public int curTextureNumber = 0;
    public int newTextureNumber = 0;


    void Start()
    {
        // Test
        //float height = Mathf.PerlinNoise(.5f, .5f);
        //print(height);


        // 3 - Disable
        Run();


        // 6
        InvokeRepeating("Sequence", 1, .25f);
    }


    // 6
    void Sequence ()
    {
        newTextureNumber += 1;
        newTextureNumber %= 6;
        //if (curTextureNumber > 5)
        //    curTextureNumber = 0;

        Run();
    }



    void Run()
    { 
        // you better try this on an unlit material
        Renderer renderer = texturedObject.GetComponent<Renderer>();
        Texture2D noiseTexture = new Texture2D(32, 32);


        // 5
      

        for (int x = 0; x < 32; x++)
        {
            for (int z = 0; z < 32; z++)
            {
                // texture matters
                float height = Mathf.PerlinNoise((float)x / 32f,
                    (float)z / 32f);


                // for 5
                // Color color = new Color(height, height, height);


                // 5
                int xSample = 512 / 32 * x;
                int ySample = 512 / 32 * z;
                Color color = textureSequence[curTextureNumber].GetPixel(xSample, ySample);

                noiseTexture.SetPixel(x, z, color);
                height = color.g;

                // 4 - pool
                // GameObject copy = GameObject.Instantiate(sample);
                // copy.transform.localScale = new Vector3(1, .1f, 1);
                float xLoc = x;
                float zLoc = z;

                // copy.transform.position = new Vector3(xLoc, height * curHeightMult, zLoc);
                // copy.transform.parent = gameObject.transform;


                CubePool.Take(new Vector3(xLoc, height * curHeightMult, zLoc),
                                Quaternion.identity);
            }
        }

        noiseTexture.Apply();
        renderer.material.mainTexture = noiseTexture;

    }

    // 3 add heigh changer
    void Update ()
    {
        if (newheightMult != curHeightMult)
        {
            curHeightMult = newheightMult;



            // delete all
            foreach (Transform child in gameObject.transform)
            {
                // 4 - pool
                // Destroy(child.gameObject);
            }

            // re - run 
            Run();
        }


        // 5 
        if (newTextureNumber != curTextureNumber)
        {
            curTextureNumber = newTextureNumber;

            // re - run 
            Run();
        }

    }
}
