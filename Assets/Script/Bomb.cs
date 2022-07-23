using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    
    GameObject[] list;
      
    [SerializeField]float intervalForSpawn = 3f;
    float nextTime=0;

    bool spawnBomb;
    
    private void Start()
    {
        spawnBomb = false;
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnBomb)
        {
            list = GameObject.FindGameObjectsWithTag("cube");

            if (Time.time > nextTime)
            {
                nextTime = Time.time + intervalForSpawn;
                var random = Random.Range(0, list.Length);
                var pos = list[random].transform.position;

                if (list[random].transform.childCount <= 0)
                {
                    transform.position = pos;
                    transform.SetParent(list[random].transform);
                }
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
        spawnBomb = true;
    }

}
