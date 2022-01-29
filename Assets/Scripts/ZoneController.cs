using UnityEngine;
using System.Collections.Generic;

using RayFire;

public class ZoneController : MonoBehaviour
{
    private bool playerInside = false;

    [SerializeField] 
    private List<RayfireRigid> destructibles;

    [SerializeField]
    private List<EnemyController> enemies;

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
    }
}
