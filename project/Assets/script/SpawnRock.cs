using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRock : MonoBehaviour
{
    public GameObject rockPrefab;

    public float RandomTimeBtwSpawn;
    public float amountOfRocks;

    
    private float tmpTime;
    
    void Start() {

        tmpTime = Random.Range(0f, RandomTimeBtwSpawn);
    }

    void Update() {

        if (tmpTime <= 0f) {
            Spawn();
            tmpTime = Random.Range(0f,RandomTimeBtwSpawn);
        } else {
            tmpTime -= Time.deltaTime;
        }
    }

    private void Spawn() {

        if (amountOfRocks > 0) {
            Instantiate(rockPrefab, transform.position, transform.rotation);
            amountOfRocks--;
        }
    }
}
