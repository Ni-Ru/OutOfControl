using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUIButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] string description;
    [SerializeField] AbilitySlotPositions slotPositions = new AbilitySlotPositions();
    [SerializeField] int cost;
    [SerializeField] string keyBindDescription;
    [SerializeField] TextMeshProUGUI descriptionBoxTextUI;

    GameObject lastSelected;


    private void Awake()
    {
        descriptionBoxTextUI = GameObject.Find("DescriptionBoxText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            lastSelected = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null && lastSelected != null)
            EventSystem.current.SetSelectedGameObject(lastSelected);
    }

    public void OnButtonClicked(Button currentButton)
    {

        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();


        foreach (Button btn in allButtons)
        {
            if (btn != currentButton && btn.gameObject.activeInHierarchy && btn.interactable)
            {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                break;
            }
        }


        currentButton.gameObject.SetActive(false);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionBoxTextUI.text = description + "\n" +
                                    "\n" +
                                    "Slot position: " + slotPositions + " Slot" + "\n" +
                                    "\n" +
                                    "Keybind: " + keyBindDescription + "\n" +
                                    "\n" +
                                    "Energy cost: " + cost;
    }

    public void OnSelect(BaseEventData eventData)
    {
        descriptionBoxTextUI.text = description + "\n" +
                                    "\n" +
                                    "Slot position: " + slotPositions + " Slot" + "\n" +
                                    "\n" +
                                    "Keybind: " + keyBindDescription + "\n" +
                                    "\n" +
                                    "Energy cost: " + cost;
    }
}
