using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingTest : MonoBehaviour
{
    private static SavingTest instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (!SaveManager.SaveSlotExists("save_1"))
        {
            SaveManager.CreateSaveSlot("save_1");
        }
        if (!SaveManager.SaveSlotExists("save_2"))
        {
            SaveManager.CreateSaveSlot("save_2");
        }
        
        SaveManager.SetActiveSaveSlot("save_1", false, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveManager.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveManager.Load();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveManager.SetActiveSaveSlot("save_1", false, true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveManager.SetActiveSaveSlot("save_2", false, true);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SaveManager.SetString("test", "Hello from the other side");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(SaveManager.GetString("test"));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(1);
        }
    }
}
