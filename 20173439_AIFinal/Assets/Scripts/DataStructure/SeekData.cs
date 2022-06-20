using UnityEngine;

namespace DataStructure
{
    [System.Serializable]
    public class SeekData
    {
        public float MaxAcceleration;
        public float maxSpeed;
        [HideInInspector]
        public Vector3 CharacterPosition;
        [HideInInspector]
        public Vector3 TargetPosition;
    }
}