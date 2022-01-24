using UnityEngine;
using System.Collections;

using RayFire;
using MoreMountains.Feedbacks;

public class ExplosionManager : MonoBehaviour
{
    private MMFeedbacks feedback;

    private Movement playerMovement;
    private Vector2 explosionForce = new Vector2(0, 840f);
    private float forceMultiplier = 35f;

    [SerializeField]
    private ParticleSystem explosion;
    private RayfireBomb bomb;
    private float bombDelay = 0f;

    [SerializeField]
    private GameObject explosionDecal;
    private float decalLifeTime = 16f;
    private float decalMaxDistance = 2f;
    private int decalLayerMask = 1 << 0;
    private float decalOffset = 0.01f;
    private float decalCenter = 0f;
    private float decalRotation = 90;

    private void Start()
    {
        bomb = GetComponent<RayfireBomb>();
        playerMovement = GetComponent<Movement>();

        feedback = GameObject.FindGameObjectWithTag("Feedbacks").GetComponent<MMFeedbacks>();
    }

    public void DetonateBomb()
    {
        feedback.PlayFeedbacks();
        bomb.Explode(bombDelay);
        explosion.Play();
        ApplyDecals();
        playerMovement.ApplyForceToPlayer(explosionForce, forceMultiplier);
    }

    private void ApplyDecals()
    {
        DecalWrapper(Vector3.down, 0, decalOffset, 0);
        DecalWrapper(Vector3.left, decalOffset, 0, -decalRotation);
        DecalWrapper(Vector3.right, -decalOffset, 0, decalRotation);
        DecalWrapper(Vector3.up, 0, -decalOffset, 2 * decalRotation);
    }

    private void DecalWrapper(Vector3 direction, float xOffset, float yOffset, float zRotation)
    {
        Ray ray = new Ray(transform.position, direction);

        Physics.Raycast(ray, out RaycastHit info, decalMaxDistance, decalLayerMask);

        if (info.collider != null)
        {
            explosionDecal.transform.position = new Vector3(info.point.x + xOffset, info.point.y + yOffset, decalCenter);
            explosionDecal.transform.rotation = Quaternion.Euler(0, 0, zRotation);

            GameObject decal = Instantiate(explosionDecal, info.collider.transform, true);

            StartCoroutine(DestroyOverTime(decal, decalLifeTime));
        }
    }

    private IEnumerator DestroyOverTime(GameObject toDestroy, float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(toDestroy);
    }
}
