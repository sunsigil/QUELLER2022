using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellcaster : MonoBehaviour
{
    // Settings
    [SerializeField]
    float charge_time;
    [SerializeField]
    float min_scale;
    [SerializeField]
    float max_scale;

    // Props
    [SerializeField]
    Missile projectile_prefab;

    // Plugins
    [SerializeField]
    Transform projectile_anchor;

    // Components
    Controller controller;
    Combatant combatant;

    // State
    Missile projectile;
    Timeline charge;

    void SpawnProjectile()
    {
        projectile = Instantiate(projectile_prefab, projectile_anchor);
        projectile.transform.localScale = Vector3.one * min_scale;
        projectile.Sleep();

        charge = new Timeline(charge_time);
    }

    void LaunchProjectile()
    {
        projectile.velocity = transform.forward * 30;
        projectile.transform.SetParent(null);
        projectile.Activate();
    }

    public void Charge(float amount)
    {
        charge.Tick(amount);
        projectile.Prime();
    }

    public void Cast()
    {
        LaunchProjectile();
        SpawnProjectile();
    }

    void Awake()
    {
        controller = GetComponent<Controller>();
        combatant = GetComponent<Combatant>();

        SpawnProjectile();
    }

    void Update()
    {
        float size = Mathf.Lerp(min_scale, max_scale, charge.progress);
        projectile.transform.localScale = Vector3.one * size;
    }
}
