using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOne : MonoBehaviour, ISaveable
{
    [Serializable]
    private struct Data
    {
        public string name;
        public float health;
    }

    [SerializeField] private string _name;
    [SerializeField] private float health;
    
    
    public object SaveState()
    {
        var data = new Data();
        data.name = _name;
        data.health = this.health;

        return data;
    }

    public void LoadState(object state)
    {
        var data = (Data) state;
        _name = data.name;
        health = data.health;
    }
}
