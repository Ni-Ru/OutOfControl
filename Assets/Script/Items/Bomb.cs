using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float bombTimer = 2;
    [SerializeField] GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(bombFuseTimer());
    }

    IEnumerator bombFuseTimer() 
    { 
        yield return new WaitForSeconds(bombTimer);

        explodeBomb();

    }

    private void explodeBomb()
    {
        player.gameObject.GetComponent<PlayerAttacks>().isBombSpawned = false;
        Destroy(gameObject);
        
        Debug.Log("bomb explode");
    }
}
