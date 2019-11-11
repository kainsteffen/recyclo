using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject[] flowers;
    public GameObject[] trashItems;
    public GameObject[] animals;
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }

        flowers = Resources.LoadAll<GameObject>("Flowers");
        trashItems = Resources.LoadAll<GameObject>("TrashItems");
        animals = Resources.LoadAll<GameObject>("Animals");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
