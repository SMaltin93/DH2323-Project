using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AUTHOR: Sam Maltin , Harald Olin 2023 

public class SpaceCraftMovment : MonoBehaviour
{


    public float speed; // speed of the space craft
    public float acceleration; // acceleration of the space craft
    public float currentSpeed; // current speed of the space craft
    public float maxSpeed;  // maximum speed of the space craft


    public float mouseSensitivity = 10f; // mouse sensitivity

    // private variables
    private float turnSpeed = 20; // turn speed of the space craft
    private Vector3 moveDirection; // move direction of the space craft
    
 
    // particle system size
    private Vector3 maxMainEngineSize = new Vector3(2f, 2f, 2f); // max size of the main engine
    private Vector3 maxSideEngineSize = new Vector3(1f, 1f, 1f); // max size of the side engine
    private float increaseRate = 1.01f; // increase rate of the particle system
    private float decreaseRate = 0.999f; // decrease rate of the particle system
    private Vector3 startMainEngineSize; // start size of the main engine
    private Vector3 startSideEngineSize; // start size of the side engine


    // bool variables
    private bool isForward; // to check if the space craft is moving forward
    private bool isUp;  // to check if the space craft is moving up
    private bool isAccelerating; // to check if the space craft is accelerating
    private bool hasLanded = false; // to check if the space craft has landed
    private Vector3 StartPosition; // start position of the space craft

    // reference 

    private Rigidbody rigidBody;  // reference to rigid body
    private LayerMask layerMask; // reference to layer mask
    ParticleSystem [] particleSystems;  // reference to particle system


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();  // get the rigid body component
        layerMask = LayerMask.GetMask("Ground"); // get the layer mask
        particleSystems = GetComponentsInChildren<ParticleSystem>(); // get the particle system component
        startMainEngineSize = particleSystems[0].transform.localScale; // get the current size of main engine
        startSideEngineSize = particleSystems[1].transform.localScale; // get the current size of side engine
        currentSpeed = speed;   // set the current speed to speed
        Cursor.lockState = CursorLockMode.Locked;  // lock the cursor to the center of the screen
 
