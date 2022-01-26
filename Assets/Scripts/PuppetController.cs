using UnityEngine;
using System.Collections;

using RootMotion.Dynamics;
using Input = InputWrapper.Input;

public class PuppetController : MonoBehaviour
{
    [SerializeField]
    private AnimatedCharacterController charAnim;

    private PuppetMaster puppet;
    private Animator anim;

    private Vector3 rayDirection = Vector3.down;
    private float floorMaxDistance = 0.5f;
    private int floorLayerMask = 1 << 0;

    private float timer = 0f;
    private bool targetStanding = false;
    private string danceAnimation = "Dancing";
    private string idleAnimation = "Standing";
    private float inactiveTimeForIdle = 1.5f;

    private float puppetRessurectionTime = 0.3f;

    private void Start()
    {
        puppet = GetComponentInParent<PuppetMaster>();
        anim = charAnim.GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("InGame") == 1)
        {
            if (Input.TouchCount > 0)
            {
                timer = 0f;
            }

            Ray ray = new Ray(transform.position, rayDirection);

            if (Physics.Raycast(ray, floorMaxDistance, floorLayerMask))
            {
                timer += Time.deltaTime;
            }

            if (timer > inactiveTimeForIdle && !targetStanding)
            {
                targetStanding = true;

                charAnim.SetPosition(transform);

                puppet.state = PuppetMaster.State.Alive;

                anim.Play(idleAnimation);
            }
            else if (timer <= inactiveTimeForIdle)
            {
                targetStanding = false;

                puppet.state = PuppetMaster.State.Dead;
            }
        }
    }

    public IEnumerator SetPuppetAlive()
    {
        yield return new WaitForSeconds(puppetRessurectionTime);

        while (!Physics.Raycast(transform.position, rayDirection, floorMaxDistance, floorLayerMask))
            yield return null;

        charAnim.SetPosition(transform);

        puppet.state = PuppetMaster.State.Alive;

        anim.Play(danceAnimation);
    }
}
