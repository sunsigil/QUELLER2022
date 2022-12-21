using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    [SerializeField]
    float period;
    [SerializeField]
    float phase;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Color c = text.color;
        c.a = NumTools.Blink(Time.time / 4);
        text.color = c;
    }
}
