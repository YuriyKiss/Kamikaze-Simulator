using UnityEngine;

public class UpdateCurrentPosition : MonoBehaviour
{
    public void SetAnimatorCharacter(Transform pos)
    {
        Ray ray = new Ray(pos.transform.position, Vector3.down);

        Physics.Raycast(ray, out RaycastHit info, 100f, 1 << 0);

        transform.position = new Vector3(info.point.x, info.point.y - 0.05f, -0.3f);
    }
}
