using UnityEngine;
using System.Collections;

using RayFire;
using MoreMountains.Feedbacks;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private MMFeedbacks shakeFeedback;

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
    private float decalMinDistance = 0.05f;
    private float decalMaxDistance = 2f;
    private int decalLayerMask = 1 << 0;
    private float decalOffset = 0.01f;
    private float decalCenter = 0f;
    private float decalRotation = 90;

    private void Start()
    {
        bomb = GetComponent<RayfireBomb>();
        playerMovement = GetComponent<Movement>();
    }

    public void DetonateBomb()
    {
        shakeFeedback.PlayFeedbacks();
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

            StripDecal(decal, direction);

            StartCoroutine(DestroyOverTime(decal, decalLifeTime));
        }
    }

    private void StripDecal(GameObject decal, Vector3 direction)
    {
        Mesh decalMesh = decal.GetComponent<MeshFilter>().mesh;

        float extent = decalMesh.bounds.extents.x;

        float xMinCoord = -10f, xMaxCoord = 10f;
        for (float i = 0; i <= extent; i += 0.1f)
        {
            if (RaycastForStripping(decal, direction, ref xMaxCoord, i)) break;
        }
        for (float i = 0; i >= -extent; i -= 0.1f)
        {
            if (RaycastForStripping(decal, direction, ref xMinCoord, i)) break;
        }

        Vector3[] vertices = decalMesh.vertices;

        for (int i = 0; i < vertices.Length; ++i)
        {
            if (vertices[i].x <= xMinCoord)
            {
                vertices[i] = new Vector3(xMinCoord, vertices[i].y, vertices[i].z);
            }
            if (vertices[i].x >= xMaxCoord)
            {
                vertices[i] = new Vector3(xMaxCoord, vertices[i].y, vertices[i].z);
            }
        }

        decalMesh.vertices = vertices;
    }

    private bool RaycastForStripping(GameObject decal, Vector3 direction, ref float limitCoord, float i)
    {
        Vector3 transformed = decal.transform.TransformPoint(new Vector3(i, 0, 0));

        if (!Physics.Raycast(transformed, direction, decalMinDistance, decalLayerMask))
        {
            limitCoord = i;
            return true;
        }

        return false;
    }

    private IEnumerator DestroyOverTime(GameObject toDestroy, float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(toDestroy);
    }
}
