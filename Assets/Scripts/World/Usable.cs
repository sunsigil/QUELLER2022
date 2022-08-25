using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using TMPro;

[RequireComponent(typeof(CircleCollider2D))]

public class Usable : MonoBehaviour
{
    [Header("Editor")]

    [SerializeField]
    Color inner_color = Color.green;
    [SerializeField]
    Color outer_color = Color.red;

    [Header("Game")]

    [SerializeField]
    float inner_dist;
    [SerializeField]
    float outer_dist;

    UsableState _state;
    public UsableState state
    {
        get => _state;
        set => _state = value;
    }

    UnityEvent _on_used;
    public UnityEvent on_used => _on_used;

    Transform _user;
    public Transform user => _user;
    Distline _distline;
    public Distline distline => _distline;
    public bool usable => _state == UsableState.ACTIVE && _distline.progress >= 1;

    public bool RequestUse()
    {
        if(usable)
        { _on_used.Invoke(); return true; }
        return false;
    }

    void Awake()
    {
        _on_used = new UnityEvent();

        _user = GameObject.FindObjectOfType<User>().transform;
        _distline  = new Distline(_user, transform, inner_dist, outer_dist);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = inner_color;
        Gizmos.DrawWireSphere(transform.position, inner_dist);

        Gizmos.color = outer_color;
        Gizmos.DrawWireSphere(transform.position, outer_dist);
    }
}
