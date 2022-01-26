using UnityEngine;
using System.Collections;
using System;

using RootMotion.Dynamics;

public class PuppetController : MonoBehaviour
{
    [SerializeField]
    private AnimatedCharacterController charAnim;

    private PuppetMaster puppet;

    private float puppetRessurectionTime = 0.3f;

    private void Start()
    {
        puppet = GetComponentInParent<PuppetMaster>();
    }

    public IEnumerator SetPuppetAlive()
    {
        yield return new WaitForSeconds(puppetRessurectionTime);

        while (!Physics.Raycast(transform.position, Vector3.down, 0.5f, 1 << 0))
            yield return null;

        charAnim.SetPosition(transform);

        puppet.state = PuppetMaster.State.Alive;
    }
}
