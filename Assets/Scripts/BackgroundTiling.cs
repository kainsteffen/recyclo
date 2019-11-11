using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SpriteRenderer))]
public class BackgroundTiling : MonoBehaviour
{
    public int offsetX = 2;

    public bool hasRight = false;
    public bool hasLeft = false;

    public bool reverseScale = false;

    public Transform parents;

    private float spriteWidth = 0f;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Use this for initialization
    void Start ()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        spriteWidth = renderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (hasLeft == false || hasRight == false)
        {
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            float edgeVisiblePositionRight = (transform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (transform.position.x - spriteWidth / 2) + camHorizontalExtend;

            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasRight == false)
            {
                MakeNewTile(1);
                hasRight = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasLeft == false)
            {
                MakeNewTile(-1);
                hasLeft = true;
            }

        }
	}

    void MakeNewTile(int direction)
    {
        Vector3 newPos = new Vector3(transform.position.x + spriteWidth * direction, transform.position.y, transform.position.z);
        Transform newTile = Instantiate(transform, newPos, transform.rotation);

        if (reverseScale == true)
        {
            newTile.localScale = new Vector3(newTile.localScale.x * -1, newTile.localScale.y, newTile.localScale.z);
        }

        newTile.parent = transform;

        if (direction > 0)
        {
            newTile.GetComponent<BackgroundTiling>().hasLeft = true;
        }
        else
        {
            newTile.GetComponent<BackgroundTiling>().hasRight = true;
        }
    }
}
