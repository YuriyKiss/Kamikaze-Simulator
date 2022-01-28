using UnityEngine;

public class UpdateCanvasCamera : MonoBehaviour
{
    private void Start()
    {
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");

        Canvas canvas = canvasObject.GetComponent<Canvas>();

        canvas.worldCamera = GetComponent<Camera>();
    }
}
