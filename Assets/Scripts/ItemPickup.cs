using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float ammoValue;
    public Transform target;
    public float moveSpeed;
    private float anim;
    private float startTime;
    public float duration;
    private Vector3 defaultScale;

    void Start()
    {
        //defaultScale = transform.localScale;
    }

    void Update()
    {
        if(target)
        {
            //transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed);
            anim = (Time.time - startTime) / duration;
            transform.position = Bezier(transform.position, (Vector2) target.position + Vector2.right * 2, target.position, anim);
            //if (transform.localscale.x > 0)
            //{
            //    transform.localscale = defaultscale * (1 - anim);
            //}
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(.1f, .1f, .1f), 0.1f);

        } else
        {
            startTime = Time.time;
        }
    }

    public Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            GameController.Instance.TrashCollected();
            SoundManager.Instance.Play("collect trash");
            Destroy(gameObject);
        }
    }
}
