using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform targetToFollow;

    [SerializeField]
    private Vector3 targetOffset;

    [SerializeField]
    private float smoothFactor = 0.25f;

    /// <summary>
    /// LateUpdate is called after all Updates are finished (Update, FixedUpdate, etc)
    /// That way, player position is already computed before we follow
    /// </summary>
    private void LateUpdate()
    {
        // Dont do anything if there is no target to follow
        if (targetToFollow == null)
        {
            return;
        }

        Vector3 targetCameraPosition = targetToFollow.position + targetOffset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position,
            targetCameraPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
