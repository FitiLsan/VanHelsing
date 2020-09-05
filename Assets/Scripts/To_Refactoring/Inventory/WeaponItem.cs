using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateItem/CreateWeapon", order = 0)]
    public sealed class WeaponItem : BaseItem
    {
        #region Fields

        public WeaponHandType WeaponHandType;
        public WeaponType WeaponType;

        public GameObject WeaponPrefab;
        public GameObject WeaponObject;

        public Vector3 PrefabPositionInHand;
        public Vector3 PrefabRotationInHand;

        [Tooltip("Weapon weight between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float Weight;

        [Tooltip("Time needed to get the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TimeToGet;

        [Tooltip("Time needed for weapon to appear between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TimeToAppear;

        [Tooltip("Time needed to remove the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TimeToRemove;

        [Tooltip("Time needed for weapon to disappear between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TimeToDisappear;

        public AttackModel CurrentAttack;
        public AttackModel[] AttacksLeft;
        public AttackModel[] AttacksRight;
        public AttackModel SpecialAttackLeft;
        public AttackModel SpecialAttackRight;

        public string SimpleAttackAnimationName;
        public string SpecialAttackAnimationName;

        public int SimpleAttackFromLeftkAnimationHash { get { return Animator.StringToHash(SimpleAttackAnimationName + "Left"); } }
        public int SimpleAttackFromRightkAnimationHash { get { return Animator.StringToHash(SimpleAttackAnimationName + "Right"); } }
        public int SpecialAttackFromLeftAnimationHash { get { return Animator.StringToHash(SpecialAttackAnimationName + "Left"); } }
        public int SpecialAttacFromRightkAnimationHash { get { return Animator.StringToHash(SpecialAttackAnimationName + "Right"); } }
        public int GettingAnimationHash { get { return Animator.StringToHash("Getting" + ItemStruct.Name); } }
        public int RemovingAnimationHash { get { return Animator.StringToHash("Removing" + ItemStruct.Name); } }

        #endregion
    }
}


