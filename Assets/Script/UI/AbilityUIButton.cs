using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityUIButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] string description;
    [SerializeField] AbilitySlotPositions slotPositions = new AbilitySlotPositions();
    [SerializeField] int cost;
    [SerializeField] string keyBindDescription;
    [SerializeField] TextMeshProUGUI descriptionBoxTextUI;

    private void Awake()
    {
        descriptionBoxTextUI = GameObject.Find("DescriptionBoxText").GetComponent<TextMeshProUGUI>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionBoxTextUI.text = description + "\n" +
                                    "\n" +
                                    "Slot position: " + slotPositions + " Slot" +"\n" +
                                    "\n" +
                                    "Keybind: " + keyBindDescription + "\n" +
                                    "\n" +
                                    "Energy cost: " + cost;
    }

}
