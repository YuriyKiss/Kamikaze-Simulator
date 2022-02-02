using UnityEngine;
using System.Collections.Generic;

using RootMotion.Dynamics;

public class GirlController : MonoBehaviour
{
    private GirlSavingController tracking;

    [SerializeField]
    private Material girlNormalMaterial;

    [SerializeField]
    private GameObject balloon;

    [SerializeField]
    private List<Rigidbody> bones;

    private Animator animator;
    private PuppetMaster puppet;
    private SpriteRenderer sprite;
    private ParticleSystem confetti;
    private BoxCollider boxCollider;
    private SkinnedMeshRenderer meshRenderer;

    private float ballonDeviation = -0.15f;

    private string cheerAnimation = "Cheering Idle";

    private bool girlActive = false;
    private bool girlSaved = false;

    // Line Renderer
    private LineRenderer line;

    private Rigidbody handBone;
    private Rigidbody balloonsRigid;

    private void Start()
    {
        GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
        tracking = scripts.GetComponent<GirlSavingController>();

        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponentInChildren<Animator>();
        puppet = GetComponentInChildren<PuppetMaster>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        confetti = GetComponentInChildren<ParticleSystem>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (line != null)
        {
            line.SetPosition(0, handBone.position + Vector3.down * 0.05f);
            line.SetPosition(1, balloonsRigid.position + Vector3.up * 0.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerPuppet>() != null && girlActive)
        {
            boxCollider.enabled = false;

            StartCoroutine(tracking.ActivateTracker());

            confetti.Play();

            ConnectToBalloon();
        }
    }

    private void ConnectToBalloon()
    {
        puppet.state = PuppetMaster.State.Dead;

        Rigidbody bone = bones[Random.Range(0, bones.Count)];

        balloon.transform.position = bone.transform.position + ballonDeviation * Vector3.up;
        GameObject instantiatedBallon = Instantiate(balloon, transform.parent, true);

        FixedJoint joint = instantiatedBallon.GetComponent<FixedJoint>();
        joint.connectedBody = bone;

        balloonsRigid = instantiatedBallon.GetComponent<Rigidbody>();
        handBone = bone;
        line = instantiatedBallon.GetComponent<LineRenderer>();

        girlSaved = true;
    }

    public void ActivateGirl()
    {
        meshRenderer.material = girlNormalMaterial;

        sprite.enabled = false;

        girlActive = true;

        animator.Play(cheerAnimation);
    }

    public bool IsSaved() => girlSaved;
}
