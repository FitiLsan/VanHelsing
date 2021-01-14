using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "WeakPoint", menuName = "Enemy/BossData")]
    public sealed class WeakPointData : ScriptableObject
    {
        public string Name;
        public GameObject InstancePrefab;
        
        public Vector3 PrefabLocalPosition;
        public Vector3 PrefabLocalEulers;

        public Damage AdditionalDamage;
    }
}

