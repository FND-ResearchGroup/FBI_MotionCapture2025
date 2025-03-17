using UnityEngine;

public class updateMaeshAvatar : MonoBehaviour
{
    void OnEnable()
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
            renderer.enabled = true;
        }

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }
}
