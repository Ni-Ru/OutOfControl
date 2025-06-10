using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    Vector3 playerPosition;
    public bool isBombSpawned = false;
    [SerializeField]GameObject bomb;
    [SerializeField]GameObject laserSword;

    private void Awake()
    {
        //playerPosition = GetComponent<Transform>();
    }

    private void Update()
    {
        playerPosition = this.transform.position;
    }

    public void SpawnBomb() 
    {
       
        Vector3 tempPos = new Vector3(1,1,0);

        if (isBombSpawned == false)
        {
            //playerPosition = this.transform.position;
            Instantiate(bomb, new Vector3(playerPosition.x + 2, playerPosition.y, playerPosition.z), quaternion.identity);
            isBombSpawned = true;

        }
    }

    public void swingLaserSword()
    {
        Debug.Log("swinged sword");
    }

}
