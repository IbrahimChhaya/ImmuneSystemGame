using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    public Text deathCounterText;
    private int deathCounter = 0;

    public Text winCounterText;
    private int winCounter = 0;

    public Text activeDistance;

    private int iteration = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        deathCounterText.text = "Death Counter: " + deathCounter.ToString();
        winCounterText.text = "Win Counter: " + winCounter.ToString();
        activeDistance.text = "Current Distance Metric: " + PlayerPrefs.GetString("activeDistance");
    }

    public void AddDeath()
    {
        deathCounter++;
        deathCounterText.text = "Death Counter: " + deathCounter.ToString();

        incIteration();
    }

    public void AddWin()
    {
        winCounter++;
        winCounterText.text = "Win Counter: " + winCounter.ToString();

        incIteration();
    }

    private void incIteration()
    {
        iteration++;
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "BridgeScene")
            PlayerPrefs.SetInt("BridgeIterations", iteration);
        else
            PlayerPrefs.SetInt("DungeonIterations", iteration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
