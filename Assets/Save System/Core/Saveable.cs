using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public class Saveable : MonoBehaviour
    {
        [SerializeField] private string id;
        public string Id => id;
        
# region Unity Functions

        private void Awake()
        {
            SaveManager.AddSaveable(this);
        }

        private void OnDestroy()
        {
            SaveManager.RemoveSaveable(this);            
        }
# endregion

        
# region Public Functions
        public void GenerateId()
        {
            id = Guid.NewGuid().ToString();
        }

        public object SaveState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveable in GetComponentsInChildren<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.SaveState();
            }

            return state;
        }

        public void LoadState(object _state)
        {
            if (_state == null) return;
            var state = (Dictionary<string, object>)_state;

            foreach (var saveable in GetComponentsInChildren<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();
                if (state.TryGetValue(typeName, out object savedState))
                {
                    saveable.LoadState(savedState);
                }
            }
        }
# endregion
    }
}
