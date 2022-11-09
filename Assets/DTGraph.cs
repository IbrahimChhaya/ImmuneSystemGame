/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class DTGraph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;

    public int enemyID;
    public string map;
    public string type;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        MapStats mapStats;
        if (map == "bridge")
            mapStats = SaveLoad.LoadData("Bridge");
        else
            mapStats = SaveLoad.LoadData("Dungeon");

        /*List<float> valueList = new List<float>();
        foreach (EnemyStats es in bridgeStats.enemyStats)
        {
            valueList.Add(es.detectionTime);              
        }*/


        List<float> valueList = new List<float>();

        if(type == "detection")
        { 
            if (enemyID == 1)
                valueList = mapStats.enemyStats[0].detectionTime;//new List<float>() { 5.245f, 98.1114f, 56.0000f, 45.984f, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36 };
            else if (enemyID == 2)
                valueList = mapStats.enemyStats[1].detectionTime;//new List<float>() { 13, 17, 25, 37, 40, 36, 5.245f, 98.1114f, 56.0000f, 45.984f, 30, 22, 17, 15 };
            else if(enemyID == 3)
                valueList = mapStats.enemyStats[2].detectionTime; //new List<float>() { 37, 40, 36, 5.245f, 98.1114f, 13, 17, 30, 22, 17, 25, 56.0000f, 45.984f, 15 };
            else
                valueList = mapStats.enemyStats[3].detectionTime;
        }
        else if(type == "affinity")
        {
            if (enemyID == 1)
                valueList = mapStats.enemyStats[0].affinity;
            else if (enemyID == 2)
                valueList = mapStats.enemyStats[1].affinity;
            else if (enemyID == 3)
                valueList = mapStats.enemyStats[2].affinity;
            else
                valueList = mapStats.enemyStats[3].affinity;
        }
        else if(type == "radius")
        {
            if (enemyID == 1)
                valueList = mapStats.enemyStats[0].fovRadius;
            else if (enemyID == 2)
                valueList = mapStats.enemyStats[1].fovRadius;
            else if (enemyID == 3)
                valueList = mapStats.enemyStats[2].fovRadius;
            else
                valueList = mapStats.enemyStats[3].fovRadius;
        }
        else if (type == "angle")
        {
            if (enemyID == 1)
                valueList = mapStats.enemyStats[0].fovAngle;
            else if (enemyID == 2)
                valueList = mapStats.enemyStats[1].fovAngle;
            else if (enemyID == 3)
                valueList = mapStats.enemyStats[2].fovAngle;
            else
                valueList = mapStats.enemyStats[3].fovAngle;
        }
        else if (type == "speed")
        {
            if (enemyID == 1)
                valueList = mapStats.enemyStats[0].runningSpeeds;
            else if (enemyID == 2)
                valueList = mapStats.enemyStats[1].runningSpeeds;
            else if (enemyID == 3)
                valueList = mapStats.enemyStats[2].runningSpeeds;
            else
                valueList = mapStats.enemyStats[3].runningSpeeds;
        }

        ShowGraph(valueList, -1, (int _i) => "Iteration " + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }

    private void ShowGraph(List<float> valueList, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        int xIndex = 0;

        GameObject lastCircleGameObject = null;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if (lastCircleGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            /*RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
            gameObjectList.Add(dashX.gameObject);*/

            xIndex++;
        }

        int separatorCount = 5;
        for (int i = 0; i < separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            gameObjectList.Add(labelY.gameObject);

            /*RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);*/
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        if (enemyID == 1)
            gameObject.GetComponent<Image>().color = Color.white;
        else if(enemyID == 2)
            gameObject.GetComponent<Image>().color = Color.red;
        else if(enemyID == 3)
            gameObject.GetComponent<Image>().color = new Color32(1, 254, 1, 255);
        else
            gameObject.GetComponent<Image>().color = new Color32(1, 1, 254, 255);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        if(enemyID == 1)
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        else if(enemyID == 2)
            gameObject.GetComponent<Image>().color = new Color32(254, 1, 1, 150);
        else if(enemyID == 3)
            gameObject.GetComponent<Image>().color = new Color32(1, 254, 1, 150);
        else
            gameObject.GetComponent<Image>().color = new Color32(1, 1, 254, 150);


        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        return gameObject;
    }

}
