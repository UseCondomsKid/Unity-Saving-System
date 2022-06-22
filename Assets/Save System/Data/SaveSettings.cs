using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "SaveSettings", menuName = "SO/Saving and Loading/Save Settings")]
public class SaveSettings : ScriptableObject
{
    private static SaveSettings Instance;

    [Header("_______________Saving System Settings_______________")]
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

        var saveSettings = Resources.Load("Dialogue Settings", typeof(SaveSettings)) as SaveSettings;

#if UNITY_EDITOR
        // In case the settings are not found, we create one
        if (saveSettings == null)
        {
            return CreateFile();
        }
#endif

        // In case it still doesn't exist, somehow it got removed.
        if (saveSettings == null)
        {
            Debug.LogWarning("Could not find SavePluginsSettings in resource folder, did you remove it? Using default settings.");
            saveSettings = ScriptableObject.CreateInstance<SaveSettings>();
        }

        Instance = saveSettings;

        return Instance;
    }

#if UNITY_EDITOR

    public static SaveSettings CreateFile()
    {
        string resourceFolderPath = string.Format("{0}/{1}", Application.dataPath, "Resources");
        string filePath = string.Format("{0}/{1}", resourceFolderPath, "Dialogue Settings.asset");

        // In case the directory doesn't exist, we create a new one.
        if (!Directory.Exists(resourceFolderPath))
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
        }

        // Check if the settings file exists in the resources path
        // If not, we create a new one.
        if (!File.Exists(filePath))
        {
            Instance = ScriptableObject.CreateInstance<SaveSettings>();
            UnityEditor.AssetDatabase.CreateAsset(Instance, "Assets/Resources/Dialogue Settings.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return Instance;
        }
        else
        {
            return Resources.Load("Dialogue Settings", typeof(SaveSettings)) as SaveSettings;
        }
    }
# endif
}
