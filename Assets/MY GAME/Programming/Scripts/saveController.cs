using System;
using System.IO;
using Menu;
using UnityEngine;
using GameDev;

namespace GameDev
{
    [AddComponentMenu("GameDev/Save Controller")]
    public class saveController : MonoBehaviour
    {
        [SerializeField] private ControllerData controllerData = new ControllerData();
        public bool refUsingController;
        [SerializeField] private bool inMainMenu;

        [ContextMenu("Save")]
        public void Save()
        {
            RefValues();
            string json = JsonUtility.ToJson(controllerData);

            File.WriteAllText($"{Application.persistentDataPath}/ControllerSave.txt", json);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            if (!File.Exists($"{Application.persistentDataPath}/ControllerSave.txt")) { return; }

            string json = File.ReadAllText($"{Application.persistentDataPath}/ControllerSave.txt");

            controllerData = JsonUtility.FromJson<ControllerData>(json);
            SavedValues();
        }

        void Awake()
        {
            Load();
        }

        public void RefValues()
        {
            controllerData.usingController = refUsingController;
        }

        public void SavedValues()
        {
            refUsingController = controllerData.usingController;
            if (inMainMenu)
            {
                GetComponent<MyResolution>().ControllerToggle(refUsingController);
            }
        }
    }

    [Serializable]
    public class ControllerData
    {
        [SerializeField] public bool usingController;
    }
}
