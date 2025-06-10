using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Transform playerPosition;

    bool isGrounded = true;
    [SerializeField] Vector2 groundOffset;

    [SerializeField] LayerMask groundLayer;

    bool isTouchingWall = false;
    [SerializeField] float wallCollisionRadius;
    [SerializeField] float groundCollisionRadius;
    [SerializeField] Vector2 rightOffset;
    [SerializeField] Vector2 leftOffset;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerPosition = GetComponent<Transform>();

    }

    public void addHorizontalMovement(float speed)
    {
        rigid.linearVelocityX += speed;
    }

    public void addJump(float speed)
    {
        if (isGrounded) 
        {
            rigid.linearVelocityY = speed;
        }
        
    }

    public void addClimbingUp(float speed)
    {
        if (isTouchingWall) 
        {
            //playerPosition.transform.position = playerPosition.transform.position + new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
            rigid.linearVelocityY = speed;
        }
    }

    public void purgeHorizontalVelocity()
    {
        rigid.linearVelocityX = 0;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + groundOffset, groundCollisionRadius, groundLayer);

        isTouchingWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, wallCollisionRadius, groundLayer) ||
                         Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, wallCollisionRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundOffset, groundCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, wallCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, wallCollisionRadius);
    }

}
