using UnityEngine;
using UnityEngine.Rendering;

public class SeeInvisibility : PlayerAction
{
    private bool showingInvisible;

    private readonly Volume _globalVolume;
    private readonly VolumeProfile _normalVisionProfile;
    private readonly VolumeProfile _ultravioletVisionProfile;

    public SeeInvisibility(Volume globalVolume, VolumeProfile normalVisionProfile, VolumeProfile ultravioletVisionProfile)
    {
        showingInvisible = false;
        _globalVolume = globalVolume;
        _normalVisionProfile = normalVisionProfile;
        _ultravioletVisionProfile = ultravioletVisionProfile;
    }

    public override void execute(GameObject player)
    {
        showingInvisible = !showingInvisible;
        InvisObjects.instance.setObjectVisibility(showingInvisible);
        _globalVolume.profile = showingInvisible ? _ultravioletVisionProfile : _normalVisionProfile;
    }
}
