using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject initLevel;
    public GameObject[] levelPool;
    public BoxCollider2D currentSection;
    public BoxCollider2D nextSection;
    public List<GameObject> activeSections;
    public float removalDistance;

    private void Awake()
    {
        levelPool = Resources.LoadAll<GameObject>("LevelSections");
        activeSections = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentSection.bounds.Contains(GameController.Instance.player.transform.position) && nextSection == null)
        {
            GameObject randomLevel = levelPool[Random.Range(0, levelPool.Length)];
            float currentSectionXBound = ((Vector2)currentSection.transform.position - currentSection.offset).x + (currentSection.bounds.size.x / 2);
            GameObject newSection = Instantiate(
                randomLevel,
                new Vector3(currentSectionXBound,
                transform.position.y,
                transform.position.z),
                transform.rotation);
            activeSections.Add(newSection);
            newSection.transform.Translate(new Vector2(newSection.GetComponent<BoxCollider2D>().bounds.size.x / 2, 0));
            nextSection = newSection.GetComponent<BoxCollider2D>();
        }

        if (nextSection && nextSection.bounds.Contains(GameController.Instance.player.transform.position))
        {
            currentSection = nextSection;
            nextSection = null;     
        }



        RemoveOldSections();
    }

    void RemoveOldSections()
    {
        for(int i = activeSections.Count - 1; i > -1; i--)
        {
            
            GameObject section = activeSections[i];
            if (Vector2.Distance(section.transform.position, GameController.Instance.player.transform.position) > removalDistance 
                && (section.transform.position.x + section.GetComponent<BoxCollider2D>().bounds.size.x / 2) < GameController.Instance.player.transform.position.x - GameController.Instance.player.GetComponent<BoxCollider2D>().bounds.size.x / 2)
            {
                activeSections.RemoveAt(i);
                Destroy(section);
            }
        }
    }

    public void ResetLevel()
    {
        RemoveAllSections();
        if(initLevel)
        {
            GameObject newSection = Instantiate(
                initLevel,
                transform.position,
                transform.rotation
            );
            activeSections.Add(newSection);
            currentSection = newSection.GetComponent<BoxCollider2D>();
            nextSection = null;
        } else
        {
            GameObject randomLevel = levelPool[Random.Range(0, levelPool.Length)];
            GameObject newSection = Instantiate(
                randomLevel,
                transform.position,
                transform.rotation
            );
            activeSections.Add(newSection);
            currentSection = newSection.GetComponent<BoxCollider2D>();
            nextSection = null;
        }
    }

    void RemoveAllSections()
    {
        for (int i = activeSections.Count - 1; i > -1; i--)
        {
            GameObject section = activeSections[i];
            activeSections.RemoveAt(i);
            Destroy(section);
        }
    }
}
