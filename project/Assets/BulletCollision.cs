using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;
    public GameObject rockPiecesPrefab;
    
    private void OnCollisionEnter(Collision collision)
    {
  
        if(collision.transform.name =="Rock_Overgrown_D Variant(Clone)"){

            GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
            Destroy(gameObject);
            Destroy(collision.gameObject);
            GameObject rockPieces = (GameObject)Instantiate(rockPiecesPrefab, collision.gameObject.transform.position, rockPiecesPrefab.transform.rotation);
            var rockPiecesRigidBodies = rockPieces.GetComponentsInChildren<Rigidbody>();
            foreach (var rockPiecesRigidBody in rockPiecesRigidBodies)
            {
                rockPiecesRigidBody.AddExplosionForce(1000f, collision.gameObject.transform.position, 100f);
            }
            Destroy(rockPieces, 5f);
        }
        else{
            // dont collide with the aircraft
            if(collision.transform.name !="Aircraft"){ 
             GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
             Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
             Destroy(gameObject);
            }
            
           
        }
    }
}
