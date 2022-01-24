using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private float topHeight;
    [SerializeField]
    private float bottomHeight;
    [SerializeField]
    private float leftMin;
    [SerializeField]
    private float rightMax;

    private Vector3 finalCameraPosition;

    private float smoothTime = 1.5f;
    private float cameraMovementSmoothingTime = 0.25f;
    private float twoFifth = 2f / 5f;
    private Vector2 playerViewportPosition;

    private bool cameraMovesUpwards = false;
    private bool cameraMovesDownwards = false;
    private bool cameraMovesLeft = false;
    private bool cameraMovesRight = false;

    private float timerX = 0f;
    private float timerY = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);

        finalCameraPosition = transform.position;

        if (playerViewportPosition.y > 1 - twoFifth)
        {
            cameraMovesUpwards = true;
            cameraMovesDownwards = false;

            ClearTimerY();
        }
        else if (playerViewportPosition.y < twoFifth)
        {
            cameraMovesDownwards = true;
            cameraMovesUpwards = false;

            ClearTimerY();
        }
        else
        {
            timerY += Time.deltaTime;

            if (timerY > cameraMovementSmoothingTime)
            {
                cameraMovesUpwards = false;
                cameraMovesDownwards = false;
            }
        }

        if (playerViewportPosition.x > 1 - twoFifth)
        {
            cameraMovesRight = true;
            cameraMovesLeft = false;

            ClearTimerX();
        }
        else if (playerViewportPosition.x < twoFifth)
        {
            cameraMovesLeft = true;
            cameraMovesRight = false;

            ClearTimerX();
        }
        else
        {
            timerX += Time.deltaTime;

            if (timerX > cameraMovementSmoothingTime)
            {
                cameraMovesLeft = false;
                cameraMovesRight = false;
            }
        }

        if (cameraMovesUpwards)
        {
            finalCameraPosition.y = topHeight;
        }
        else if (cameraMovesDownwards)
        {
            finalCameraPosition.y = bottomHeight;
        }

        if (cameraMovesLeft)
        {
            finalCameraPosition.x = leftMin;
        }
        else if (cameraMovesRight)
        {
            finalCameraPosition.x = rightMax;
        }

        transform.position = Vector3.Lerp(transform.position, finalCameraPosition,
                                              smoothTime * Time.deltaTime);
    }

    private void ClearTimerY()
    {
        timerY = 0f;
    }
    private void ClearTimerX()
    {
        timerX = 0f;
    }

    public void UpdateLimits(float top, float bottom, float left, float right, float smooth)
    {
        topHeight = top;
        bottomHeight = bottom;
        leftMin = left;
        rightMax = right;

        smoothTime = smooth;
    }
}
