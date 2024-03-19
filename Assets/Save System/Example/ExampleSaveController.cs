using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleSaveController : MonoBehaviour
{
    private static ExampleSaveController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveManager.SetActiveSaveSlot("save_1", false, true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SaveManager.SetActiveSaveSlot("save_2", false, true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveManager.SetString("test", "Hello World");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(SaveManager.GetString("test"));
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(1);
        }
    }
}
