using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    public Vector2 turn;
    public float mouseSensitivity = 5f; 
 


    void Start()
    {
     // lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;   
    }

    void Update()
    {
        MovingGun();
        Fire();
    }

    private void Fire()
    {
       // shooting by klicking f key
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.up * bulletSpeed);
        }
    }

    private void MovingGun()
    {
        //lock up and down rotation of gunand left and right rotation of gun
        turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;

        turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity;

        // accept only values between -90 and 90
        turn.y = Mathf.Clamp(turn.y, -90f, 90f);
       turn.x = Mathf.Clamp(turn.x, -90f, 90f);

       
        transform.localRotation = Quaternion.Euler(turn.y, 0f,turn.x);

        
    }
}
