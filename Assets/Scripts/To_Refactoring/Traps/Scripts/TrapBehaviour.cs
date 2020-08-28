using UnityEngine;


namespace BeastHunter
{
    public sealed class TrapBehaviour : MonoBehaviour
    {
        #region Fields
        
        public GameObject Projectile;
        public Transform ProjectilePlace;
        public int Type;
        public bool IsActive;

        private SphereCollider _sphereCollider;
        private GameContext _context;
        private Animator _animator;
        private int _charge = 1;
        private bool _isTargeting = false;
        private Transform _target;

        #endregion


        #region UnityMethods

        private void OnTriggerEnter(Collider other)
        {
            if(!other.isTrigger)
            {
                if (other.transform.CompareTag(TagManager.ENEMY) && Type == 0)
                {
                    if (_charge > 0)
                    {
                        _context.NpcModels[other.gameObject.GetInstanceID()].TakeDamage(
                            Services.SharedInstance.AttackService.CountDamage(
                                _context.TrapModels[this.gameObject.GetInstanceID()].TrapStruct.Damage,
                                    _context.NpcModels[other.gameObject.GetInstanceID()].GetStats().MainStats));

                        _animator.Play("BearTrapLock");
                        _charge -= 1;

                        other.transform.position = this.transform.position;

                        _context.TrapModels.Remove(this.gameObject.GetInstanceID());
                        Destroy(gameObject, 2f);
                    }
                    else
                    {
                        _context.TrapModels.Remove(this.gameObject.GetInstanceID());
                        Destroy(gameObject, 2f);
                    }
                }
                if (other.transform.CompareTag(TagManager.ENEMY) && Type == 1)
                {
                    if (_charge > 0)
                    {
                        _target = other.transform;
                        _isTargeting = true;
                        gameObject.GetComponent<Animation>().Play();
                        TimeRemaining launch = new TimeRemaining(LaunchAcid, 0.7f);
                        launch.AddTimeRemaining(0.7f);

                        _charge -= 1;
                        _context.TrapModels.Remove(this.gameObject.GetInstanceID());
                        Destroy(gameObject, 2f);
                    }
                    else
                    {
                        _context.TrapModels.Remove(this.gameObject.GetInstanceID());
                        Destroy(gameObject, 2f);
                    }
                }
            }           
        }

        private void Update()
        {
            if (_isTargeting)
            {
                transform.LookAt(_target);
            }
            if (IsActive)
            {
                if(_sphereCollider.enabled == false)
                {
                    SetActive();
                }
            }
        }

        #endregion


        #region Methods

        public void Init(TrapModel model, GameContext context)
        {
            IsActive = false;
            _context = context;
            _animator = gameObject.GetComponent<Animator>();
            _sphereCollider = gameObject.GetComponent<SphereCollider>();
            _sphereCollider.enabled = false;
            
            if (model.TrapData.TrapType == TrapsEnum.AcidCatapult)
            {
                foreach (Transform place in transform.GetComponentsInChildren<Transform>())
                {
                    if (place.name == "Acid") ProjectilePlace = place;
                }
            }

            else if (transform.childCount != 0)
            {
                ProjectilePlace = transform.GetChild(0);
            }
        }

        private void LaunchAcid()
        {
            GameObject _acid = Instantiate(Projectile, ProjectilePlace.position, Quaternion.identity);

            _acid.GetComponent<Rigidbody>().AddForce((_target.position - _acid.transform.position + Vector3.up * 3f) * 2f, 
                ForceMode.Impulse);
            _acid.GetComponent<ProjectileScript>().Context = _context;
            Destroy(ProjectilePlace.gameObject);
        }

        private void ChangeMaterial(Transform t)
        {
            string Path = t.GetComponent<MeshRenderer>().material.name.Remove(t.GetComponent<MeshRenderer>().material.name.Length - 11);
            t.GetComponent<MeshRenderer>().material = Resources.Load("Materials/" + Path + "N") as Material;
        }
        private void ChangeMaterialSkinned(Transform t)
        {
            string Path = t.GetComponent<SkinnedMeshRenderer>().material.name.Remove(t.GetComponent<SkinnedMeshRenderer>().material.name.Length - 11);
            t.GetComponent<SkinnedMeshRenderer>().material = Resources.Load("Materials/" + Path + "N") as Material;
        }
        private void SetActive()
        {
            foreach (Transform child in this.transform)
            {
                if (child.GetComponent<MeshRenderer>() != null)
                {
                    ChangeMaterial(child);
                }
                if (child.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    ChangeMaterialSkinned(child);
                }
                foreach (Transform childchild in child)
                {
                    if (childchild.GetComponent<MeshRenderer>() != null)
                    {
                        ChangeMaterial(childchild);
                    }
                    if (childchild.GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        ChangeMaterialSkinned(childchild);
                    }
                    foreach (Transform ccc in childchild)
                    {
                        if (ccc.GetComponent<MeshRenderer>() != null)
                        {
                            ChangeMaterial(ccc);
                        }
                        if (ccc.GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            ChangeMaterialSkinned(ccc);
                        }
                    }
                }
            }
            _sphereCollider.enabled = true;
        }
        #endregion
    }
}
