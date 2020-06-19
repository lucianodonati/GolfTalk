using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxManager : Singleton<SfxManager>
{
    [SerializeField]
    private AudioClip[] ballHitClips = null;

    [SerializeField, HideInInspector]
    private AudioSource aSource = null;

    private void OnValidate()
    {
        if (null == aSource)
            aSource = GetComponent<AudioSource>();
    }

    public void PlayHitBall()
    {
        aSource.clip = ballHitClips[0];
        aSource.PlayScheduled(AudioSettings.dspTime);
    }
}