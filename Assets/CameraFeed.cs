
using UnityEngine;

public class CameraFeed : MonoBehaviour
{
    public GameObject texturedObject;
    // you better try this on an unlit material
    Renderer renderer;

    [SerializeField] float newheightMult = 1f;


    WebCamTexture camTex;
    // Start is called before the first frame update
    void Start()
    {
        renderer = texturedObject.GetComponent<Renderer>();



        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + i +  devices[i].name);
        }

       // Renderer rend = this.GetComponentInChildren<Renderer>();

        // assuming the first available WebCam is desired
        camTex = new WebCamTexture(devices[1].name);
        camTex.Play();


        camTex.requestedWidth = 640;
        camTex.requestedHeight = 480;
        camTex.requestedFPS = 12;

        //  rend.material.mainTexture = camTex;


        InvokeRepeating("Sequence", .5f, .5f);

    }


    // 6
    void Sequence()
    {
        //newTextureNumber += 1;
        //newTextureNumber %= 6;


        Run();
    }



    void Run()
    {


        texturedObject.GetComponent<Renderer>().material.mainTexture = camTex;


        for (int x = 0; x < 64; x++)
        {
            for (int z = 0; z < 48; z++)
            {
                Color col = camTex.GetPixel(x * 10, z * 10);

                // texture matters
                float height = col.r;

  
                float xLoc = x * 10;
                float zLoc = z * 10;


                CameraCubePool.Take(new Vector3(xLoc, height * newheightMult , zLoc),
                                Quaternion.identity);
            }
        }



    }


    private void Update()
    {
        //this.GetComponentInChildren<Renderer>();

        //renderer.material.mainTexture = camTex;


        //Color col = camTex.GetPixel(20, 20);

        //Debug.Log(col.r * 10);
    }
}




// 7 make it dynamic
