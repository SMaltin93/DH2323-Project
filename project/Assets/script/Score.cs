using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// AUTHOR: Sam Maltin , Harald Olin 2023 


public class Score : MonoBehaviour
{
     // static variables that can be accessed from anywhere in the code
    public static float score = 0f;  // The score
    public static float amountOfBullets = 0f; // The amount of bullets
    
    // Text UI component to display the score and bullet count
    public Text scoreText;
    
    
    // Update is called once per frame
    void Update()
    {
        // Display the current score and bullet count in the text UI component
        scoreText.text = "Score: " + score.ToString()+ "\n Bullets: " + amountOfBullets.ToString();    
    }
}
