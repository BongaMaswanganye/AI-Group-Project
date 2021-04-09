using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImEverywhere : MonoBehaviour
{
    public int spawnCount = 30;
    public Transform randomSpawn;
    // Start is called before the first frame update
    void Start()
    {
        randomSpawn = this.transform;
        randomSpawn.position = Random.insideUnitSphere * 5;

        StartCoroutine(MoreForever());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoreForever()
    {
        int i = 0;
        while(spawnCount > i)
        {
            yield return new WaitForSeconds(3);
            Instantiate(this, randomSpawn);
        }
    }
}
