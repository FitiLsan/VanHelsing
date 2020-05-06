using UnityEngine;


namespace BeastHunter
{
    public class GiantMudCrabProjectile
    {
        #region Fields

        public DamageStruct Damage;
        public GameObject Mud;
        public Rigidbody RB;

        #endregion


        #region ClassLifeCycle

        public GiantMudCrabProjectile(DamageStruct damage, Transform player, Transform CrabMouth, GameObject Prefab)
        {
            Damage = damage;
            Prefab.GetComponent<CrabPtojectileBehaviour>().Damage = Damage;
            Mud = GameObject.Instantiate(Prefab, CrabMouth.position, Quaternion.identity);
            RB = Mud.GetComponent<Rigidbody>();
            Vector3 forceVector = player.position - Mud.transform.position;
            RB.AddForce(forceVector * 100);
        }

        #endregion

    }
}


