using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftMovment : MonoBehaviour
{


    public float speed;
    public float acceleration;

    // private variables
    private float maxSpeed = 200f;
    private float turnSpeed = 20;
    private Vector3 moveDirection;
    private float currentSpeed;

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

        
    }

    private void Update()
    {
        Move();
        Turn();
        AccelertingParticleSystemManager();
    }

    private void Move() {

        Debug.Log("Move is called");
        float moveVertical = Input.GetAxis("Vertical");
        // make w and s key to move forward and backward
        moveDirection = transform.forward * moveVertical;
       

        // move the space craft
        if ( Input.GetKey(KeyCode.W) )
        {
            rigidBody.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);

        }
        if ( Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) ) {

            if(currentSpeed < maxSpeed){

                currentSpeed += acceleration;
            
            }else{
                currentSpeed = maxSpeed;
            }

            rigidBody.MovePosition(transform.position + moveDirection * currentSpeed * Time.deltaTime);

        }

        // active gravity if space key is pressed and move upper 
        if ( Input.GetKey(KeyCode.Space) )
        {  
            rigidBody.AddForce(Vector3.up * 1000f * Time.deltaTime);

        }

    }

    private void Turn() {

        float inputHorizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, inputHorizontal, 0f);
        rigidBody.MoveRotation(rigidBody.rotation * turnRotation);

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
