using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    public Text deathCounterText;

    private int deathCounter = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        deathCounterText.text = "Death Counter: " + deathCounter.ToString();
    }

    public void AddDeath()
    {
        deathCounter++;
        deathCounterText.text = "Death Counter: " + deathCounter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
