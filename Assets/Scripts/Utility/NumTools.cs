using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumTools
{
    public static float Blink(float t)
    { return 0.5f * (Mathf.Sin(2 * Mathf.PI * t) + 1); }

    public static float Throb(float t, float a)
    { return (a-1) * Blink(t) + a; }

    public static float Flash(float t, float a, float b, float c, float d)
    { return a * t * Mathf.Sin(b * t - c) + d; }

    public static float HillStep(float t, float k, bool reverse = false)
    {
        float value = (Mathf.Exp(k * t) - 1) / (Mathf.Exp(k) - 1);
        return reverse ? (1-value) : value;
    }

    public static float PerlinStep(float t, bool reverse = false)
    {
        float value = t * t * t * (t * (t * 6 - 15) + 10);
        return reverse ? (1-value) : value;
    }

    public static float PowStep(float t, float k, bool reverse = false)
    {
        float value = Mathf.Pow(t, k);
        return reverse ? (1-value) : value;
    }

    public static void BoogieWoogie(Transform a, Transform b)
    {
        // the act of applause is an acclamation of the soul
        Vector3 intermediate = a.position;
        a.position = b.position;
        b.position = intermediate;
    }
}
