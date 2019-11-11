using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (cameraPoint.x > 2 || cameraPoint.y > 2 && gameObject.CompareTag("PlayerProjectile")) {
            Destroy(gameObject);
        }

        if (cameraPoint.x > 1.0 || cameraPoint.y > 1.0 && gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && gameObject.CompareTag("PlayerProjectile"))
        {
            SoundManager.Instance.Play("hit_garbage");
            collider.GetComponent<EnemyBase>().TakeDamage(damageAmount);
        }

        if (collider.CompareTag("Player") && gameObject.CompareTag("EnemyProjectile"))
        {
            collider.GetComponent<PlayerController>().TakeDamage(damageAmount);
            Die();
        }

        if (collider.CompareTag("EnemyProjectile") && gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(collider.gameObject);
        }

        if (collider.CompareTag("SwappableTile") && gameObject.CompareTag("PlayerProjectile"))
        {
            collider.GetComponent<SwappableTile>().ShowGrassTile();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
