using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static float score = 0f;
    public static float amountOfBullets = 0f;
    
    public Text scoreText;
    // Start is called before the first frame update
    // Update is called once per frame
    
    void Update()
    {
        scoreText.text = "Score: " + score.ToString()+ "\n Bullets: " + amountOfBullets.ToString();    
    }
}
