using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    GameObject target_widget_prefab;
    [SerializeField]
    NotificationWidget notification_widget_prefab;

    [SerializeField]
    Image health_bar;
    Combatant combatant;
    Material health_bar_mat;

    List<GameObject> target_widgets;
    Targeter targeter;
    Camera projection_cam;

    public void PushNotification(string message)
    {
        NotificationWidget widget = Instantiate(notification_widget_prefab, transform);
        widget.Bind(message);
    }

    private void Awake()
    {
        health_bar_mat = health_bar.material;
        target_widgets = new List<GameObject>();
    }

    private void Start()
    {
        projection_cam = Camera.main;
        combatant = FindObjectOfType<User>().GetComponent<Combatant>();
        targeter = FindObjectOfType<User>().GetComponent<Targeter>();
    }

    // Update is called once per frame
    void Update()
    {
        health_bar_mat.SetFloat("_Progress", combatant.life);

        foreach (GameObject widget in target_widgets)
        {
            Destroy(widget);
        }
        target_widgets = new List<GameObject>();

        foreach(Combatant target in targeter.targets)
        {
            Vector3 ws_pos = target.transform.position + Vector3.up * 0.5f;
            Vector3 line = ws_pos - targeter.transform.position;
            Vector3 fore = targeter.transform.forward;

            if(Vector3.Dot(fore, line) < 0)
            {
                continue;
            }

            Vector2 ss_pos = projection_cam.WorldToScreenPoint(ws_pos);
            Vector2 scale = new Vector2(1, 1) * Mathf.Lerp(0.1f, 1.5f, 1/Vector3.Distance(ws_pos, targeter.transform.position));

            GameObject widget = Instantiate(target_widget_prefab, transform);
            widget.transform.position = ss_pos;
            widget.GetComponent<RectTransform>().sizeDelta = scale * 100;
            target_widgets.Add(widget);
        }
    }
}
