using UnityEngine;

public abstract class PlayerAction
{
    public int maxEnergyPenalty = 10;
    public abstract void execute(GameObject player);

    public enum AbilityName
    {
        Void,
        Jump,
        Climb,
        SeeInvis
    }

    public static PlayerAction parseAbilityName(AbilityName abilityName)
    {
        return abilityName switch
        {
            AbilityName.Jump => new Jump(),
            AbilityName.Climb => new ClimbUp(),
            AbilityName.SeeInvis => new SeeInvisibility(),
            _ => null
        };
    }
}
