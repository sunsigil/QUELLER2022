using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, ISpellcaster
{
    [SerializeField]
    float charge_time;
    [SerializeField]
    float min_scale;
    [SerializeField]
    float max_scale;

    [SerializeField]
    Spell prefab;

    [SerializeField]
    Transform anchor;

    Spell instance;
    Timeline charge;

    public void Spawn()
    {
        instance = Instantiate(prefab, anchor);
        instance.transform.localScale = Vector3.one * min_scale;
        instance.Sleep();

        charge = new Timeline(charge_time);
    }

    public void Charge(float amount)
    {
        if (instance == null)
        { Spawn(); }
        else
        {
            charge.Tick(amount);
            float size = Mathf.Lerp(min_scale, max_scale, charge.progress);
            instance.transform.localScale = Vector3.one * size;
        }
    }

    public void Cast()
    {
        if(instance == null)
        { Spawn(); }

        instance.Wake();
        instance.Launch(transform.forward * 30);
        instance = null;
    }
}
