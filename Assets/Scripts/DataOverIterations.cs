using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq.Expressions;
using BarGraph.VittorCloud;

public class DataOverIterations : MonoBehaviour
{
    public List<BarGraphDataSet> exampleDataSet; // public data set for inserting data into the bar graph
    BarGraphGenerator barGraphGenerator;
    MapStats mapStats;

    public Material green;
    public Material pink;
    public Material purple;
    public Material yellow;
    public Material blue;

    void Start()
    {
        barGraphGenerator = GetComponent<BarGraphGenerator>();

        LoadData();
        List<XYBarValues> listDT = new List<XYBarValues>();
        List<XYBarValues> listAF = new List<XYBarValues>();
        List<XYBarValues> listfovA = new List<XYBarValues>();
        List<XYBarValues> listfovR = new List<XYBarValues>();
        List<XYBarValues> listRS = new List<XYBarValues>();

        foreach (EnemyStats es in mapStats.enemyStats)
        {
            foreach(float dt in es.detectionTime)
            {
                listDT.Add(new XYBarValues("", dt));
            }

            foreach (float af in es.affinity)
            {
                listAF.Add(new XYBarValues("", af));
            }

            foreach (float fovA in es.fovAngle)
            {
                listfovA.Add(new XYBarValues("", fovA));
            }

            foreach (float fovR in es.fovRadius)
            {
                listfovR.Add(new XYBarValues("", fovR));
            }

            foreach (float rs in es.runningSpeeds)
            {
                listRS.Add(new XYBarValues("", rs));
            }
        }

        exampleDataSet.Add(new BarGraphDataSet
        {
            barColor = Color.green,
            barMaterial = green,
            GroupName = "",
            ListOfBars = listDT
        });

        exampleDataSet.Add(new BarGraphDataSet
        {
            barColor = Color.yellow,
            barMaterial = yellow,
            GroupName = "",
            ListOfBars = listAF
        });

        exampleDataSet.Add(new BarGraphDataSet
        {
            barColor = Color.red,
            barMaterial = pink,
            GroupName = "",
            ListOfBars = listfovA
        });

        exampleDataSet.Add(new BarGraphDataSet
        {
            barColor = Color.blue,
            barMaterial = blue,
            GroupName = "",
            ListOfBars = listfovR
        });

        exampleDataSet.Add(new BarGraphDataSet
        {
            barColor = Color.cyan,
            barMaterial = purple,
            GroupName = "",
            ListOfBars = listRS
        });

        //if the exampleDataSet list is empty then return.
        if (exampleDataSet.Count == 0)
        {

            Debug.LogError("ExampleDataSet is Empty!");
            return;
        }

        barGraphGenerator.GeneratBarGraph(exampleDataSet);
    }

    void LoadData()
    {
        mapStats = SaveLoad.LoadData("Bridge");
    }
}



