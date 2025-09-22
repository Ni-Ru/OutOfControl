using UnityEngine;

public class WalkLeft : AbilitySlot
{
    public WalkLeft() : base(new Walk(false), AbilitySlotPositions.Left, KeyCode.LeftArrow, KeyCode.None) { }
}
