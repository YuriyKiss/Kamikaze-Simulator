using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

using RootMotion.Dynamics;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Material deadMaterial;

    [SerializeField]
    private ParticleSystem kickEffect;

    private Animator anim;
    private PuppetMaster puppet;
    private SkinnedMeshRenderer rend;
    private PositionConstraint constraintPos;
    private RotationConstraint constraintRot;

    private Transform playerTransform;
    private PlayerPuppet playerPuppet;
    private PlayerMovement playerMovement;

    private bool kicking = false;

    private Vector2 playerPunchVector = new Vector2(600f, 600f);

    private float maxDistanceToKick = 2f;
    private float minHeightOffset = 0.6f;
    private float kickDelay = 0.1f;
    private float kickEffectDelay = 0.1f;
    private float kickAnimationLeftoverTime = 0.933f;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        puppet = GetComponentInChildren<PuppetMaster>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        constraintPos = GetComponentInChildren<PositionConstraint>();
        constraintRot = GetComponentInChildren<RotationConstraint>();

        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = playerRoot.GetComponent<PlayerManager>();

        playerTransform = playerManager.playerTransform;
        playerPuppet = playerManager.playerPuppet;
        playerMovement = playerManager.playerMovement;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        bool distanceCheck = distance < maxDistanceToKick;
        bool heightCheck = transform.position.y - minHeightOffset < playerTransform.position.y;

        if (distanceCheck && heightCheck && !kicking)
        {
            StartCoroutine(KickPlayer());
        }
    }

    private IEnumerator KickPlayer()
    {
        ConstraintsState(true);

        anim.Play("Boxing");
        yield return new WaitForSeconds(kickDelay);

        if (this.enabled)
        {
            playerPuppet.ClearTimer();
            playerPuppet.KnockoutPuppet();
            playerMovement.ApplyForceToPlayer(playerPunchVector);

            yield return new WaitForSeconds(kickEffectDelay);

            kickEffect.Play();

            yield return new WaitForSeconds(kickAnimationLeftoverTime);
        }

        ConstraintsState(false);
    }

    private void ConstraintsState(bool value)
    {
        constraintPos.constraintActive = value;
        constraintRot.constraintActive = value;

        kicking = value;
    }

    public void KillEnemy()
    {
        this.enabled = false;

        puppet.state = PuppetMaster.State.Dead;
        rend.material = deadMaterial;
    }
}
