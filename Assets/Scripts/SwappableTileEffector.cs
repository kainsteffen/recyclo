using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappableTileEffector : MonoBehaviour
{
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask tileLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, tileLayer);

        if (collider) {
            collider.GetComponent<SwappableTile>().ShowGrassTile();
        }
    }
}
