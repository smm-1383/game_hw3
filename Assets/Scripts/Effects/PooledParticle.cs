using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PooledParticle : MonoBehaviour, IPoolable
{
    private ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void OnSpawn()
    {
        particles.Play();
    }

    public void OnDespawn()
    {
        particles.Clear();
    }

    private void OnParticleSystemStopped()
    {
        PoolManager.Instance.Release(gameObject);
    }
}
