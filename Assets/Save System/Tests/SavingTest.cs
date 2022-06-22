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
        if (!SaveManager.SaveSlotExists("deez_nuts"))
        {
            SaveManager.CreateSaveSlot("deez_nuts");
        }
        
        SaveManager.SetActiveSaveSlot("deez_nuts", false, false);
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
            SaveManager.SetActiveSaveSlot("deez_nuts", false, true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!SaveManager.SaveSlotExists("amen"))
            {
                SaveManager.CreateSaveSlot("amen");
            }
            SaveManager.SetActiveSaveSlot("amen", false, true);
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
