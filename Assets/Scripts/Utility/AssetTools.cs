using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetTools
{
    public static Dictionary<string, T> ResourceMap<T>(string path) where T : Object
    {
        Dictionary<string, T> dict = new Dictionary<string, T>();

        T[] assets = Resources.LoadAll<T>(path);

        foreach(T asset in assets)
        {
            dict.Add(asset.name, asset);
        }

        return dict;
    }

	public static T InitGet<T>(T prefab)
	where T : MonoBehaviour
	{ return Object.Instantiate(prefab.gameObject).GetComponent<T>(); }

	public static T InitGet<T>(GameObject prefab)
	where T : MonoBehaviour
	{ return Object.Instantiate(prefab).GetComponent<T>(); }
}
