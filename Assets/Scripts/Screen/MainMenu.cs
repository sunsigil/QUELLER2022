using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    string entry_point;

    Controller controller;

    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.Pressed(InputCode.CONFIRM))
        {
            SceneManager.LoadScene(entry_point);
        }
    }
}
