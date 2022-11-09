using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq.Expressions;
using BarGraph.VittorCloud;

public class DetectionTimeGraph : MonoBehaviour
{
    public List<BarGraphDataSet> exampleDataSet; // public data set for inserting data into the bar graph
    BarGraphGenerator barGraphGenerator;
    public Material materialRed;
    MapStats bridgeStats;
    MapStats dungeonStats;

    private void LoadData()
    {
        bridgeStats = SaveLoad.LoadData("Bridge");
        //dungeonStats = SaveLoad.LoadData("Dungeon");
    }


    void Start()
    {
        LoadData();
        barGraphGenerator = GetComponent<BarGraphGenerator>();



        var listXY = new List<XYBarValues>();
        //x = iterations y = detection time
        /*foreach(EnemyStats es in bridgeStats.enemyStats)
        {
            int count = 1;
            listXY.Add(new XYBarValues(count.ToString(), es.detectionTime[2]));
            count++;
        }*/


        foreach(var bs in bridgeStats.enemyStats[2].detectionTime)
        {
            listXY.Add(new XYBarValues("1", bs));
        }
        /*listXY.Add(new XYBarValues("1", 1.4f));
        listXY.Add(new XYBarValues("2", 3.5f));
        listXY.Add(new XYBarValues("3", 2.4f));
        */

        exampleDataSet.Add(new BarGraphDataSet
        {
            GroupName = "Initial Detection Time for " + bridgeStats.mapName + " Map",
            barColor = Color.red,
            barMaterial = materialRed,
            ListOfBars = listXY
        });

        //if the exampleDataSet list is empty then return.
        if (exampleDataSet.Count == 0)
        {

            Debug.LogError("ExampleDataSet is Empty!");
            return;
        }

        barGraphGenerator.GeneratBarGraph(exampleDataSet);
        transform.localScale = new Vector3(1000, 550, 1000);
        var trans = transform.position;
        transform.position = new Vector3(-20, -30, 90);
    }


    //call when the graph starting animation completed,  for updating the data on run time
    public void StartUpdatingGraph()
    {


        StartCoroutine(CreateDataSet());
    }



    IEnumerator CreateDataSet()
    {
        //  yield return new WaitForSeconds(3.0f);
        while (true)
        {

            GenerateRandomData();

            yield return new WaitForSeconds(2.0f);

        }

    }



    //Generates the random data for the created bars
    void GenerateRandomData()
    {

        int dataSetIndex = UnityEngine.Random.Range(0, exampleDataSet.Count);
        int xyValueIndex = UnityEngine.Random.Range(0, exampleDataSet[dataSetIndex].ListOfBars.Count);
        exampleDataSet[dataSetIndex].ListOfBars[xyValueIndex].YValue = UnityEngine.Random.Range(barGraphGenerator.yMinValue, barGraphGenerator.yMaxValue);
        barGraphGenerator.AddNewDataSet(dataSetIndex, xyValueIndex, exampleDataSet[dataSetIndex].ListOfBars[xyValueIndex].YValue);
    }
}



