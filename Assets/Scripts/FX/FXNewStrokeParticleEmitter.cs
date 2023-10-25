using System;
using System.Collections;
using System.Collections.Generic;
using Golf;
using UnityEngine;

public class FXNewStrokeParticleEmitter : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        Stroke.OnStrokeStatusChange += OnStrokeStatusChange;
    }

    private void OnDisable()
    {
        Stroke.OnStrokeStatusChange -= OnStrokeStatusChange;
    }

    private void OnStrokeStatusChange(Stroke stroke, StrokeStatus newStatus)
    {
        Debug.Log("Stroke Status Change:"+newStatus);
        if (newStatus == StrokeStatus.Aiming)
        {
            //we went from NotTaken to Aiming...
            transform.position = stroke.BallRB.position;
            _particleSystem.Play();
        }
    }
}
