 
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Offsets")]
    public float sideOffset = 0.8f;
    public float heightOffset = 2f;
    public float backOffset = 4f;

    public float followSpeed = 10f;

    void LateUpdate()
    {
        if (!target) return;

        // Follow position
        Vector3 desiredPosition =
            target.position +
            Vector3.up * heightOffset -
            target.forward * backOffset +
            target.right * sideOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // Look in SAME direction as player (no feedback loop)
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, target.eulerAngles.y, 0f),
            followSpeed * Time.deltaTime
        );
    }
}

