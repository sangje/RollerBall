using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The target object to follow
    public Vector3 offset = new Vector3(0f, 2f, -5f);  // Offset from the target's center
    private Quaternion initialRotation;  // Store the initial rotation value
    public Vector3 rotationOffset = new Vector3(30f, 0f, 0f); // Rotation offset from the initial rotation

    void Start()
    {
        if (target != null)
        {
            // Store the initial rotation value
            initialRotation = Quaternion.LookRotation(target.position - transform.position);
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Set the camera position to the target's position with the offset
            transform.position = target.position + offset;

            // Apply the rotation offset to the initial rotation
            Quaternion modifiedRotation = initialRotation * Quaternion.Euler(rotationOffset);

            // Maintain the modified rotation
            transform.rotation = modifiedRotation;
        }
    }
}
