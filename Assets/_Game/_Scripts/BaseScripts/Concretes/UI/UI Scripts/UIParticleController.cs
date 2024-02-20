using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _confettiParticle;
    [SerializeField] private ParticleSystem _itemSparkleParticle;
    [SerializeField] private ParticleSystem _boosterPanelParticle;

    public void PlayConfettiParticle()
    {
        _confettiParticle.Play();
    }

    public void PlayItemSparkleParticle()
    {
        _itemSparkleParticle.Play();
    }

    public void PlayBoosterPanelParticle()
    {
        _boosterPanelParticle.Play();
    }
    public void StopBoosterPanelParticle()
    {
        _boosterPanelParticle.Stop();
    }
}
