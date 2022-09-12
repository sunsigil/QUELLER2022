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

    AudioWizard audio_wizard;

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
        instance.Launch(anchor.forward * 30);
        instance = null;

        audio_wizard.PlayEffect("bb_restore");
    }

    void Awake()
    { audio_wizard = FindObjectOfType<AudioWizard>(); }
}
