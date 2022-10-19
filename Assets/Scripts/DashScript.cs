using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashScript : MonoBehaviour
{
    public TextMeshProUGUI dungeonInitial;
    public TextMeshProUGUI[] enemyDTimes;
    public TextMeshProUGUI[] enemyDAffinities;
    public TextMeshProUGUI dungeonIteration;

    public TextMeshProUGUI bridgeInitial;
    public TextMeshProUGUI[] enemyBTimes;
    public TextMeshProUGUI[] enemyBAffinities;
    public TextMeshProUGUI bridgeIteration;
    // Start is called before the first frame update
    void Start()
    {
        var dIteration = PlayerPrefs.GetInt("DungeonIterations");
        var bIteration = PlayerPrefs.GetInt("BridgeIterations");
        dungeonIteration.text = "Number of Iterations: " + dIteration.ToString();
        bridgeIteration.text = "Number of Iterations: " + bIteration.ToString();

        dungeonInitial.text = "Initial time to detection: " + PlayerPrefs.GetFloat("DInitialDetection").ToString("#.00") + " seconds";
        if (dungeonInitial.text == "Initial time to detection: ,00 seconds")
            dungeonInitial.text = "Initial time to detection: Not recorded";
        bridgeInitial.text = "Initial time to detection: " + PlayerPrefs.GetFloat("BInitialDetection").ToString("#.00") + " seconds";
        if (bridgeInitial.text == "Initial time to detection: ,00 seconds")
            bridgeInitial.text = "Initial time to detection: Not recorded";

        List<float> dungeonTimes = EnemyDetectionTimes.dungeonTimes;
        List<float> bridgeTimes = EnemyDetectionTimes.bridgeTimes;


        List<float> dungeonAffinities = EnemyDetectionTimes.dAffinities;
        List<float> bridgeAffinities = EnemyDetectionTimes.bAffinities;

        for (int i = 0; i < enemyDTimes.Length; i++)
        {
            if (dungeonTimes != null && dungeonTimes.Count > 0)
            {
                enemyDTimes[i].text = (dungeonTimes[i] / dIteration).ToString("#.00") + "s";
                if(enemyDTimes[i].text == ",00s")
                {
                    enemyDTimes[i].text = "N/A";
                }
            }
            else
                enemyDTimes[i].text = "Not recorded";

            if(dungeonAffinities != null && dungeonAffinities.Count > 0)
            {
                enemyDAffinities[i].text = Math.Round((decimal)dungeonAffinities[i], 2).ToString() + "%";
            }
            else
                enemyDAffinities[i].text = "Not recorded";

        }

        for (int i = 0; i < enemyBTimes.Length; i++)
        {
            if (bridgeTimes != null && bridgeTimes.Count > 0)
            {
                enemyBTimes[i].text = (bridgeTimes[i] / bIteration).ToString("#.00") + "s";
                if (enemyBTimes[i].text == ",00s")
                {
                    enemyBTimes[i].text = "N/A";
                }
            }
            else
                enemyBTimes[i].text = "Not recorded";

            if (bridgeAffinities != null && bridgeAffinities.Count > 0)
            {
                enemyBAffinities[i].text = Math.Round((decimal)bridgeAffinities[i], 2).ToString() + "%";
            }
            else
                enemyBAffinities[i].text = "Not recorded";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
