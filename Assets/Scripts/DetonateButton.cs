using UnityEngine;

public class DetonateButton : MonoBehaviour
{
    private EndGameConditions endGame;
    private PlayerExplosion playerExplosion;

    private GameObject[] zones;

    private Animator animator;
    private string pulsingAnimation = "Pulsing";

    private void OnEnable()
    {
        GameObject scriptsRoot = GameObject.FindGameObjectWithTag("Scripts");
        endGame = scriptsRoot.GetComponent<EndGameConditions>();

        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = playerRoot.GetComponent<PlayerManager>();
        playerExplosion = playerManager.playerExplosion;

        zones = GameObject.FindGameObjectsWithTag("X Zone");

        animator = GetComponent<Animator>();
    }

    public void ActivateExplosion()
    {
        animator.Play(pulsingAnimation);

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

        playerExplosion.DetonateBomb();
    }
}
