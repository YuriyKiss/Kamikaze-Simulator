using UnityEngine;
using System.Collections;

public class GirlSavingController : MonoBehaviour
{
    private GameObject InGameMenu;
    private EndGameConditions endGame;

    [SerializeField]
    private GameObject trackerPrefab;
    [SerializeField]
    private Sprite newSpriteForTracker;

    private GameObject[] girls;
    private GameObject[] trackers;

    private float spritesMargin = 1.25f;
    private float trackerActivationDelay = 1.25f;
    private float endGameTrackerDelay = 0.75f;
    private string trackerPulsingAnimation = "Pulsing";

    private int trackerIndex = 0;

    private void Awake()
    {
        GameObject canvasRoot = GameObject.FindGameObjectWithTag("Canvas");
        CanvasManager canvasManager = canvasRoot.GetComponent<CanvasManager>();
        InGameMenu = canvasManager.InGameMenu;

        girls = GameObject.FindGameObjectsWithTag("Girl");
        trackers = new GameObject[girls.Length];

        endGame = GetComponent<EndGameConditions>();
    }

    private void Start()
    {
        for (int i = 0; i < girls.Length; ++i)
        {
            GameObject tracker = Instantiate(trackerPrefab, InGameMenu.transform, true);

            tracker.transform.position -= Vector3.right * i * spritesMargin;

            trackers[i] = tracker;
        }
    }

    public IEnumerator ActivateTracker()
    {
        SpriteRenderer renderer = trackers[trackerIndex].GetComponentInChildren<SpriteRenderer>();
        Animator animator = trackers[trackerIndex].GetComponentInChildren<Animator>();
        ParticleSystem particleSystem = trackers[trackerIndex].GetComponentInChildren<ParticleSystem>();

        yield return new WaitForSeconds(trackerActivationDelay);

        renderer.sprite = newSpriteForTracker;
        animator.Play(trackerPulsingAnimation);
        particleSystem.Play();

        trackerIndex++;

        yield return new WaitForSeconds(endGameTrackerDelay);

        endGame.CheckCondition();
    }

    public bool AreGirlsSaved() => girls.Length == trackerIndex;
}