        // freeze the rotation of the space craft 
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        // get the start position of the space craft to reset the position if the user press the 'R' key
        StartPosition = transform.position;
        
    }


    private void Update()
    {
        Turn();  // to allow for turning of spaceship.
        AccelertingParticleSystemManager();  // Manage the particle systems for engines.
        Rotation();  // Allow for spaceship rotation.
        Landing(); // Handle landing mechanics.
        ResetPosition(); // Reset spaceship position when necessary.

    }

    // Physics updates in FixedUpdate.
    private void FixedUpdate() {
        Move(); // Handle spaceship movement.
        Dive(); // Handle spaceship diving.
    }
    

    private void OnCollisionEnter(Collision collision)
    {
         Rigidbody rigidBody = GetComponent<Rigidbody>(); // get the rigid body component
         // If the spacecraft hasn't landed yet and it collided with an object named "Ground"
        if (!hasLanded && collision.gameObject.name == "Ground")
        {
            // Indicate that the spaceship has landed
            hasLanded = true;
             // Unfreeze the position and rotation of the Rigidbody, allowing it to move and rotate freely.
            rigidBody.constraints = RigidbodyConstraints.None;
            // set gravity  to make it stay on the ground
            rigidBody.useGravity = true;
        }
        // If clicking some key w or space set gravity to false
        if (hasLanded && Input.GetKey(KeyCode.W) || hasLanded && Input.GetKey(KeyCode.Space))
        {   
            // Disable gravity for the Rigidbody, this will allow the spacecraft to lift off
            rigidBody.useGravity = false;
        }
       
    }
    
     // This method checks if the spacecraft is still in the air and tries to land it at the start of the game.
    private void Landing() {
         // If the spacecraft hasn't landed yet
        if(!hasLanded){
            // Move the spacecraft downward 
            transform.position += new Vector3(0, -1 * Time.deltaTime, 0);
            // stabilize the spacecraft smoothly https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html 
            // interpolate between two rotations. current rotation of (transform.rotation) to the target rotation around the Y-axis, often used to turn left and right)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * 2f);

        }
    }

    private void Move() {

        // get the input from the keyboard it will be between -1 and 1
        float moveVertical =  Input.GetAxis("Vertical");

        float lowEnginePower = 0.3f; // low engine power
        float maxEnginePower = 0.3f; // max engine power
        
        // make w to move forward and backward
        // If the 'W' key is pressed (without 'LeftShift'), the spacecraft decelerates
        if ( Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift) )
        {
            // Reduce speed with the acceleration value until it reaches the speed value
            if(currentSpeed > speed){
                currentSpeed -= acceleration;
            }
            else{
                currentSpeed = speed; 
            }
        } 
        // If the 'W' key is pressed (with 'LeftShift'), the spacecraft accelerates
        else if ( Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) ) {

            // Increase speed with the acceleration value until it reaches the maxSpeed value
            if(currentSpeed < maxSpeed){

                currentSpeed += acceleration ;
            }else{
                currentSpeed = maxSpeed;
            }
        }
        // If vertical movement is upwards or neutral, move the spacecraft in that direction
        if (moveVertical >= 0) {
             moveDirection = transform.forward * moveVertical;
             rigidBody.MovePosition(transform.position + moveDirection * currentSpeed * Time.deltaTime);
        }
        
         // move upwards if 'Space' key is pressed
        if ( Input.GetKey(KeyCode.Space) )
        { 
            float launchPower = turnSpeed;
            float lowLaunchPower = 10f;
            float maxLaunchPower = 1f;

             // If 'LeftShift' is pressed, increase the launch power
            if ( Input.GetKey(KeyCode.LeftShift)) {
                launchPower += acceleration;
            } else {
               
                if ( launchPower > turnSpeed ) {
                    launchPower -= (acceleration + lowLaunchPower);
                } else {
                    launchPower = turnSpeed;
                }
                maxLaunchPower = lowLaunchPower;
            } 
            // Add force to the rigid body
            rigidBody.AddRelativeForce(new Vector3(0, launchPower/maxLaunchPower, 0));
        }

    }

    // Method to control the turning of the spacecraft

    private void Turn() {
        // Capture horizontal mouse input
        float inputHorizontal = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime * mouseSensitivity ;
        // Determine new rotation
        Quaternion turnRotationY = Quaternion.Euler(0f, inputHorizontal, 0f);
         // Apply the rotation to the spacecraft
        rigidBody.MoveRotation(rigidBody.rotation * turnRotationY);
    }

    
    // Method to control the rotation of the spacecraft using 'A' and 'D' keys
    private void Rotation() {
        // If 'A' key is pressed, rotate the spacecraft to the left
        if ( Input.GetKey(KeyCode.A) )
        {
            transform.localRotation *= Quaternion.Euler(0, 0, 0.2f);
        }
        // If 'D' key is pressed, rotate the spacecraft to the right
        if(Input.GetKey(KeyCode.D)){
             
             transform.localRotation *= Quaternion.Euler(0, 0, -0.2f);
        }
    }


    // reset the position of the spacecraft when 'R' key is pressed
    void ResetPosition() {
        if ( Input.GetKey(KeyCode.R) ) {
            // Reset the position of the spacecraft, same as the start position
            transform.position =  StartPosition;
        }

    }

    // to control diving of the spacecraft using mouse Y axis
    private void Dive() {
         // Get vertical input from mouse and multiply by turnSpeed, deltaTime, mouseSensitivity, and -1
        float inputVertical = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime * mouseSensitivity * -1;
        // Determine new rotation
        Quaternion turnRotationX = Quaternion.Euler(inputVertical, 0f, 0f);
        // Apply the rotation to the spacecraft
        rigidBody.MoveRotation(rigidBody.rotation * turnRotationX);

    }
    

    // manage the Particle System during acceleration
    private void AccelertingParticleSystemManager() {


        // get the input from the keyboard
        // Check if user is holding LeftShift (accelerating), W (moving forward), and Space (moving up)
        isAccelerating = Input.GetKey(KeyCode.LeftShift);
        isForward = Input.GetKey(KeyCode.W);
        isUp = Input.GetKey(KeyCode.Space);

       // If moving forward and accelerating, but not moving upwards
        if( (isForward && isAccelerating && !isUp)){

            // If side engines are bigger than their starting size, decrease their size
            if (particleSystems[1].transform.localScale.x > startSideEngineSize.x){
                particleSystems[1].transform.localScale *= decreaseRate;
                particleSystems[2].transform.localScale *= decreaseRate;
            }
            else{
                // If side engines are not bigger than their starting size, set their size to starting size
                particleSystems[1].transform.localScale = startSideEngineSize;
                particleSystems[2].transform.localScale = startSideEngineSize;
            }

            // If main engine is smaller than its maximum size, increase its size
            if (particleSystems[0].transform.localScale.x < maxMainEngineSize.x ){
                // increase the size of main engine
                particleSystems[0].transform.localScale *= increaseRate;
            } else{
                // If main engine is not smaller than its maximum size, set its size to maximum size
                particleSystems[0].transform.localScale = maxMainEngineSize;
            }
        }
        // If not moving forward but accelerating and moving upwards
        else if( (!isForward && isAccelerating && isUp) ){
                // If main engine is bigger than its starting size, decrease its size
            if (particleSystems[0].transform.localScale.x > startMainEngineSize.x){
                particleSystems[0].transform.localScale *= decreaseRate;
            }
            else{
                // If main engine is not bigger than its starting size, set its size to starting size
                particleSystems[0].transform.localScale = startMainEngineSize;
            }
            
                 // If side engines are smaller than their maximum size, increase their size
            if (particleSystems[2].transform.localScale.x < maxSideEngineSize.x) {
                 particleSystems[1].transform.localScale *= increaseRate;
                 particleSystems[2].transform.localScale *= increaseRate;
            } 
            else{
                // If side engines are not smaller than their maximum size, set their size to maximum size
                 particleSystems[1].transform.localScale = maxSideEngineSize;
                 particleSystems[2].transform.localScale = maxSideEngineSize;
            }
        }
        // If moving forward, accelerating, and moving upwards
        else if( (isUp && isAccelerating && isUp)){
            // increase the size of main engine and side engines until they reach their maximum size
            if ( particleSystems[0].transform.localScale.x < maxMainEngineSize.x ){
                 particleSystems[0].transform.localScale *= increaseRate;
            } 
            else{
                particleSystems[0].transform.localScale = maxMainEngineSize;
            }

            // increase the size of side engines until they reach their maximum size
            if ( particleSystems[2].transform.localScale.x < maxSideEngineSize.x) {
                 particleSystems[1].transform.localScale *= increaseRate;
                 particleSystems[2].transform.localScale *= increaseRate;
            } else{
                 particleSystems[1].transform.localScale = maxSideEngineSize;
                 particleSystems[2].transform.localScale = maxSideEngineSize;
            }
        }
        
        // If not moving forward, accelerating, and moving upwards
        else {
            // decrease the size of main engine until it reaches its starting size
            if (particleSystems[0].transform.localScale.x > startMainEngineSize.x){
                particleSystems[0].transform.localScale *= decreaseRate;
            }
            else{
                particleSystems[0].transform.localScale = startMainEngineSize;
            }
            
            // decrease the size of side engines until they reach their starting size
            if (particleSystems[1].transform.localScale.x > startSideEngineSize.x){
                particleSystems[1].transform.localScale *= decreaseRate;
                particleSystems[2].transform.localScale *= decreaseRate;
            }
            else{
                particleSystems[1].transform.localScale = startSideEngineSize;
                particleSystems[2].transform.localScale = startSideEngineSize;
            }
        }  
    }
}
