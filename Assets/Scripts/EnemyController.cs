using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

using RootMotion.Dynamics;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Material deadMaterial;

    private Animator anim;
    private PuppetMaster puppet;
    private SkinnedMeshRenderer rend;
    private PositionConstraint constraintPos;
    private RotationConstraint constraintRot;

    private GameObject player;
    private PuppetController playerPuppet;
    private Movement playerMovement;

    private bool kicking = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        puppet = GetComponentInChildren<PuppetMaster>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        constraintPos = GetComponentInChildren<PositionConstraint>();
        constraintRot = GetComponentInChildren<RotationConstraint>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerPuppet = player.GetComponent<PuppetController>();
        playerMovement = player.GetComponent<Movement>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2f
            && transform.position.y - 0.6f < player.transform.position.y)
        {
            if (!kicking)
            {
                kicking = true;
                StartCoroutine(KickPlayer());
            }
        }
    }

    private IEnumerator KickPlayer()
    {
        constraintPos.constraintActive = true;
        constraintRot.constraintActive = true;

        anim.Play("Boxing");
        yield return new WaitForSeconds(0.1f);

        if (this.enabled && Vector3.Distance(transform.position, player.transform.position) < 1.5f
            && transform.position.y - 0.6f < player.transform.position.y)
        {
			playerPuppet.ClearTimer();
            playerPuppet.KnockoutPuppet();
            playerMovement.ApplyForceToPlayer(new Vector2(600f, 600f), 40f);

            yield return new WaitForSeconds(1.133f);
        }

        constraintPos.constraintActive = false;
        constraintRot.constraintActive = false;

        kicking = false;
    }

    public void KillEnemy()
    {
        this.enabled = false;

        puppet.state = PuppetMaster.State.Dead;
        rend.material = deadMaterial;
    }
}
