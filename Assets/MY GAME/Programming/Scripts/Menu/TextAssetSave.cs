using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [AddComponentMenu("GameDev/Menu/Text Asset Save")]
    public class TextAssetSave : MonoBehaviour
    {
        public void CreateText(Text fileName)
        {
            // Path
            string path = Application.dataPath + $"/{fileName.text}.txt";
            //Create a file if file doesn't exist
            Debug.Log(path);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "New");
            }
            //content for the file
            //string content = $"Log Date/Time: {System.DateTime.Now}";
            string content = "";
            // Put content into file
            File.WriteAllText(path, content); // Replace in the file
                                              // File.AppenedAllText(path, content); // Adds to the file
        }

        //void Start()
        //{
        //    CreateText();
        //}
    }
}