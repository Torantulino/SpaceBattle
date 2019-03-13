using UnityEngine;

public static class ParticleSystemExtension
{
    public static void EnableEmission(this ParticleSystem particleSystem, bool enabled)
    {
        var emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    public static float GetEmissionRate(this ParticleSystem particleSystem)
    {
        return particleSystem.emission.rateOverTime.constantMax;
    }

    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        var emission = particleSystem.emission;
        var rate = emission.rateOverTime;
        rate.constantMax = emissionRate;
        emission.rateOverTime = rate;
    }
}