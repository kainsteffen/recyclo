using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashKitten : EnemyBase
{
    public int trashDropCount;
    public GameObject[] trashItems;
    public float trashTargetAssignDelay;
    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        trashItems = ObjectPool.Instance.trashItems;
    }

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        GameController.Instance.AnimalSaved();
        SpawnTrash();
    }

    void SpawnTrash()
    {
        for(int i = 0; i < trashDropCount; i++)
        {
            GameObject randomTrashItem = trashItems[Random.Range(0, trashItems.Length)];
            Vector2 randomTrashPos = new Vector2(
                transform.position.x + Random.Range(-(collider.size.x / 2), collider.size.x / 2), 
                transform.position.y + Random.Range(-(collider.size.y / 2), collider.size.y / 2));
            GameObject newTrash = Instantiate(randomTrashItem, randomTrashPos, randomTrashItem.transform.rotation);
            StartCoroutine(DelayTargetAssign(newTrash, trashTargetAssignDelay + (i * 0.1f)));
        }
        StartCoroutine(DelayDeath());
        isDead = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Instantiate(ObjectPool.Instance.animals[Random.Range(0, ObjectPool.Instance.animals.Length)], transform.position, transform.rotation);
    }

    IEnumerator DelayTargetAssign(GameObject trash, float delay)
    {
        yield return new WaitForSeconds(delay);
        trash.GetComponent<ItemPickup>().target = GameController.Instance.player.transform;
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(trashTargetAssignDelay + 1);
        Destroy(gameObject);
    }
}
