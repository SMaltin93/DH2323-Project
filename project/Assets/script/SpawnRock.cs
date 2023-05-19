using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AUTHOR: Sam Maltin , Harald Olin 2023 

public class SpawnRock : MonoBehaviour
{

    // prefab for the rock
    public GameObject rockPrefab;

    public float RandomTimeBtwSpawn; // Random time between each rock spawn
    public float amountOfRocks; // Amount of rocks to spawn

    
    private float tmpTime; // Temporary time variable
    
    // Start is called before the first frame update
    void Start() {
        // Initialize the timer with a random value between 0 and the specified maximum random time between spawns
        tmpTime = Random.Range(0f, RandomTimeBtwSpawn);
    }


    // Update once per frame
    void Update() {
        // // Spawn a rock if the timer has reached 0
        if (tmpTime <= 0f) {
            Spawn();
            // Reset the timer with a new random value to start counting down from
            tmpTime = Random.Range(0f,RandomTimeBtwSpawn);
        } else {
            // decrement it by the time that has passed since the last frame
            tmpTime -= Time.deltaTime;
        }
    }


    // This method spawns a rock
    private void Spawn() {
        // If there are still rocks to spawn
        if (amountOfRocks > 0) {
            // Spawn a rock at the position and with the rotation of this GameObject
            Instantiate(rockPrefab, transform.position, transform.rotation);
            // Decrease the amount of rocks left to spawn
            amountOfRocks--;
        }
    }
}
