using UnityEngine;

public class EyeSlot : AbilitySlot
{
    public EyeSlot(PlayerAction playerAction) : base(playerAction, AbilitySlotPositions.Eyes, KeyCode.I, KeyCode.Joystick1Button3) { }
}
