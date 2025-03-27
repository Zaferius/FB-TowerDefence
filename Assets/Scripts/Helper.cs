using UnityEngine;
using DG.Tweening;

public static class Helper
{
    public static void KillAllTweenReferencesIn(Transform root)
    {
        if (root == null) return;
        
        DOTween.Kill(root);
        
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            DOTween.Kill(child);
        }
        
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            foreach (var mat in rend.materials)
            {
                DOTween.Kill(mat); // Color / Fade için
            }
        }
        
        var rects = root.GetComponentsInChildren<RectTransform>(true);
        foreach (var rect in rects)
        {
            DOTween.Kill(rect);
        }
    }
}