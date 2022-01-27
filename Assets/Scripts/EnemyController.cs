using UnityEngine;
using System.Collections;

using RootMotion.Dynamics;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Material deadMaterial;

    private Animator anim;
    private PuppetMaster puppet;
    private SkinnedMeshRenderer rend;

    private GameObject player;
    private PuppetController playerPuppet;
    private Movement playerMovement;

    private bool kicking = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        puppet = GetComponentInChildren<PuppetMaster>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerPuppet = player.GetComponent<PuppetController>();
        playerMovement = player.GetComponent<Movement>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f
            && transform.position.y - 0.6f < player.transform.position.y)
        {
            if (!kicking)
            {
                kicking = true;
                StartCoroutine(KickPlayer());
            }
        }
        else
        {
            if (anim.isActiveAndEnabled)
                anim.Play("Boxing Idle");
        }
    }

    private IEnumerator KickPlayer()
    {
        anim.Play("Boxing");
        yield return new WaitForSeconds(0.75f);

        if (this.enabled && Vector3.Distance(transform.position, player.transform.position) < 1.5f
            && transform.position.y - 0.6f < player.transform.position.y)
        {
            playerPuppet.KnockoutPuppet();
            playerMovement.ApplyForceToPlayer(new Vector2(600f, 600f), 40f);

            yield return new WaitForSeconds(1.5f);
        }

        anim.Play("Boxing Idle");
        kicking = false;
    }

    public void KillEnemy()
    {
        this.enabled = false;

        puppet.state = PuppetMaster.State.Dead;
        rend.material = deadMaterial;
    }
}
