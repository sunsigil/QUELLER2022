using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField]
    Faction faction;
    [SerializeField]
    float radius;
    [SerializeField]
    int capacity;
    
    List<Combatant> _targets;
    public List<Combatant> targets => _targets;
    Combatant _target;
    public Combatant target => _target;

    static float HDist(Combatant fulcrum, Combatant target)
    {
        Vector3 line = target.transform.position - fulcrum.transform.position;
        line.Scale(new Vector3(1, 0, 1));
        return line.sqrMagnitude;
    }

    public void Retarget()
    {
        _targets = new List<Combatant>();

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, ~LayerMask.NameToLayer("Character"));
        if(hits.Length == 0)
        { return; }

        // if target is found as a candidate, dont add it to the list
        // flag a bool and dont nullify the target
        // add the target to the front of the target list after all the sorting is done
        bool target_preserved = false;

        foreach (Collider hit in hits)
        {
            Combatant candidate = hit.transform.GetComponent<Combatant>();
            if(candidate == null || candidate.faction == faction)
            { continue; }
            if(candidate == _target)
            { target_preserved = true; continue; }

            if (_targets.Count < capacity)
            {
                _targets.Add(candidate);
            }
            else if (target_preserved)
            { break; }
        }

        FulcrumComparer<Combatant, float> comparer = new FulcrumComparer<Combatant, float>(GetComponent<Combatant>(), HDist);
        _targets.Sort(comparer);

        if(!target_preserved && _targets.Count > 0)
        {
            if(_target != null)
            { _target.on_untargeted.Invoke(); }
            _target = _targets[0];
            _target.on_targeted.Invoke();
        }
        else if(_target != null)
        { _targets.Insert(0, _target); }
    }

    private void Awake()
    {
        _targets = new List<Combatant>();
    }

    // Update is called once per frame
    void Update()
    {
        Retarget();
    }

    private void OnDrawGizmos()
    {
        if(_targets != null)
        {
            Gizmos.color = Color.red;
            foreach(Combatant combatant in _targets)
            {
                if(combatant == null)
                { return; }

                Gizmos.DrawLine(transform.position, combatant.transform.position);
                Gizmos.DrawSphere(combatant.transform.position, 0.25f);
            }
        }
        if(_target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_target.transform.position, 0.5f);
        }
    }
}
