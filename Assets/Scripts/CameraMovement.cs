using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private float topHeight;
    [SerializeField]
    private float bottomHeight;

    private Vector3 topCameraPosition;
    private Vector3 bottomCameraPosition;

    private float smoothTime = 1.5f;
    private float cameraMovementSmoothingTime = 0.25f;
    private float oneThird = 1f / 3f;
    private Vector2 playerViewportPosition;

    private bool cameraMovesUpwards = false;
    private bool cameraMovesDownwards = false;

    private float timer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        topCameraPosition = new Vector3(transform.position.x, topHeight, transform.position.z);
        bottomCameraPosition = new Vector3(transform.position.x, bottomHeight, transform.position.z);
    }

    private void FixedUpdate()
    {
        playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);

        if (playerViewportPosition.y > 1 - oneThird)
        {
            cameraMovesUpwards = true;
            cameraMovesDownwards = false;

            ClearTimer();
        }
        else if (playerViewportPosition.y < oneThird)
        {
            cameraMovesDownwards = true;
            cameraMovesUpwards = false;

            ClearTimer();
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > cameraMovementSmoothingTime)
            {
                cameraMovesUpwards = false;
                cameraMovesDownwards = false;
            }
        }

        if (cameraMovesUpwards)
        {
            transform.position = Vector3.Lerp(transform.position, topCameraPosition, 
                                              smoothTime * Time.deltaTime);
        }
        else if (cameraMovesDownwards)
        {
            transform.position = Vector3.Lerp(transform.position, bottomCameraPosition, 
                                              smoothTime * Time.deltaTime);
        }
    }

    private void ClearTimer()
    {
        timer = 0f;
    }
}
