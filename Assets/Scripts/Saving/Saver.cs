using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    [SerializeField]
    string save_path;
    string full_save_path => $"{Application.persistentDataPath}\\{save_path}";

    List<ISavable> FindSavables()
    {
        List<ISavable> savables = new List<ISavable>();

        foreach(GameObject candidate in GameObject.FindGameObjectsWithTag("Savable"))
        {
            foreach(Component component in candidate.GetComponents(typeof(Component)))
            {
                if(component is ISavable){ savables.Add(component as ISavable); }
            }
        }

        return savables;
    }

    [ContextMenu("Clear Saved Data")]
    public void Clear()
    {
        StreamWriter writer = new StreamWriter(full_save_path);
        writer.WriteLine("");
        writer.Close();
    }

    public void Save()
    {
        List<ISavable> savables = FindSavables();
        StreamWriter writer = new StreamWriter(full_save_path);

        foreach(ISavable savable in savables)
        {
            string block = savable.WriteBlock();

            writer.WriteLine($"[{savable.GetType().ToString()}]");
            if(block.Contains('\n'))
            {
                string[] lines = block.Split('\n');
                foreach(string line in lines)
                { writer.WriteLine(line); }
            }
            else{ writer.WriteLine(block); }
            writer.WriteLine("+");
        }

        writer.Close();
    }

    public void Load()
    {
        List<ISavable> savables = FindSavables();

        StreamReader reader = new StreamReader(full_save_path);
        ISavable target = null;
        string block = "";
        string line = "";

        while((line = reader.ReadLine()) != null)
        {
            if(target == null && line.StartsWith('[')) // Retarget
            {
                string name = line.Trim(new char[]{'[', ']'});
                foreach(ISavable savable in savables)
                {
                    string candidate_name = savable.GetType().ToString();
                    if(candidate_name.Equals(name))
                    {
                        target = savable;
                        break;
                    }
                }
            }
            else if(target != null)
            {
                if(line.Equals("+")) // End block
                {
                    if(!target.ReadBlock(block))
                    { target.LoadDefaults(); }

                    savables.Remove(target);
                    target = null;
                    block = "";
                }
                else{ block += line.Trim(); } // Continue block
            }
        }

        foreach(ISavable savable in savables) // Cleanup unsaved
        { savable.LoadDefaults(); }

        reader.Close();
    }

    public void Restart()
    {
        Save();
        SceneManager.LoadScene("SampleScene");
    }

    void Start()
    {
        Load();
    }

    void OnDestroy()
    { Save(); }

    void OnApplicationQuit()
    { Save(); }
}
