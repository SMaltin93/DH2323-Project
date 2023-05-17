using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;
    
    private void OnCollisionEnter(Collision collision)
    {
  
        if(collision.transform.name =="Rock_Overgrown_D Variant(Clone)"){

            GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else{
            GameObject explosion = (GameObject)Instantiate(explosionParticlesPrefab, transform.position, explosionParticlesPrefab.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
            Destroy(gameObject);
        }
    }
}
