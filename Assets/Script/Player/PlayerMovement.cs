using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    bool isGrounded = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void addHorizontalMovement(float speed)
    {
        rigid.linearVelocityX += speed;
    }

    public void addVerticalMovement(float speed)
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.58f, 0.05f), CapsuleDirection2D.Horizontal, 0 , groundLayer);

        if (isGrounded) 
        {
            rigid.linearVelocityY = speed;
        }
        
    }

    public void purgeHorizontalVelocity()
    {
        rigid.linearVelocityX = 0;
    }
    
}
