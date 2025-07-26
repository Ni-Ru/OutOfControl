using UnityEngine;

public class Workbench : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,240,0,255);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
