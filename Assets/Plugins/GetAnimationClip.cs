using UnityEngine;
using UnityEditor;

public class GetAnimationClip : MonoBehaviour
{
    void Start()
    {
        // Extract the clip
        AnimationClip orgClip = (AnimationClip)AssetDatabase.
                                LoadAssetAtPath("Assets/Animations/Character/Dancing.fbx",
                                                typeof(AnimationClip));

        // Save the clip
        AnimationClip placeClip = new AnimationClip();

        EditorUtility.CopySerialized(orgClip, placeClip);

        AssetDatabase.CreateAsset(placeClip, "Assets/Animations/Character/Dancing.anim");
        AssetDatabase.Refresh();
    }
}
