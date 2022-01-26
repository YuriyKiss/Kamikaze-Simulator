using UnityEngine;
using System.Collections;

using MoreMountains.Feedbacks;

public class EndGameConditions : MonoBehaviour
{
    [SerializeField]
    private MMFeedbacks zoomFeedback;
    [SerializeField]
    private CameraMovement cam;
    [SerializeField]
    private GameObject InGameMenu;
    [SerializeField]
    private GameObject WinMenu;

    private PuppetController puppet;

    private GameObject[] zones;

    private float winMenuTimeDelay = 0.5f;

    private float newUpValue = 11f;
    private float newBottomValue = 0.25f;
    private float newLeftValue = -1.25f;
    private float newRightValue = 1.25f;
    private float newSmoothingValue = 1.1f;

    private void Start()
    {
        zones = GameObject.FindGameObjectsWithTag("X Zone");

        puppet = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetController>();

        PlayerPrefs.SetInt("InGame", 1);
    }

    public void CheckCondition()
    {
        bool allZonesAreDestroyed = true;

        foreach (GameObject zone in zones)
        {
            if (zone != null)
            {
                allZonesAreDestroyed = false;

                break;
            }
        }

        if (allZonesAreDestroyed)
        {
            StartCoroutine(FinishGame());
        }
    }

    private IEnumerator FinishGame()
    {
        PlayerPrefs.SetInt("InGame", 0);

        zoomFeedback.PlayFeedbacks();

        cam.UpdateLimits(newUpValue, newBottomValue, newLeftValue, newRightValue, newSmoothingValue);

        InGameMenu.SetActive(false);

        yield return StartCoroutine(puppet.SetPuppetAlive());

        yield return new WaitForSeconds(winMenuTimeDelay);

        WinMenu.SetActive(true);
    }
}
