using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AUTHOR: Sam Maltin , Harald Olin 2023 

public class BulletCollision : MonoBehaviour
{

    // prefab for the explosion particles when the bullet hits the rock
    public GameObject explosionParticlesPrefab;

    // prefab for the rock pieces when the rock is destroyed
    public GameObject rockPiecesPrefab;
    
    private void OnCollisionEnter(Collision collision)

    {
        // check if the bullet collides with the rock
        if(collision.transform.name =="Rock_Overgrown_D Variant(Clone)"){
            
            // increase the score by 1
            Score.score += 1f;
           // create an explosion at the position of the collision
            GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
            // destroy the explosion after the lifetime of the particle system
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);

            // destroy the bullet and the collided rock object.
            Destroy(gameObject);
            Destroy(collision.gameObject);

            // create rock pieces at the position of the destroyed rock
            GameObject rockPieces = (GameObject)Instantiate(rockPiecesPrefab, collision.gameObject.transform.position, rockPiecesPrefab.transform.rotation);
            // get all the rigidbodies in the children of the rock pieces
            var rockPiecesRigidBodies = rockPieces.GetComponentsInChildren<Rigidbody>();
            foreach (var rockPiecesRigidBody in rockPiecesRigidBodies)
            {
                // add explosion force to the rock pieces that means they will fly away from the explosion
                rockPiecesRigidBody.AddExplosionForce(1000f, collision.gameObject.transform.position, 100f); 
            }
             // destroy the rock pieces after 5 seconds
            Destroy(rockPieces, 5f);
        }

        else{
             // If the object is not a rock and not the "Aircraft"  explode and destroy the bullet
            if(collision.transform.name !="Aircraft"){ 
             GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
             Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
             Destroy(gameObject);
            }
            
        }
    }
}
