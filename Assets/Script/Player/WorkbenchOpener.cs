using UnityEngine;

public class WorkbenchOpener : MonoBehaviour
{
    [SerializeField] GameObject abilityInventoryMenu;
    private bool insideWorkbench;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Workbench"))
        {
            insideWorkbench = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Workbench"))
        {
            insideWorkbench = false;
        }
    }

    void Update()
    {
        if (insideWorkbench && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            if (!abilityInventoryMenu.activeSelf)
            {
                abilityInventoryMenu.SetActive(true);
                Time.timeScale = 0;
                //UpdateBatteryUI();
            }
            else
            {
                abilityInventoryMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
