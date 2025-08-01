using UnityEngine;

public class FollowPlayer : MonoBehaviour

{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
