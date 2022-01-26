using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;

    private float maxUpPosition = 9f;
    private float maxDownPosition = 0.25f;
    private float maxLeftPosition = 0f;
    private float maxRightPosition = 0f;

    private bool cameraMovesUpwards = false;
    private bool cameraMovesDownwards = false;
    private bool cameraMovesLeft = false;
    private bool cameraMovesRight = false;

    private Vector2 playerViewportPosition;
    private Vector3 desiredCameraPosition;

    private float cameraMovementSmoothingTime = 0.25f;
    private float cameraSmoothing = 1.5f;
    private float twoFifth = 2f / 5f;

    private float timerX = 0f;
    private float timerY = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);

        desiredCameraPosition = transform.position;

        CalculateAxisValue(ref desiredCameraPosition.x, ref cameraMovesRight, ref cameraMovesLeft,
                           playerViewportPosition.x, ClearTimerX, ref timerX,
                           maxRightPosition, maxLeftPosition);

        CalculateAxisValue(ref desiredCameraPosition.y, ref cameraMovesUpwards, ref cameraMovesDownwards,
                           playerViewportPosition.y, ClearTimerY, ref timerY,
                           maxUpPosition, maxDownPosition);

        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition,
                                              cameraSmoothing * Time.deltaTime);
    }

    private void CalculateAxisValue
                              (ref float axisValue, 
                              ref bool axisPositiveMovement, ref bool axisNegativeMovement,
                              float axisPlayerViewportPosition, 
                              Action ClearTimer, ref float timer,
                              float maxAxisPositiveValue, float maxAxisNegativeValue)
    {
        if (axisPlayerViewportPosition > 1 - twoFifth)
        {
            axisPositiveMovement = true;
            axisNegativeMovement = false;

            ClearTimer();
        }
        else if (axisPlayerViewportPosition < twoFifth)
        {
            axisNegativeMovement = true;
            axisPositiveMovement = false;

            ClearTimer();
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
        maxUpPosition = top;
        maxDownPosition = bottom;
        maxLeftPosition = left;
        maxRightPosition = right;

        cameraSmoothing = smooth;
    }
}
