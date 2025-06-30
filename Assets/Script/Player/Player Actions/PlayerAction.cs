using UnityEngine;

public abstract class PlayerAction
{
    public int maxEnergyPenalty = 10;
    public abstract void execute(GameObject player);
}
