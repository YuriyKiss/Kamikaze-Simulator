using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    private Rigidbody rigidb;

    private float timer = 0f;
    private float deltaTimeModifier = 0.08f;
    private float xMovementPerUpdate = 0.01f;
    private float yMovementPerUpdate = 0.03f;
    private float targetZPosition = -2f;

    void Start()
    {
        rigidb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float x = rigidb.position.x + Mathf.Sign(rigidb.position.x) * xMovementPerUpdate;
        float y = rigidb.position.y + yMovementPerUpdate;
        float z = rigidb.position.z;

        if (rigidb.transform.position.z >= targetZPosition)
        {
            timer += Time.deltaTime * deltaTimeModifier;
            z = Mathf.Lerp(rigidb.position.z, targetZPosition, timer);
        }

        rigidb.MovePosition(new Vector3(x, y, z));
    }
}
