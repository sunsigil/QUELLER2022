using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour
{
    LineRenderer line_renderer;

    Vector3 start;
    Vector3 end;
    Timeline timer;

    public void Initialize(Vector3 start, Vector3 end, float speed)
    {
        this.start = start;
        this.end = end;

        float dist = (end - start).magnitude;
        float time = dist / speed;
        timer = new Timeline(time);

        line_renderer.SetPosition(0, start);
        line_renderer.SetPosition(1, end);
    }

    private void Awake()
    { line_renderer = GetComponent<LineRenderer>(); }

    private void FixedUpdate()
    {
        if(timer == null)
        { return; }

        timer.Tick(Time.fixedDeltaTime);
        line_renderer.SetPosition(0, Vector3.Lerp(start, end, timer.progress));
        if(timer.finished)
        { Destroy(gameObject); }
    }
}
