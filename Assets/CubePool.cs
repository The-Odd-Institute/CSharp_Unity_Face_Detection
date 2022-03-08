using UnityEngine;


// 7 make it dynamic


// Need a new gameoObject for this
public class CubePool : MonoBehaviour
{
    // singleton
    static CubePool instance;
    public GameObject sample;
    public int poolSize = 1024;

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

            pool[i].transform.localScale = new Vector3(.95f, .5f, .95f);
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
