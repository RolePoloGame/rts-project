using NaughtyAttributes;
using System;
using UnityEngine;

namespace RTS.Core
{
    [System.Serializable]
    public class UniqueID
    {
        public UniqueID()
        {
            ID = Guid.NewGuid().ToString();
        }
        public UniqueID(string id)
        {
            ID = id;
        }

        [field: SerializeField, ReadOnly]
        public string ID { get; set; }
    }
}
