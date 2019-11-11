using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour
{
	  public GameObject myPrefab;
	  public GameObject myPrefab1;
	  
	  public GameObject groundbase;
	  public GameObject groundbaseEnd;
	 
    // Start is called before the first frame update
    void Start()
    {
		// transform.position
        // Instantiate at position (0, 0, 0) and zero rotation.
       // Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
		
	 for (int i = 0; i < 10; ++i)
   {
	   if(i == 0)
	   {
		   SpawnLifeStrt(transform.position + new Vector3(i, 0, 0));
	   }
	   else if(i == 9)
	   {
        SpawnLifeEnd(transform.position + new Vector3(i, 0, 0));
	   }
	   else{
		   SpawnLife(transform.position + new Vector3(i, 0, 0)); // same as pos + (Vector3.right * i)
	   }
   }
    }	

    // Update is called once per frame
    void Update()
    {
        
    }
	
	 void SpawnLife(Vector3 spawnPosition)
 {
 //i was trying to modify this to something like  Instantiate(lifePrefab, spawnPosition+
 //lifePrefab.transform.right , lifePrefab.transform.rotation);
 
     Instantiate(myPrefab, spawnPosition , myPrefab.transform.rotation);
	     Instantiate(groundbase, spawnPosition , groundbase.transform.rotation);
	// Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
 }
 
  void SpawnLifeEnd(Vector3 spawnPosition)
 {
 //i was trying to modify this to something like  Instantiate(lifePrefab, spawnPosition+
 //lifePrefab.transform.right , lifePrefab.transform.rotation);
 
     GameObject newshit = Instantiate(myPrefab1, spawnPosition , myPrefab1.transform.rotation);
	 GameObject newstf2 = Instantiate(groundbaseEnd, spawnPosition , groundbaseEnd.transform.rotation);
	// newshit.transform.localScale = new Vector3 (-newshit.transform.localScale.x, newshit.transform.localScale.y, newshit.transform.localScale.z);
	// Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
 }
 
  void SpawnLifeStrt(Vector3 spawnPosition)
 {
 //i was trying to modify this to something like  Instantiate(lifePrefab, spawnPosition+
 //lifePrefab.transform.right , lifePrefab.transform.rotation);
 
     GameObject newshit = Instantiate(myPrefab1, spawnPosition , myPrefab1.transform.rotation);
	 newshit.transform.localScale = new Vector3 (-newshit.transform.localScale.x, newshit.transform.localScale.y, newshit.transform.localScale.z);
	  GameObject newstf2 = Instantiate(groundbaseEnd, spawnPosition , groundbaseEnd.transform.rotation);
	   newstf2.transform.localScale = new Vector3 (-newstf2.transform.localScale.x, newstf2.transform.localScale.y, newstf2.transform.localScale.z);
	// Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity); 
 }
 

}
