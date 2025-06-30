using UnityEngine;

public class BatteryChargeDisplay : MonoBehaviour
{
    private PlayerControls player;
    private RectTransform rect;
    private float maxHeight;
    private void Awake()
    {
        player = FindFirstObjectByType<PlayerControls>();
        rect = GetComponent<RectTransform>();
        maxHeight = rect.rect.height;
    }

    private void Update()
    {
        rect.sizeDelta = new Vector2(rect.rect.width, maxHeight * player.currentEnergy / PlayerControls.maxEnergyLimit);
    }
}
