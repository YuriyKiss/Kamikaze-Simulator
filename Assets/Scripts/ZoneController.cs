using UnityEngine;

using RayFire;

public class ZoneController : MonoBehaviour
{
    private bool playerInside = false;

    private RayfireRigid[] destructibles;
    private EnemyController[] enemies;
    private GirlController[] girls;

    private void Start()
    {
        Transform explosionField = transform.parent;

        destructibles = explosionField.GetComponentsInChildren<RayfireRigid>();
        enemies = explosionField.GetComponentsInChildren<EnemyController>();
        girls = explosionField.GetComponentsInChildren<GirlController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerExplosion>() != null)
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerExplosion>() != null)
        {
            playerInside = false;
        }
    }

    public bool IsPlayerInside() => 
        playerInside;

    public void ActivateConnectedObjects()
    {
        foreach (RayfireRigid rigid in destructibles)
        {
            rigid.Initialize();
            rigid.Demolish();
        }

        foreach (EnemyController enemy in enemies)
        {
            enemy.KillEnemy();
        }

        foreach (GirlController girl in girls)
        {
            girl.ActivateGirl();
        }
    }
}
