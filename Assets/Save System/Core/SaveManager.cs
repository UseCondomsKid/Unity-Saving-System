using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Saving
{
    [System.Serializable]
    public class SaveSlot
    {
        public string slotName;
        public int currentSceneIndex;
        public string creationDate;
        public string timePlayed;
        // public string progress;
    }
    
    public static class SaveManager
    {
# region Properties

        private static List<Saveable> saveables;

        private static string path => Application.persistentDataPath + "/" + SaveSettings.Get().saveFolder + "/" + activeSaveSlot.slotName + SaveSettings.Get().saveFileExtension;
        private static SaveSlot activeSaveSlot = null;
        public static event Action OnSave;
        public static event Action OnLoad;
        public static event Action OnSwitchedSaveSlot;
# endregion

# region Public Functions
        public static void Save()
        {
            if (activeSaveSlot == null)
            {
                LogWarn("No Save Slot is currently active, Can't save");
                return;
            }
            
            // Saving States
            var state = LoadFile();
            SaveState(state);
            SaveFile(state);
            PlayerPrefs.Save();
            
            LogInfo("Game Saved in [" + activeSaveSlot.slotName + "]");
            
            OnSave?.Invoke();
        }

        public static void Load()
        {
            if (activeSaveSlot == null)
            {
                LogWarn("No Save Slot is currently active, Can't load");
                return;
            }
            
            // Loading States
            var state = LoadFile();
            LoadState(state);
            
            LogInfo("Game Loaded from [" + activeSaveSlot.slotName + "]");
            
            OnLoad?.Invoke();
        }

        public static void CreateSaveSlot(string _name)
        {
            SaveSlot slot = new SaveSlot();
            slot.slotName = _name;
            slot.creationDate = DateTime.Now.ToString();
            slot.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            slot.timePlayed = "00:00:00";

            var previousSlot = activeSaveSlot;
            activeSaveSlot = slot;

            var newState = new Dictionary<string, object>();
            newState[_name + SaveSettings.Get().saveFileExtension] = activeSaveSlot;
            SaveFile(newState);
            
            LogInfo("Save Slot [" + _name + "] Created!");

            activeSaveSlot = previousSlot;
        }

        public static void DeleteSaveSlot(string _name)
        {
            string p = Application.persistentDataPath + "/" + SaveSettings.Get().saveFolder + "/" + _name + SaveSettings.Get().saveFileExtension;
            File.Delete(p);
            
            LogInfo("Save Slot [" + _name + "] Deleted!");
        }

        public static List<SaveSlot> GetSaveSlots()
        {
            List<SaveSlot> slots = new List<SaveSlot>();
            var state = LoadFile();

            DirectoryInfo info =
                new DirectoryInfo(Application.persistentDataPath + "/" + SaveSettings.Get().saveFolder + "/");

            FileInfo[] files = info.GetFiles("*" + SaveSettings.Get().saveFileExtension);

            foreach (var slot in files)
            {
                if (state.TryGetValue(slot.Name, out object slotData))
                {
                    slots.Add((SaveSlot)slotData);
                }
            }

            return slots;
        }

        public static SaveSlot GetSaveSlot(string _name)
        {
            var state = LoadFile(_name);

            if (state.TryGetValue(_name + SaveSettings.Get().saveFileExtension, out object slotData))
            {
                return (SaveSlot) slotData;
            }
            else
            {
                return null;
            }
        }

        public static bool SaveSlotExists(string _name)
        {
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/" + SaveSettings.Get().saveFolder + "/");
            FileInfo[] files = info.GetFiles("*" + SaveSettings.Get().saveFileExtension);

            foreach (var slot in files)
            {
                if (_name + SaveSettings.Get().saveFileExtension == slot.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool SetActiveSaveSlot(string _name, bool _savePreviousSlot = true, bool _loadNewSlot = false)
        {
            SaveSlot slot = GetSaveSlot(_name);

            if (slot == null) return false;
            
            if (_savePreviousSlot) Save();

            activeSaveSlot = slot;
            OnSwitchedSaveSlot?.Invoke();
            
            LogInfo("Save Slot [" + _name + "] is now active!");
            
            if (_loadNewSlot) Load();

            return true;
        }

        public static void AddSaveable(Saveable _saveable)
        {
            if (saveables == null) saveables = new List<Saveable>();
            saveables.Add(_saveable);
            LogInfo("Saveable [" + _saveable.name + "] Has been registered");
        }

        public static void RemoveSaveable(Saveable _saveable)
        {
            if (saveables.Remove(_saveable))
            {
                LogInfo("Saveable [" + _saveable.name + "] Has been removed");
            }
        }
        
# region Player Prefs

        public static void DeleteKey(string _key)
        {
            PlayerPrefs.DeleteKey(_key + activeSaveSlot.slotName);
        }
        public static bool HasKey(string _key)
        {
            return PlayerPrefs.HasKey(_key + activeSaveSlot.slotName);
        }

        public static void SetInt(string _key, int _value)
        {
            PlayerPrefs.SetInt(_key + activeSaveSlot.slotName, _value);
        }
        public static int GetInt(string _key)
        {
            return PlayerPrefs.GetInt(_key + activeSaveSlot.slotName);
        }

        public static void SetFloat(string _key, float _value)
        {
            PlayerPrefs.SetFloat(_key + activeSaveSlot.slotName, _value);
        }
        public static float GetFloat(string _key)
        {
            return PlayerPrefs.GetFloat(_key + activeSaveSlot.slotName);
        }
        
        public static void SetString(string _key, string _value)
        {
            PlayerPrefs.SetString(_key + activeSaveSlot.slotName, _value);
        }
        public static string GetString(string _key)
        {
            return PlayerPrefs.GetString(_key + activeSaveSlot.slotName);
        }

# endregion
        

# endregion
        
# region Private Functions
        private static object SaveActiveSlotData()
        {
            activeSaveSlot.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            var timePlayed = TimeSpan.FromSeconds(Time.fixedUnscaledTimeAsDouble);
            TimeSpan timePlayedStored = new TimeSpan();
            var exists = TimeSpan.TryParse(activeSaveSlot.timePlayed, out timePlayedStored);
            if (exists) timePlayed = timePlayed.Add(timePlayedStored);
            activeSaveSlot.timePlayed = timePlayed.ToString();

            return activeSaveSlot;
        }
        
        private static void SaveFile(object _state)
        {
            using (var stream = File.Open(path, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, _state);
            }
        }

        private static Dictionary<string, object> LoadFile(string _slotName = "")
        {
            string p = "";
            if (_slotName != "")
            {
                p = Application.persistentDataPath + "/" + SaveSettings.Get().saveFolder + "/" + _slotName + SaveSettings.Get().saveFileExtension;
            }
            else
            {
                p = path;
            }
            
            if (!File.Exists(p))
            {
                LogWarn("No File was found at [" + p + "] When loading");
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(p, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private static void SaveState(Dictionary<string, object> _state)
        {
            foreach (var saveable in saveables)
            {
                _state[saveable.Id] = saveable.SaveState();
            }

            _state[activeSaveSlot.slotName + SaveSettings.Get().saveFileExtension] = SaveActiveSlotData();
        }
        
        private static void LoadState(Dictionary<string, object> _state)
        {
            foreach (var saveable in saveables)
            {
                if (_state.TryGetValue(saveable.Id, out object savedState))
                {
                    saveable.LoadState(savedState);
                }
            } 
        }
        
# region Debugging

    private static void LogInfo(string _msg)
    {
        if (!SaveSettings.Get().debug) return;
        Debug.Log("<< SaveManager >> || " + _msg);
    }
    private static void LogWarn(string _msg)
    {
        if (!SaveSettings.Get().debug) return;
        Debug.LogWarning("<< SaveManager >> || " + _msg);
    }
# endregion
        
        
# endregion
    }
}
