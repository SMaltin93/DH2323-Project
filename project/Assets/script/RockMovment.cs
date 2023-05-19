using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AUTHOR: Sam Maltin , Harald Olin 2023 


public class RockMovment : MonoBehaviour
{
   // variables for rock movment
   public float rockSpeed = 10f;  // Rock's speed in the Y direction
   public float anglePerSecond; // rotates around the spawn point's Y-axis per second
   public float radiusSpeed;  // Rock's speed in the X and Z direction
   public Transform spawnPoint;  // The spawn point of the rock
    

   // private variables
   private Vector3 moveDirection;  // The direction of the rock's movement
   private Rigidbody rigidBody;  // Rigidbody component attached to the rock
   private float maxHeight; // Maximum height the rock can reach
   private float maxRadius; // Maximum radius from the spawn point the rock can reach
   private float radius; // Current radius from the spawn point 



   // Initialize variables on start
   void Start () {

      rigidBody = GetComponent<Rigidbody>(); // Get the Rigidbody component
      maxHeight = 200; // Set the maximum height
      maxRadius = 200; // Set the maximum radius 
   }

   // Update is called once per frame
   void Update () {
      Movement(); 
   }
   

   // Movement works by: first 
   // 1: translating the rock upwards
   // 2: rotating the rock around the spawnpoints y-axis
   // 3: translating the rock forwards
   // These three steps allow the rocks to fly in a upwards spiral

   // Method that performs the movement
   private void Movement() {
    

      moveDirection = new Vector3(0f, 1f, 0f); // Set the move direction to upwards

      // If the rock hasn't reached its maximum height yet
      if(transform.position.y < maxHeight){
         // Move the rock upwards
         transform.Translate(Vector3.up * rockSpeed * Time.deltaTime);
      }
      
      // rotate around the spawn point's y axis
      transform.RotateAround(spawnPoint.transform.position, Vector3.up, anglePerSecond * Time.deltaTime);

      // translates the rock forward. Calculate the current radius
      radius = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.z, 2));
      
      // If the rock hasn't reached its maximum radius yet
      if(radius< maxRadius){
          // Move the rock forward
         transform.Translate(Vector3.forward * Time.deltaTime*radiusSpeed);
      }
        
   }



   

 
}
