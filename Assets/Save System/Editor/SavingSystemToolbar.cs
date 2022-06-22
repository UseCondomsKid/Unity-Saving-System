using System.IO;
using UnityEditor;
using UnityEngine;

namespace Saving
{
    public class SavingSystemToolbar
    {
        [MenuItem(itemName: "Tools/Saving System/Open Save Location")]
        public static void OpenSaveLocation()
        {
            string dataPath = string.Format("{0}/{1}/", Application.persistentDataPath, SaveSettings.Get().saveFolder);

    #if UNITY_EDITOR_WIN
            dataPath = dataPath.Replace(@"/", @"\"); // Windows uses backward slashes
    #elif UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            dataPath = dataPath.Replace("\\", "/"); // Linux and MacOS use forward slashes
    #endif

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            EditorUtility.RevealInFinder(dataPath);
        }

        [MenuItem("Tools/Saving System/Open Save Settings")]
        public static void OpenSaveSystemSettings()
        {
            Selection.activeInstanceID = SaveSettings.Get().GetInstanceID();
        }
    }
}