using UnityEngine;

using Input = InputWrapper.Input;

public class Movement : MonoBehaviour
{
    private Rigidbody rigidb;

    [SerializeField]
    private float forceMultiplier;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private Vector2 swipe;

    private void Start()
    {
        rigidb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.TouchCount > 0 && PlayerPrefs.GetInt("InGame") == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;

                swipe = new Vector2(endTouchPosition.x - startTouchPosition.x, 
                                    endTouchPosition.y - startTouchPosition.y);

                ApplyForceToPlayer(swipe, forceMultiplier);
            }
        }
    }

    public void ApplyForceToPlayer(Vector2 direction, float force)
    {
        rigidb.AddForce(direction * force);
    }
}
