using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hole : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particlesPrefab = null;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        var ps = Instantiate(particlesPrefab, transform);
        SfxManager.Instance.PlayCheer();
        yield return new WaitForSeconds(2);
        ps.Stop();
    }
}