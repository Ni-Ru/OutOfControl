using UnityEngine;

public class WalkRight : AbilitySlot
{
    public WalkRight() : base(new Walk(true), AbilitySlotPositions.Right, KeyCode.RightArrow, KeyCode.None) { }
}
