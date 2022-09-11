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

    Timeline charge;
    int count;
    List<Spell> instances;
    

    public void Spawn()
    {
        if(instances == null)
        { instances = new List<Spell>(); }

        Spell instance = Instantiate(prefab, anchor);
        instance.Sleep();
        instances.Add(instance);

        for(int i = 0; i < count; i++)
        {
            float arc = 2 * Mathf.PI / count;
            float theta = arc * i;

            Vector3 x_arm = anchor.right * Mathf.Cos(theta);
            Vector3 y_arm = anchor.up * Mathf.Sin(theta);
            Vector3 pos = x_arm + y_arm;

            instances[i].transform.position = anchor.position + pos;
        }
    }

    public void Charge(float amount)
    {
        if (charge != null)
        { charge.Tick(amount); }
        else
        { charge = new Timeline(charge_time); }

        int last_count = count;
        count = (int)Mathf.Lerp(min_count, max_count, charge_time);

        if(count > last_count)
        { Spawn(); }
    }

    public void Cast()
    {   
        foreach(Spell instance in instances)
        {
            instance.Wake();
            instance.Launch(transform.forward * 30);
        }

        instances = null;
        charge = null;
    }
}
