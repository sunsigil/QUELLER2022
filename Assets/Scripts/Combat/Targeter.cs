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
    
    List<Combatant> targets;
    Combatant target;

    static float HDist(Combatant fulcrum, Combatant target)
    {
        Vector3 line = target.transform.position - fulcrum.transform.position;
        line.Scale(new Vector3(1, 0, 1));
        return line.sqrMagnitude;
    }

    public void Retarget()
    {
        targets = new List<Combatant>();
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
            if(candidate == target)
            { target_preserved = true; continue; }

            if (targets.Count < capacity)
            {
                print($"Adding: {candidate}");
                targets.Add(candidate);
            }
            else if (target_preserved)
            { break; }
        }

        FulcrumComparer<Combatant, float> comparer = new FulcrumComparer<Combatant, float>(GetComponent<Combatant>(), HDist);
        targets.Sort(comparer);

        if(!target_preserved && targets.Count > 0)
        { target = targets[0]; }
        else if(target != null)
        { targets.Insert(0, target); }

        print($"Targeting: {target}");
    }

    // Update is called once per frame
    void Update()
    {
        Retarget();
    }

    private void OnDrawGizmos()
    {
        if(targets != null)
        {
            Gizmos.color = Color.red;
            foreach(Combatant combatant in targets)
            {
                Gizmos.DrawLine(transform.position, combatant.transform.position);
                Gizmos.DrawSphere(combatant.transform.position, 0.25f);
            }
        }
        if(target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target.transform.position, 0.5f);
        }
    }
}
