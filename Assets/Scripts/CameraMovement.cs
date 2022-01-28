using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera gameCamera;
    private Transform characterTransform;

    private Vector2 playerViewportPosition;
    private Vector3 desiredCameraPosition;

    private float maxUpPosition = 9f;
    private float maxDownPosition = 0.25f;
    private float maxLeftPosition = 0f;
    private float maxRightPosition = 0f;

    private bool cameraMovesUpwards = false;
    private bool cameraMovesDownwards = false;
    private bool cameraMovesLeft = false;
    private bool cameraMovesRight = false;

    private float cameraMovementSmoothingTime = 0.25f;
    private float cameraSmoothing = 1.5f;
    private float twoFifth = 2f / 5f;

    private float timerX = 0f;
    private float timerY = 0f;

    private void Start()
    {
        GameObject cameraRoot = GameObject.FindGameObjectWithTag("MainCamera");
        CameraManager cameraManager = cameraRoot.GetComponent<CameraManager>();

        gameCamera = cameraManager.GameCamera.GetComponent<Camera>();

        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = playerRoot.GetComponent<PlayerManager>();

        characterTransform = playerManager.playerTransform;
    }

    private void FixedUpdate()
    {
        playerViewportPosition = gameCamera.WorldToViewportPoint(characterTransform.position);

        desiredCameraPosition = transform.position;

        CalculateAxisValue(ref desiredCameraPosition.x, ref cameraMovesRight, ref cameraMovesLeft,
                           playerViewportPosition.x, ref timerX,
                           maxRightPosition, maxLeftPosition);

        CalculateAxisValue(ref desiredCameraPosition.y, ref cameraMovesUpwards, ref cameraMovesDownwards,
                           playerViewportPosition.y, ref timerY,
                           maxUpPosition, maxDownPosition);

        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition,
                                              cameraSmoothing * Time.deltaTime);
    }

    private void CalculateAxisValue
                              (ref float axisValue, 
                              ref bool axisPositiveMovement, ref bool axisNegativeMovement,
                              float axisPlayerViewportPosition, ref float timer,
                              float maxAxisPositiveValue, float maxAxisNegativeValue)
    {
        if (axisPlayerViewportPosition > 1 - twoFifth)
        {
            timer = 0f;

            axisPositiveMovement = true;
            axisNegativeMovement = false;
        }
        else if (axisPlayerViewportPosition < twoFifth)
        {
            timer = 0f;

            axisNegativeMovement = true;
            axisPositiveMovement = false;
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > cameraMovementSmoothingTime)
            {
                axisNegativeMovement = false;
                axisPositiveMovement = false;
            }
        }

        if (axisNegativeMovement)
        {
            axisValue = maxAxisNegativeValue;
        }
        else if (axisPositiveMovement)
        {
            axisValue = maxAxisPositiveValue;
        }
    }

    public void UpdateLimits(float top, float bottom, float left, float right, float smooth)
    {
        maxUpPosition = top;
        maxDownPosition = bottom;
        maxLeftPosition = left;
        maxRightPosition = right;

        cameraSmoothing = smooth;
    }
}
