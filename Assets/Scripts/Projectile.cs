using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if ((cameraPoint.x > 1 || cameraPoint.y > 1) && gameObject.CompareTag("PlayerProjectile")) {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if ((cameraPoint.x > 1.0 || cameraPoint.y > 1.0) && gameObject.CompareTag("EnemyProjectile"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && gameObject.CompareTag("PlayerProjectile"))
        {
            SoundManager.Instance.Play("hit_garbage");
            StartCoroutine(FreezeFrame());
            collider.GetComponent<EnemyBase>().TakeDamage(damageAmount);
        }

        if (collider.CompareTag("Player") && gameObject.CompareTag("EnemyProjectile"))
        {
            Camera.main.DOShakePosition(0.1f, .5f);
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

    IEnumerator FreezeFrame()
    {
        float originalTimescale = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = originalTimescale;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
