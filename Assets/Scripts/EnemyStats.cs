using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStats
{
    public List<float> fovRadius;
    public List<float> fovAngle;
    public List<float> detectionTime;
    public List<float> affinity;
    public List<float> runningSpeeds;

    public EnemyStats(List<float> fovRadius, List<float> fovAngle, List<float> detectionTime, List<float> affinity, List<float> runningSpeeds)
    {
        this.fovRadius = fovRadius;
        this.fovAngle = fovAngle;
        this.detectionTime = detectionTime;
        this.affinity = affinity;
        this.runningSpeeds = runningSpeeds;
    }
}