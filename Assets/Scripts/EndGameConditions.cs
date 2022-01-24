using UnityEngine;

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
            PlayerPrefs.SetInt("InGame", 0);

            zoomFeedback.PlayFeedbacks();

            cam.UpdateLimits(11f, 0.25f, -1.25f, 1.25f, 1.1f);

            StartCoroutine(puppet.SetPuppetAlive(ActivateMenu));

            InGameMenu.SetActive(false);

            
        }
    }

    private void ActivateMenu()
    {
        WinMenu.SetActive(true);
    }
}
