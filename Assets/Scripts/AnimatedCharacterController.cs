using UnityEngine;

public class AnimatedCharacterController : MonoBehaviour
{
    private Vector3 direction = Vector3.down;

    private float distance = 100f;
    private int layerMask = 1 << 0;
    private float heightOffset = 0.05f;

    public void SetPosition(Transform pos)
    {
        Ray ray = new Ray(pos.transform.position, direction);

        Physics.Raycast(ray, out RaycastHit info, distance, layerMask);

        transform.position = new Vector3(info.point.x, info.point.y - heightOffset, 0f);
    }
}
