using UnityEngine;
using System.Collections;

using RootMotion.Dynamics;
using Input = InputWrapper.Input;

public class PlayerPuppet: MonoBehaviour
{
    private PlayerAnimator playerAnimator;

    private PuppetMaster puppet;

    private Vector3 rayDirection = Vector3.down;
    private float floorMaxDistance = 1f;
    private int floorLayerMask = 1 << 0;

    private float timer = 0f;
    private bool targetStanding = false;
    private string danceAnimation = "Dancing";
    private string idleAnimation = "Standing";
    private float inactiveTimeForIdle = 1.5f;

    private float puppetRessurectionTime = 0.3f;

    private void Start()
    {
        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = playerRoot.GetComponent<PlayerManager>();

        playerAnimator = playerManager.playerAnimator;

        puppet = GetComponentInParent<PuppetMaster>();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("InGame") == 1)
        {
            if (Input.TouchCount > 0)
            {
                ClearTimer();
            }

            Ray ray = new Ray(transform.position, rayDirection);

            bool raycast = Physics.Raycast(ray, floorMaxDistance, floorLayerMask);

            if (raycast)
            {
                timer += Time.deltaTime;
            }

            if (timer > inactiveTimeForIdle && !targetStanding)
            {
                targetStanding = true;

                playerAnimator.SetPosition(transform);

                puppet.state = PuppetMaster.State.Alive;

                playerAnimator.Play(idleAnimation);
            }
            else if (timer <= inactiveTimeForIdle)
            {
                KnockoutPuppet();
            }
        }
    }

    public void ClearTimer()
    {
        timer = 0f;
    }

    public void KnockoutPuppet()
    {
        targetStanding = false;

        puppet.state = PuppetMaster.State.Dead;
    }

    public IEnumerator SetPuppetDancing()
    {
        yield return new WaitForSeconds(puppetRessurectionTime);

        while (!Physics.Raycast(transform.position, rayDirection, floorMaxDistance, floorLayerMask))
            yield return null;

        playerAnimator.SetPosition(transform);

        puppet.state = PuppetMaster.State.Alive;

        playerAnimator.Play(danceAnimation);
    }
}
