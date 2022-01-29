using UnityEngine;

using RootMotion.Dynamics;

public class GirlController : MonoBehaviour
{
    private PuppetMaster puppet;
    private BoxCollider boxCollider;
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    private Material girlNormal;

    private bool girlActive = false;

    private string cheerAnimation = "Cheering Idle";

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        puppet = GetComponentInChildren<PuppetMaster>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerPuppet>() != null && girlActive)
        {
            boxCollider.enabled = false;

            ConnectToBalloon();
        }
    }

    private void ConnectToBalloon()
    {
        puppet.state = PuppetMaster.State.Dead;

        // Select random bone

        // Spawn balloon

        // Connect girl to balloon

        // Set balloon flying
    }

    public void ActivateGirl()
    {
        meshRenderer.material = girlNormal;

        girlActive = true;

        animator.Play(cheerAnimation);
    }
}
