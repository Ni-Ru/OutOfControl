using UnityEngine;

public class SpawnBomb : PlayerAction
{
    private PlayerAttacks attack;

    public override void execute(GameObject player)
    {
        if (attack == null) attack = player.GetComponent<PlayerAttacks>();

        attack.SpawnBomb();
    }
}
