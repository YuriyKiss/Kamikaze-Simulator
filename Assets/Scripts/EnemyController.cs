using UnityEngine;

using RootMotion.Dynamics;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Material deadMaterial;

    private PuppetMaster puppet;
    private SkinnedMeshRenderer rend;

    private void Start()
    {
        puppet = GetComponentInChildren<PuppetMaster>();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void KillEnemy()
    {
        puppet.state = PuppetMaster.State.Dead;
        rend.material = deadMaterial;
    }
}
