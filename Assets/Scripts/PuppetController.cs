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

    private float timer = 0f;
    private bool targetStanding = false;
    private string danceAnimation = "Dancing";
    private string idleAnimation = "Standing";

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

            if (Physics.Raycast(transform.position, Vector3.down, 0.5f, 1 << 0))
            {
                timer += Time.deltaTime;
            }

            if (timer > 1.5f && !targetStanding)
            {
                targetStanding = true;

                charAnim.SetPosition(transform);

                puppet.state = PuppetMaster.State.Alive;

                anim.Play(idleAnimation);
            }
            else if (timer <= 1.5f)
            {
                targetStanding = false;

                puppet.state = PuppetMaster.State.Dead;
            }
        }
    }

    public IEnumerator SetPuppetAlive()
    {
        yield return new WaitForSeconds(puppetRessurectionTime);

        while (!Physics.Raycast(transform.position, Vector3.down, 0.5f, 1 << 0))
            yield return null;

        charAnim.SetPosition(transform);

        puppet.state = PuppetMaster.State.Alive;

        anim.Play(danceAnimation);
    }
}
