using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oharae : MonoBehaviour, ISpellcaster
{
    [SerializeField]
    float charge_time;
    [SerializeField]
    int min_count;
    [SerializeField]
    int max_count;

    [SerializeField]
    Spell prefab;

    [SerializeField]
    Transform anchor;

    AudioWizard audio_wizard;

    Timeline charge;
    int count;
    List<Spell> instances;
    
    public void Spawn()
    {
        while(instances.Count < count)
        {
            Spell instance = Instantiate(prefab, anchor);
            instance.Sleep();
            instances.Add(instance);
        }

        for(int i = 0; i < count; i++)
        {
            float r = 0.5f;
            float arc = 2 * Mathf.PI / count;
            float theta = arc * i;
            theta += Mathf.PI * 0.5f;

            Vector3 x_arm = anchor.right * Mathf.Cos(theta);
            Vector3 y_arm = anchor.up * Mathf.Sin(theta);
            Vector3 pos = (x_arm + y_arm) * r;

            instances[i].transform.position = anchor.position + pos;
        }
    }

    public void Charge(float amount)
    {
        charge.Tick(amount);

        int last_count = count;
        count = (int)Mathf.Lerp(min_count, max_count, charge.progress);
        print(count);

        if(count > last_count)
        { Spawn(); }
    }

    public void Cast()
    {   
        foreach(Spell instance in instances)
        {
            instance.Wake();
            instance.Launch(anchor.forward * 30);
        }

        charge = new Timeline(charge_time);
        count = 0;
        instances = new List<Spell>();

        audio_wizard.PlayEffect("bb_restore");
    }

    void Awake()
    {
        audio_wizard = FindObjectOfType<AudioWizard>();

        charge = new Timeline(charge_time);
        count = 0;
        instances = new List<Spell>();
    }
}
