using UnityEngine;
using UnityEditor;

public class GetAnimationClip : MonoBehaviour
{
#if UNITY_EDITOR
    private string loadFrom = "Assets/Animations/Girl/Scared Idle.fbx";
    private string saveTo = "Assets/Animations/Girl/Scared Idle.anim";

    void Start()
    {
        // Extract the clip
        AnimationClip orgClip = (AnimationClip)AssetDatabase.
            LoadAssetAtPath(loadFrom, typeof(AnimationClip));

        // Save the clip
        AnimationClip placeClip = new AnimationClip();

        EditorUtility.CopySerialized(orgClip, placeClip);

        AssetDatabase.CreateAsset(placeClip, saveTo);
        AssetDatabase.Refresh();
    }
#endif
}