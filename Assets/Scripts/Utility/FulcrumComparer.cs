using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FulcrumComparer<F, C> : IComparer<F>
where C : IComparable
{
    public delegate C DistanceMethod(F fulcrum, F a);

    F _fulcrum;
    public F fulcrum
    {
        get => _fulcrum;
        set => _fulcrum = value;
    }

    DistanceMethod _distance_method;
    public DistanceMethod distance_method
    {
        get => _distance_method;
        set => _distance_method = value;
    }

    public int Compare(F a, F b)
    {
        C a_val = _distance_method(_fulcrum, a);
        C b_val = _distance_method(_fulcrum, b);
        return a_val.CompareTo(b_val);
    }

    public FulcrumComparer(F fulcrum, DistanceMethod distance_method)
    {
        _fulcrum = fulcrum;
        _distance_method = distance_method;
    }
}
