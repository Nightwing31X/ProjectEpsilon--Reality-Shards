using System;
using System.IO;
using Menu;
using UnityEngine;
using GameDev;

namespace GameDev
{
    [AddComponentMenu("GameDev/Save Fullscreen")]
    public class saveFullscreen : MonoBehaviour
    {
        [SerializeField] private FullscreenData fullscreenData = new FullscreenData();
        public bool refFullscreen; 

        [ContextMenu("Save")]
        public void Save()
        {
            RefValues();
            string json = JsonUtility.ToJson(fullscreenData);

            File.WriteAllText($"{Application.persistentDataPath}/FullscreenSave.txt", json);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            if(!File.Exists($"{Application.persistentDataPath}/FullscreenSave.txt")) {return;}

            string json = File.ReadAllText($"{Application.persistentDataPath}/FullscreenSave.txt");

            fullscreenData = JsonUtility.FromJson<FullscreenData>(json);
            SavedValues();
        }

        void Awake()
        {
            Load();
        }

        public void RefValues()
        {
            fullscreenData.fullscreen = refFullscreen;
        }

        public void SavedValues()
        {
            refFullscreen = fullscreenData.fullscreen;
            GetComponent<MyResolution>().FullscreenToggle(refFullscreen);
        }
    }

    [Serializable]
    public class FullscreenData
    {
        [SerializeField] public bool fullscreen;
    }
}
