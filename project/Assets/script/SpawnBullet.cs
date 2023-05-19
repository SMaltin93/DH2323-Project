using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AUTHOR: Sam Maltin , Harald Olin 2023 

public class SpawnBullet : MonoBehaviour
{

   // prefab for the bullet
   public GameObject bulletPrefab;
    // speed of the bullet
   public float bulletSpeed;
    // Update is called once per frame    
    void Update()
    {
         // Check if the 'F' key has been pressed to fire a bullet
        if ( Input.GetKeyDown(KeyCode.F) ) {
            Fire();
            // Increase the count of bullets fired
            Score.amountOfBullets += 1f;
        }

    }
    private void Fire() {

       // Instantiate a bullet at the current object's position and rotation
        GameObject bullet =  Instantiate(bulletPrefab, transform.position, transform.rotation);
        // Set the bullet's velocity in the forward direction of the current object
        // This means the bullet will move forward from the point of view of the object that fired it
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * (bulletSpeed);
        // Destroy the bullet after 5 seconds. That avoid infinite accumulation of bullets in the scene. 
        Destroy(bullet, 5f);
    }
}
