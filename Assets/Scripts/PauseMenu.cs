using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("currentIteration", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {

    }

    public void QuitToMenu()
    {
        List<EnemyStats> enemyStats = new List<EnemyStats>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int iteration = PlayerPrefs.GetInt("currentInteration");
        foreach(GameObject e in enemies)
        {
            var fovRadius = e.GetComponent<EnemyController>().fovRadiuses;
            var fovAngle = e.GetComponent<EnemyController>().fovAngles;
            var affinity = e.GetComponent<EnemyController>().affinities;
            var detectionTime = e.GetComponent<EnemyController>().detectionTimes;
            var runningSpeed = e.GetComponent<EnemyController>().runningSpeeds;
            EnemyStats temp = new EnemyStats(fovRadius, fovAngle, detectionTime, affinity, runningSpeed);
            enemyStats.Add(temp);
        }
        string mapName;
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "BridgeScene")
            mapName = "Bridge";
        else
            mapName = "Dungeon";

        MapStats mapStats = new MapStats(mapName, iteration, enemyStats);
        SaveLoad.SaveData(mapStats);

        //Debug.Log("ddwad");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}
