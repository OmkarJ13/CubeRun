using UnityEngine;

[System.Serializable]
public class PowerUpData
{
    public int cost;
    public float uptime;
    public float upgradeLevel;

    public PowerUpData(int cost, float uptime, float upgradeLevel)
    {
        this.cost = cost;
        this.uptime = uptime;
        this.upgradeLevel = upgradeLevel;
    }
}
