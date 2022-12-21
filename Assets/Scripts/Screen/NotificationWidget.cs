using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationWidget : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    Timeline timeline;

    public void Bind(string message)
    {
        text.text = message;
        timeline = new Timeline(1);
        text.transform.eulerAngles = Vector3.forward * Random.Range(-45, 45);
    }

    private void Update()
    {
        if(timeline == null)
        { return; }
        if(timeline.finished)
        { Destroy(gameObject); }

        timeline.Tick(Time.deltaTime);
        Color c = text.color;
        c.a = 1-timeline.progress;
        text.color = c;
    }
}
