using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftMovment : MonoBehaviour
{


    public float speed;
    public float acceleration;
    public float currentSpeed;
    public float maxSpeed;  


    public float mouseSensitivity = 10f;

    // private variables
    private float turnSpeed = 30;
    private Vector3 moveDirection;
    
 
    // particle system size
    private Vector3 maxMainEngineSize = new Vector3(2f, 2f, 2f);
    private Vector3 maxSideEngineSize = new Vector3(1f, 1f, 1f);
    private float increaseRate = 1.001f;
    private float decreaseRate = 0.999f;
    private Vector3 startMainEngineSize;
    private Vector3 startSideEngineSize;

    // bool variables
    private bool isForward;
    private bool isUp;
    private bool isAccelerating;


    // reference 
    private Rigidbody rigidBody;
    private LayerMask layerMask;
    ParticleSystem [] particleSystems;  // reference to particle system

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        layerMask = LayerMask.GetMask("Ground");
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        startMainEngineSize = particleSystems[0].transform.localScale; // get the current size of main engine
        startSideEngineSize = particleSystems[1].transform.localScale; // get the current size of side engine
        currentSpeed = speed;   
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        Turn();
        AccelertingParticleSystemManager();
        Rotation();   
    }

    private void FixedUpdate() {
        Move();
        Dive(); 
    }

    private void Move() {

        Debug.Log("Move is called");
        float moveVertical =  Input.GetAxis("Vertical");
        // make w and s key to move forward and backward
       
        // move the space craft
        if ( Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift) )
        {
            if(currentSpeed > speed){
                
                currentSpeed -=acceleration;
            }
            else{
                currentSpeed = speed; 
            }
        }
        else if ( Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) ) {

            if(currentSpeed < maxSpeed){

                currentSpeed += acceleration;
            
            }else{
                currentSpeed = maxSpeed;
            }
        }
        
        if (moveVertical >= 0) {
             moveDirection = transform.forward * moveVertical;
             rigidBody.MovePosition(transform.position + moveDirection * currentSpeed * Time.deltaTime);
        }
        
        // active gravity if space key is pressed and move upper 
        if ( Input.GetKey(KeyCode.Space) )
        { 
            float launchPower = turnSpeed;
            if ( Input.GetKey(KeyCode.LeftShift)) {
                launchPower += acceleration;
            } else {
               // rigidBody.AddForce( );
                if ( launchPower > turnSpeed ) {
                    launchPower -= acceleration;
                } else {
                    launchPower = turnSpeed;
                }
            } 
            rigidBody.AddRelativeForce(new Vector3(0, launchPower, 0));
        }

    }

    private void Turn() {
        // float inputHorizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        // Quaternion turnRotation = Quaternion.Euler(0f, inputHorizontal, 0f);
        // rigidBody.MoveRotation(rigidBody.rotation * turnRotation);

        float inputHorizontal = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime * mouseSensitivity ;
        Quaternion turnRotationZ = Quaternion.Euler(0f, inputHorizontal, 0f);
        rigidBody.MoveRotation(rigidBody.rotation * turnRotationZ);
    }

    
    // rotate using e and q key
    private void Rotation() {
        if ( Input.GetKey(KeyCode.Q) )
        {
            transform.localRotation *= Quaternion.Euler(0, 0, 0.2f);
        }
        if(Input.GetKey(KeyCode.E)){
             
             transform.localRotation *= Quaternion.Euler(0, 0, -0.2f);
        }
    }

     // rotate using e and q key
    private void Dive() {
        // if ( (Input.GetKey(KeyCode.Mouse0) ))
        // {
        //     transform.localRotation *= Quaternion.Euler(0.2f, 0, 0);
        // }
        // if( (Input.GetKey(KeyCode.Mouse1) )){
        //      transform.localRotation *= Quaternion.Euler(-0.2f, 0, 0);
        // }

         // rotate z axis using the mouse
        // lock the mouse cursor
        
        float inputVertical = Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime * mouseSensitivity * -1;
        Quaternion turnRotationZ = Quaternion.Euler(inputVertical, 0f, 0f);
        rigidBody.MoveRotation(rigidBody.rotation * turnRotationZ);

    }
    

    private void AccelertingParticleSystemManager() {

        isAccelerating = Input.GetKey(KeyCode.LeftShift);
        isForward = Input.GetKey(KeyCode.W);
        isUp = Input.GetKey(KeyCode.Space);

        if( (isForward && isAccelerating && !isUp)){

            // decrease the size of side engine
            if (particleSystems[1].transform.localScale.x > startSideEngineSize.x){
                
                particleSystems[1].transform.localScale *= decreaseRate;
                particleSystems[2].transform.localScale *= decreaseRate;
            }
            else{
                particleSystems[1].transform.localScale = startSideEngineSize;
                particleSystems[2].transform.localScale = startSideEngineSize;
            }

            if (particleSystems[0].transform.localScale.x < maxMainEngineSize.x ){
                // increase the size of main engine
                particleSystems[0].transform.localScale *= increaseRate;
            } else{
                // set the size of main engine to max size
                particleSystems[0].transform.localScale = maxMainEngineSize;
            }

        }

        else if( (!isForward && isAccelerating && isUp) ){

            if (particleSystems[0].transform.localScale.x > startMainEngineSize.x){
                // decrease the size of main engine
                particleSystems[0].transform.localScale *= decreaseRate;
            }
            else{
                particleSystems[0].transform.localScale = startMainEngineSize;
            }
            
            if (particleSystems[2].transform.localScale.x < maxSideEngineSize.x) {
                 particleSystems[1].transform.localScale *= increaseRate;
                 particleSystems[2].transform.localScale *= increaseRate;
            } else{
                 particleSystems[1].transform.localScale = maxSideEngineSize;
                 particleSystems[2].transform.localScale = maxSideEngineSize;
            }
        }

        else if( (isUp && isAccelerating && isUp)){

            if ( particleSystems[0].transform.localScale.x < maxMainEngineSize.x ){
                 particleSystems[0].transform.localScale *= increaseRate;
            } else{
                particleSystems[0].transform.localScale = maxMainEngineSize;
            }
            
            if ( particleSystems[2].transform.localScale.x < maxSideEngineSize.x) {
                 particleSystems[1].transform.localScale *= increaseRate;
                 particleSystems[2].transform.localScale *= increaseRate;
            } else{
                 particleSystems[1].transform.localScale = maxSideEngineSize;
                 particleSystems[2].transform.localScale = maxSideEngineSize;
            }
        }
        
        else {

            // decrease the size of main engine
            if (particleSystems[0].transform.localScale.x > startMainEngineSize.x){
                // decrease the size of main engine
                particleSystems[0].transform.localScale *= decreaseRate;
            }
            else{
                particleSystems[0].transform.localScale = startMainEngineSize;
            }
            // decrease the size of side engine
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
