using UnityEngine;

public class SeeInvisibility : PlayerAction
{
    private bool showingInvisible;

    public SeeInvisibility()
    {
        showingInvisible = false;
    }

    public override void execute(GameObject player)
    {
        showingInvisible = !showingInvisible;
        InvisObjects.instance.setObjectVisibility(showingInvisible);
        //Change filters here
    }
}
