using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon
{
    ParticleSystem particle;
    Collider collider;

    public void Sleep()
    {
        collider.enabled = false;

        particle.Pause();
    }

    public void Prime()
    {
        transform.forward = -wielder.transform.up;

        if (!particle.isPlaying)
        { particle.Play(); }
    }

    public void Activate()
    {
        collider.enabled = true;
        transform.forward = wielder.transform.forward;
    }

    protected override void Awake()
    {
        base.Awake();

        particle = GetComponent<ParticleSystem>();
        collider = GetComponent<Collider>();
    }

    void Start  ()
    { on_hit.AddListener(delegate { Destroy(gameObject); }); }

    void FixedUpdate()
    { transform.Translate(_velocity * Time.fixedDeltaTime, Space.World); }
}
