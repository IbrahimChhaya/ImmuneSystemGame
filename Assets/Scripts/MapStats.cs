using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapStats
{

    public string mapName;
    public int iteration;
    public List<EnemyStats> enemyStats;

    public MapStats(string mapName, int iteration, List<EnemyStats> enemyStats)
    {
        this.mapName = mapName;
        this.iteration = iteration;
        this.enemyStats = enemyStats;
    }
}
