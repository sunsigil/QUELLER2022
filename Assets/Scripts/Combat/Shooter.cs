using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    TrailRenderer trail_prefab;
    [SerializeField]
    BulletLine line_prefab;

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

    Vector3 GetTrajectory()
    {
        Vector3 eye_pos = Camera.main.transform.position;
        Vector3 eye_dir = Camera.main.transform.forward;
        float gaze_length = Mathf.Infinity;
        RaycastHit gaze_hit;

        Vector3 farpoint = eye_pos + eye_dir * 50;
        if(Physics.Raycast(eye_pos, eye_dir, out gaze_hit, gaze_length))
        { farpoint = gaze_hit.point; }

        return farpoint - shot_anchor.position;
    }

    public void Fire()
    {
        audio_wizard.PlayEffect("bb_gun_blunt");

        BulletLine line = Instantiate(line_prefab);

        Vector3 pos = shot_anchor.position;
        Vector3 dir = GetTrajectory();
        float dist = Mathf.Infinity;
        RaycastHit hit;

        if(Physics.Raycast(pos, dir, out hit, dist))
        {
            Combatant target = hit.transform.GetComponent<Combatant>();
            if(target != null)
            { target.EnqueueAttack(new Attack(combatant, dir, damage, lethal)); }
            line.Initialize(pos, hit.point, 100);
        }
        else
        { line.Initialize(pos, pos + dir * 50, 100); }
    }

    void Awake()
    {
        combatant = GetComponent<Combatant>();

        audio_wizard = FindObjectOfType<AudioWizard>();  
    }

    void Start()
    { audio_wizard.PushMusic(gameObject, "DOOM_music_1"); }

    private void OnDrawGizmos()
    {
        if(shot_anchor)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(shot_anchor.position, shot_anchor.position + shot_anchor.forward);
        }
    }
}
