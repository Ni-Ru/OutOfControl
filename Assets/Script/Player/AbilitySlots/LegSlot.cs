using UnityEngine;

public class LegSlot : AbilitySlot
{
    public LegSlot(PlayerAction playerAction) : base(playerAction, AbilitySlotPositions.Legs, KeyCode.Z, KeyCode.Joystick1Button2) { }
}
