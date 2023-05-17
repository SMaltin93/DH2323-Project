using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using sleep = System.Threading.Thread.Sleep;
using System.Threading;

public class RockMovment : MonoBehaviour
{
    // variables for rock movment
   public float rockSpeed = 10f;
   public float anglePerSecond;
   public float radiusSpeed;
   public Transform spawnPoint;
   

   //references 
  

   // private variables

   private Vector3 moveDirection;
   private Rigidbody rigidBody;
   private float maxHeight;
   private float maxRadius;
   private float radius;

   void Start () {

      rigidBody = GetComponent<Rigidbody>();
      maxHeight = 350;
      maxRadius = 200;
   }


   void Update () {
      Movement();
      
   }
   

   // Movement works by: first 
   // 1: translating the rock upwards
   // 2: rotating the rock around the spawnpoints y-axis
   // 3: translating the rock forwards
   // These three steps allow the rocks to fly in a upwards spiral
   private void Movement() {
    

      moveDirection = new Vector3(0f, 1f, 0f);

      // Translates the rock in y-direction
      if(transform.position.y < maxHeight){
         transform.Translate(Vector3.up * rockSpeed * Time.deltaTime);
      }
      
      // rotate around the spawn point's y axis
      transform.RotateAround(spawnPoint.transform.position, Vector3.up, anglePerSecond * Time.deltaTime);

      // translates the rock forward.
      radius = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2));
      if(radius< maxRadius){
         transform.Translate(Vector3.forward * Time.deltaTime*radiusSpeed);
      }
        
   }



   

 
}
