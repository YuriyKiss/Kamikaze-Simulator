using UnityEngine;
using System.Collections.Generic;

using RootMotion.Dynamics;

public class GirlController : MonoBehaviour
{
    [SerializeField]
    private Material girlNormalMaterial;
    [SerializeField]
    private SpriteRenderer spriteToUpdate;
    [SerializeField]
    private Sprite spriteExample;


    [SerializeField]
    private GameObject balloon;
    private Rigidbody balloonRigidbody;

    [SerializeField]
    private List<Rigidbody> bones;

    private Animator animator;
    private PuppetMaster puppet;
    private SpriteRenderer sprite;
    private ParticleSystem confetti;
    private BoxCollider boxCollider;
    private SkinnedMeshRenderer meshRenderer;

    private float timer = 0f;
    private float deltaTimeModifier = 0.04f;
    private float xMovementPerUpdate = 0.01f;
    private float yMovementPerUpdate = 0.04f;
    private float targetZPosition = -2f;
    private float ballonDeviation = -0.11f;

    private string cheerAnimation = "Cheering Idle";

    private bool girlActive = false;
    private bool girlSaved = false;

    private void Start()
    { 
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponentInChildren<Animator>();
        puppet = GetComponentInChildren<PuppetMaster>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        confetti = GetComponentInChildren<ParticleSystem>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (balloonRigidbody != null)
        {
            float x = balloonRigidbody.position.x + Mathf.Sign(balloonRigidbody.position.x) * xMovementPerUpdate;
            float y = balloonRigidbody.position.y + yMovementPerUpdate;
            float z = balloonRigidbody.position.z;

            if (balloonRigidbody.transform.position.z >= targetZPosition)
            {
                timer += Time.deltaTime * deltaTimeModifier;
                z = Mathf.Lerp(balloonRigidbody.position.z, targetZPosition, timer);
            }

            balloonRigidbody.MovePosition(new Vector3(x, y, z));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerPuppet>() != null && girlActive)
        {
            boxCollider.enabled = false;

            spriteToUpdate.sprite = spriteExample;

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
        balloonRigidbody = instantiatedBallon.GetComponent<Rigidbody>();

        FixedJoint joint = balloonRigidbody.GetComponent<FixedJoint>();
        joint.connectedBody = bone;

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
