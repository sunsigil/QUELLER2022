using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    TrailRenderer trail_prefab;

    [SerializeField]
    int damage;
    [SerializeField]
    bool lethal;
    [SerializeField]
    float trail_delay;

    [SerializeField]
    Transform shot_anchor;

    Combatant combatant;

    AudioWizard audio_wizard;

    

    public void Fire()
    {
        audio_wizard.PlayEffect("vineboom"); 

        TrailRenderer trail = Instantiate(trail_prefab);
        trail.transform.position = shot_anchor.position;

        Vector3 pos = shot_anchor.position;
        Vector3 dir = transform.forward;
        float dist = Mathf.Infinity;
        RaycastHit hit;

        if(Physics.Raycast(pos, dir, out hit, dist))
        {
            print(hit.transform.position);
            Combatant target = hit.transform.GetComponent<Combatant>();
            if(target != null)
            { target.EnqueueAttack(new Attack(combatant, dir, damage, lethal)); }
            trail.transform.position = hit.transform.position;
        }
        else
        { trail.transform.position = pos + dir * 50; }
    }

    void Awake()
    {
        combatant = GetComponent<Combatant>();

        audio_wizard = FindObjectOfType<AudioWizard>();  
    }

    void Start()
    { audio_wizard.PushMusic(gameObject, "DOOM_music_1"); }
}
