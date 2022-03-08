using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Need a new gameoObject for this
public class CameraCubePool : MonoBehaviour
{
    [SerializeField] GameObject sample;

    // singleton
    static CameraCubePool instance;
    public int poolSize = 3072;

    GameObject[] pool;
    int currentPoolIndex = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(sample, instance.transform) as GameObject;

            pool[i].transform.localScale = new Vector3(9.5f, 9.5f, 9.5f);
            // crucial step
            pool[i].transform.parent = gameObject.transform;
            pool[i].SetActive(false);
        }
    }

    // method to take a bullet out
    public static void Take(Vector3 position, Quaternion rot)
    {
        if (++instance.currentPoolIndex >= instance.pool.Length)
            instance.currentPoolIndex = 0;

        // make sure it's deactivated
        instance.pool[instance.currentPoolIndex].SetActive(false);
        instance.pool[instance.currentPoolIndex].transform.position = position;
        instance.pool[instance.currentPoolIndex].transform.rotation = rot;
        instance.pool[instance.currentPoolIndex].SetActive(true);
    }
}
