using UnityEngine;
using System.Collections;
using System;

using RootMotion.Dynamics;

public class PuppetController : MonoBehaviour
{
    [SerializeField]
    private UpdateCurrentPosition charAnim;

    private PuppetMaster puppet;

    private void Start()
    {
        puppet = GetComponentInParent<PuppetMaster>();
    }

    public IEnumerator SetPuppetAlive(Action callback)
    {
        yield return new WaitForSeconds(1.5f);

        while (GetComponent<Rigidbody>().velocity.y > 0.001f)
            yield return null;

        charAnim.SetAnimatorCharacter(transform);

        puppet.state = PuppetMaster.State.Alive;

        yield return new WaitForSeconds(0.5f);

        callback.Invoke();
    }
}
