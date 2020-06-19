using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxManager : Singleton<SfxManager>
{
    [SerializeField]
    private AudioClip ballHitClip = null, cheerClip = null;

    [SerializeField, HideInInspector]
    private AudioSource aSource = null;

    private void OnValidate()
    {
        if (null == aSource)
            aSource = GetComponent<AudioSource>();
    }

    public void PlayHitBall()
    {
        aSource.PlayOneShot(ballHitClip);
    }

    public void PlayCheer()
    {
        aSource.PlayOneShot(cheerClip);
    }
}