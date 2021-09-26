using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuStorageManager : MonoBehaviour
{
    private static MenuStorageManager instance;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }
    private void OnDestroy()
    {
        instance = null;
    }

    public static MenuStorageManager Instance()
    {
        return instance;
    }

    public bool IsLevelExists(string fname)
    {
        return File.Exists(fname);
    }

    public List<string> LoadLevelsNames()
    {
        List<string> names = new List<string>();
        foreach (var f in Directory.GetFiles("./"))
        {
            FileInfo fnfo = new FileInfo(f);

            if (fnfo.Extension == ".lvl")
                names.Add(f.TrimStart(new char[] { '.', '/' }));
        }

        return names;
    }
}
