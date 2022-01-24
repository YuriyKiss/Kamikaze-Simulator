using UnityEngine;

public class DetonateButton : MonoBehaviour
{
    private GameObject[] zones;
    private GameObject player;

    private void OnEnable()
    {
        zones = GameObject.FindGameObjectsWithTag("X Zone");

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ActivateExplosion()
    {
        foreach (GameObject zone in zones)
        {
            if (zone != null)
            {
                ZoneController controller = zone.GetComponent<ZoneController>();

                if (controller.IsPlayerInside())
                {
                    controller.ActivateConnectedObjects();

                    Destroy(zone);

                    break;
                }
            }
        }

        player.GetComponent<ExplosionManager>().DetonateBomb();
    }
}
