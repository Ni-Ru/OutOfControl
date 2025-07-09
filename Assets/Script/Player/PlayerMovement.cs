using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Transform playerPosition;

    bool isGrounded = true;
    [SerializeField] Vector2 groundOffset;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask climbingWallLayer;

    bool isTouchingWall = false;
    [SerializeField] float wallCollisionRadius;
    [SerializeField] float groundCollisionRadius;
    [SerializeField] Vector2 rightOffset;
    [SerializeField] Vector2 leftOffset;
    [SerializeField] private Animator animator;

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

        isTouchingWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, wallCollisionRadius, climbingWallLayer) ||
                         Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, wallCollisionRadius, climbingWallLayer);
        if(Mathf.Abs(rigid.linearVelocityX) > 0)
        {
            animator.enabled = true;
            animator.SetBool("isWalking", true);
        }else animator.SetBool("isWalking", false);
        if (rigid.linearVelocityX < 0) transform.rotation = new Quaternion(0, 180, 0, 0);
        if (rigid.linearVelocityX > 0) transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundOffset, groundCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, wallCollisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, wallCollisionRadius);
    }

}
