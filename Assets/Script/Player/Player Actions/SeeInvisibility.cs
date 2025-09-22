using UnityEngine;
using UnityEngine.Rendering;

public class SeeInvisibility : PlayerAction
{
    private static readonly string volumePath = "VolumeProfiles/";
    private static readonly string normalVolumeName = "RetroVolumeProfile";
    private static readonly string uVVolumeName = "UVVolumeProfile";

    private bool showingInvisible;

    private readonly Volume _globalVolume;
    private readonly VolumeProfile _normalVisionProfile;
    private readonly VolumeProfile _ultravioletVisionProfile;
    new public int maxEnergyPenalty = 20;

    public SeeInvisibility()
    {
        showingInvisible = false;
        _globalVolume = GameObject.FindFirstObjectByType<Volume>();
        _normalVisionProfile = (VolumeProfile) Resources.Load(volumePath + normalVolumeName);//normalVisionProfile;
        _ultravioletVisionProfile = (VolumeProfile)Resources.Load(volumePath + uVVolumeName);
    }

    public override void execute(GameObject player)
    {
        showingInvisible = !showingInvisible;
        InvisObjects.instance.setObjectVisibility(showingInvisible);
        _globalVolume.profile = showingInvisible ? _ultravioletVisionProfile : _normalVisionProfile;
    }
}
