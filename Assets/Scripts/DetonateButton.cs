using UnityEngine;

public class DetonateButton : MonoBehaviour
{
    private EndGameConditions endGame;

    private GameObject[] zones;
    private GameObject player;

    private Animator anim;
    private string pulsingAnimation = "Pulsing";

    private void OnEnable()
    {
        endGame = GameObject.FindGameObjectWithTag("Scripts").GetComponent<EndGameConditions>();

        zones = GameObject.FindGameObjectsWithTag("X Zone");

        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
    }

    public void ActivateExplosion()
    {
        anim.Play(pulsingAnimation);

        foreach (GameObject zone in zones)
        {
            if (zone != null)
            {
                ZoneController controller = zone.GetComponent<ZoneController>();

                if (controller.IsPlayerInside())
                {
                    controller.ActivateConnectedObjects();

                    DestroyImmediate(zone);

                    endGame.CheckCondition();

                    break;
                }
            }
        }

        player.GetComponent<ExplosionManager>().DetonateBomb();
    }
}
