using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "SaveSettings", menuName = "Saving/Save Settings")]
public class SaveSettings : ScriptableObject
{
    private static SaveSettings Instance;

    [Header("Saving System Settings")]
    public string saveFolder = "saves";
    public string saveFileExtension = ".ligma";
    public bool debug = true;


    private void OnDestroy()
    {
        Instance = null;
    }


    public static SaveSettings Get()
    {
        if (Instance != null)
        {
            return Instance;
        }

        var saveSettings = Resources.Load("Save Settings", typeof(SaveSettings)) as SaveSettings;

#if UNITY_EDITOR
        // In case the settings are not found, we create one
        if (saveSettings == null)
        {
            saveSettings = CreateFile();
        }
#endif

        // In case it still doesn't exist, somehow it got removed?
        if (saveSettings == null)
        {
            Debug.LogWarning("Could not find the Save Settings asset in resource folder, did you remove it? Going to use default settings.");
            saveSettings = ScriptableObject.CreateInstance<SaveSettings>();
        }


        // Making sure that the save folder is created.
        string saveFolderPath = string.Format("{0}/{1}/", Application.persistentDataPath, saveSettings.saveFolder);
#if UNITY_STANDALONE_WIN
        saveFolderPath = saveFolderPath.Replace(@"/", @"\"); // Windows uses backward slashes
#else
            dataPath = dataPath.Replace("\\", "/"); // Linux and MacOS use forward slashes
#endif
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // Set the instance, and return.
        Instance = saveSettings;
        return Instance;
    }

#if UNITY_EDITOR
    public static SaveSettings CreateFile()
    {
        string resourceFolderPath = string.Format("{0}/{1}", Application.dataPath, "Resources");
        string filePath = string.Format("{0}/{1}", resourceFolderPath, "Save Settings.asset");

        // In case the directory doesn't exist, we create a new one.
        if (!Directory.Exists(resourceFolderPath))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Check if the settings file exists in the resources path
        // If not, we create a new one.
        if (!File.Exists(filePath))
        {
            var instance = ScriptableObject.CreateInstance<SaveSettings>();
            UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/Save Settings.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            return instance;
        }
        else
        {
            return Resources.Load("Save Settings", typeof(SaveSettings)) as SaveSettings;
        }
    }
# endif
}
