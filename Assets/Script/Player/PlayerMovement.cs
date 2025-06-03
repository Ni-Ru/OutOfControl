using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void addHorizontalMovement(float speed)
    {
        rigid.linearVelocityX += speed;
    }

    public void purgeHorizontalVelocity()
    {
        rigid.linearVelocityX = 0;
    }
}
