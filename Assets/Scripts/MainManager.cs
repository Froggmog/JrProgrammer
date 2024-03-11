using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    private static MainManager instance;

    public Color teamColor = Color.magenta;

    public static MainManager Instance
    {
        get
        {
            return instance;

        }
        set => instance = value;
    }

    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadColor();
    }

    [System.Serializable]
    class SaveData
    {
        public Color teamColor;
    }

    public void SaveColor(){
        SaveData saveData = new SaveData();
        saveData.teamColor = teamColor;
        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", jsonString);
    }

    public void LoadColor(){
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            teamColor = data.teamColor;
        }
    }
}
