using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappableTile : MonoBehaviour
{
    public GameObject dirtTile;
    public GameObject grassTile;
    private GameObject[] flowers;
    private bool hasFlower = false;

    private void Awake()
    {
        flowers = ObjectPool.Instance.flowers;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGrassTile()
    {
        grassTile.SetActive(true);
        dirtTile.SetActive(false);
        if(!hasFlower)
        {
            hasFlower = true;
            Instantiate(flowers[Random.Range(0, flowers.Length)], new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.rotation);
            GameController.Instance.FlowerPlanted();
        }
    }

    public void ShowDirtTile()
    {
        grassTile.SetActive(false);
        dirtTile.SetActive(true);
    }
}
