using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayerClass : MonoBehaviour, ISaveable
{
    [Serializable]
    public struct Data
    {
        public int _health;
        public float _speed;
    }

    public int health;
    public float speed;

    // This function will be called when we save the game
    public object SaveState()
    {
        // Create a new Data
        var data = new Data();
        // Fill it up with the current state of our class
        data._health = health;
        data._speed = speed;

        // We return the object to be saved
        return data;
    }

    // This function will be called when we load the game
    // This functions will return the data we stored from this instance of our ExamplePlayerClass
    public void LoadState(object state)
    {
        // Get the data, by explicitly casting it to our Data struct
        var data = (Data)state;
        // Get the saved data
        health = data._health;
        speed = data._speed;
    }
}
