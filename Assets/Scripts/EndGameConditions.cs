using UnityEngine;
using System.Collections;

using MoreMountains.Feedbacks;

public class EndGameConditions : MonoBehaviour
{
    private CanvasManager canvasManager;

    private MMFeedbacks feedbackZoom;
    private CameraMovement cameraMovement;
    private PlayerPuppet playerPuppet;

    private GameObject[] zones;

    private float winMenuTimeDelay = 0.5f;

    private float newUpValue = 11f;
    private float newBottomValue = 0.25f;
    private float newLeftValue = -1.25f;
    private float newRightValue = 1.25f;
    private float newSmoothingValue = 1.1f;

    private void Awake()
    {
        PlayerPrefs.SetInt("InGame", 1);
    }

    private void Start()
    {
        GameObject canvasRoot = GameObject.FindGameObjectWithTag("Canvas");
        canvasManager = canvasRoot.GetComponent<CanvasManager>();

        GameObject feedbacksRoot = GameObject.FindGameObjectWithTag("Feedbacks");
        FeedbacksManager feedbacksManager = feedbacksRoot.GetComponent<FeedbacksManager>();
        feedbackZoom = feedbacksManager.feedbackZoom;

        GameObject cameraRoot = GameObject.FindGameObjectWithTag("MainCamera");
        CameraManager cameraManager = cameraRoot.GetComponent<CameraManager>();
        cameraMovement = cameraManager.cameraMovement;

        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = playerRoot.GetComponent<PlayerManager>();
        playerPuppet = playerManager.playerPuppet;

        zones = GameObject.FindGameObjectsWithTag("X Zone");
    }

    public void CheckCondition()
    {
        foreach (GameObject zone in zones)
            if (zone != null)
                return;

        StartCoroutine(FinishGame());
    }

    private IEnumerator FinishGame()
    {
        PlayerPrefs.SetInt("InGame", 0);

        feedbackZoom.PlayFeedbacks();

        cameraMovement.UpdateLimits(newUpValue, newBottomValue, newLeftValue, 
                                        newRightValue, newSmoothingValue);

        canvasManager.InGameMenu.SetActive(false);

        yield return StartCoroutine(playerPuppet.SetPuppetDancing());

        yield return new WaitForSeconds(winMenuTimeDelay);

        canvasManager.WinMenu.SetActive(true);
    }
}
