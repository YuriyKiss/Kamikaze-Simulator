using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private Vector3 direction = Vector3.down;

    private float distance = 100f;
    private int layerMask = 1 << 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPosition(Transform pos)
    {
        Ray ray = new Ray(pos.transform.position, direction);

        Physics.Raycast(ray, out RaycastHit info, distance, layerMask);

        transform.position = new Vector3(info.point.x, info.point.y, 0f);
    }

    public void Play(string animationName)
    {
        animator.Play(animationName);
    }
}
