using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class CommandConsole : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI log;
    [SerializeField]
    TMP_InputField prompt;

    Controller controller;

    string log_string;
    Stack<string> command_stack;

    List<string> command_record;
    int record_index;

#region OP_FUNCS
    void Help()
    {
        Log("[OPERATIONS]");
        Log("clear");
        Log("save");
        Log("load");
        Log("quit");
        Log("reset [WARNING: WILL DELETE SAVE DATA]");
        Log("toggle_music <toggle: bool>");
        Log("play_clip <name: string>");
        Log("respawn");
    }

    void Clear()
    {
        log_string = "";
        log.text = "";
    }

    void Save()
    {
        Saver saver = FindObjectOfType<Saver>();
        if(saver == null){ Log("Error: saver not found"); return; }

        Log("Saving...");
        saver.Save();
    }

    void Load()
    {
        Saver saver = FindObjectOfType<Saver>();
        if(saver == null){ Log("Error: saver not found"); return; }

        Log("Load...");
        saver.Load();
    }

    void Quit()
    {
        Log("Quitting...");
        Application.Quit();
    }

    void Reset()
    {
        Saver saver = FindObjectOfType<Saver>();
        if(saver == null){ Log("Error: saver not found"); return; }

        Log("Clearing save data...");
        saver.Clear();
        Log("Restarting...");
        saver.Restart();
    }

    void ToggleMusic(Stack<string> arg_stack)
    {
        string tog_str = "";
        try{ tog_str = arg_stack.Pop(); }
        catch{ Log("Operation toggle_music requires one argument"); return; }

        bool tog = false;
        try{ tog = Boolean.Parse(tog_str); }
        catch{ Log("Argument idx must be of boolean type {True, False}"); return; }

        AudioWizard audio_wizard = FindObjectOfType<AudioWizard>();
        if(audio_wizard == null){ Log("Error: audio wizard not found"); return; }

        audio_wizard.ToggleMusic(tog);
    }

    void PlayClip(Stack<string> arg_stack)
    {
        string clip_str = "";
        try{ clip_str = arg_stack.Pop(); }
        catch{ Log("Operation play_clip requires one argument"); return; }

        AudioWizard audio_wizard = FindObjectOfType<AudioWizard>();
        if(audio_wizard == null){ Log("Error: audio wizard not found"); return; }

        if(!audio_wizard.PlayEffect(clip_str)){ Log($"Error: clip {clip_str} not found"); }
    }

    void Respawn()
    {
        foreach(SpawnPoint point in FindObjectsOfType<SpawnPoint>())
        { point.Respawn(); }
    }
#endregion

#region ONSUBMIT_FUNCS
    void Record(string command)
    {
        if(command_record.Count == 0 || command_record[command_record.Count-1] != command)
        {
            command_record.Add(command);
            record_index = command_record.Count;
        }
    }

    void Log(string text)
    {
        log_string += $"{text}\n";
        log.text = log_string;
        log.ForceMeshUpdate();

        if(log.isTextTruncated)
        {
            log_string = $"{text}\n";
            log.text = log_string;
        }
    }

    void Evaluate(string command)
    {
        string[] command_arr = command.ToLower().Split(' ');
        Array.Reverse(command_arr);
        command_stack = new Stack<string>(command_arr);

        string op = command_stack.Pop();

        switch(op)
        {
            case "help":
                Help();
                break;
            case "clear":
                Clear();
                break;
            case "save":
                Save();
                break;
            case "load":
                Load();
                break;
            case "quit":
                Quit();
                break;
            case "reset":
                Reset();
                break;
            case "toggle_music":
                ToggleMusic(command_stack);
                break;
            case "play_clip":
                PlayClip(command_stack);
                break;
            case "respawn":
                Respawn();
                break;
            default:
                Log($"Unknown operation: {op}");
                break;
        }
    }

    public void Process(string command)
    {
        if(String.IsNullOrWhiteSpace(command)){ prompt.text = ""; return; }

        command = command.Trim(new char[]{' ', '\n'});
        if(command.StartsWith("#")){ prompt.text = ""; return; }

        Record(command);
        Log(command);
        prompt.text = "";

        Evaluate(command);
    }
#endregion

#region PUBLIC_FUNCS
    void Awake()
    {
        controller = GetComponent<Controller>();

        command_record = new List<string>();
        record_index = 0;

        prompt.onSubmit.AddListener(Process);     
    }

    void Update()
    {
        if(command_record.Count != 0)
        {
            if(controller.Pressed(InputCode.SCROLL_UP))
            {
                record_index--;
                if(record_index < 0){ record_index++; }
                prompt.text = command_record[record_index];
            }
            else if(controller.Pressed(InputCode.SCROLL_DOWN))
            {
                record_index++;
                if(record_index > command_record.Count-1)
                {
                    record_index--;
                    prompt.text = "";
                }
                else
                {
                    prompt.text = command_record[record_index];
                }
            }
        }
    }

    void OnEnable()
    {
        PointerEventData e = new PointerEventData(EventSystem.current);
        e.button = PointerEventData.InputButton.Left;
        EventSystem.current.SetSelectedGameObject(prompt.gameObject, e);
        prompt.OnPointerClick(e);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion
}
