using UnityEngine;

using RootMotion.Dynamics;

public class GirlController : MonoBehaviour
{
    private PuppetMaster puppet;
    private BoxCollider boxCollider;
    private Animator animator;
    private SkinnedMeshRenderer meshRenderer;

    private float timer = 0f;
    private float deltaTimeModifier = 0.1f;
    private Rigidbody[] bones;
    private GameObject createdBalloon; 

    [SerializeField]
    private GameObject balloon;

    [SerializeField]
    private Material girlNormal;

    private bool girlActive = false;
    private bool girlSaved = false;

    private string cheerAnimation = "Cheering Idle";

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        puppet = GetComponentInChildren<PuppetMaster>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        bones = GetComponentsInChildren<Rigidbody>();
    }

    private void Update()
    {
        if (createdBalloon != null)
        {
            Rigidbody ballonRigid = createdBalloon.GetComponent<Rigidbody>();

            float z = ballonRigid.position.z;

            if (createdBalloon.transform.position.z >= -2f)
            {
                timer += Time.deltaTime * deltaTimeModifier;

                z = Mathf.Lerp(ballonRigid.position.z, -2f, timer);
            }
            float y = ballonRigid.position.y + 0.05f;

            ballonRigid.MovePosition(new Vector3(ballonRigid.position.x, y, z));
        }
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
        Rigidbody bone = bones[Random.Range(0, bones.Length)];

        // Spawn balloon
        balloon.transform.position = bone.transform.position;
        createdBalloon = Instantiate(balloon, transform.parent, true);

        // Connect girl to balloon
        FixedJoint joint = bone.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = createdBalloon.GetComponent<Rigidbody>();

        girlSaved = true;
    }

    public void ActivateGirl()
    {
        meshRenderer.material = girlNormal;

        girlActive = true;

        animator.Play(cheerAnimation);
    }

    public bool IsSaved() => girlSaved;
}
