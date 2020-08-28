using UnityEngine;


namespace BeastHunter
{
    public sealed class InitializeTrapController
    {
        #region Field

        GameContext _context;
        private Transform _camera;
        RaycastHit hit;
        private Vector3 SpawnTrapPosition;
        private TrapData _trapData;

        #endregion


        #region ClassLifeCycle

        public InitializeTrapController(GameContext context, TrapData trapData, bool isActive = false)
        {
            _context = context;
            if(_context.TrapModels.Count < 5)
            {
                SpawnAtLookPoint(trapData, isActive);
            }
        }

        public InitializeTrapController(GameContext context, TrapData trapData, Vector3 spawnPoint, Vector3 spawnEulers, 
            bool isActive = false)
        {
            _context = context;
            if (_context.TrapModels.Count < 5)
            {
                SpawnAtGivenPoint(trapData, spawnPoint, spawnEulers, isActive);
            }
        }

        #endregion


        #region IAwake

        private void SpawnAtGivenPoint(TrapData trapData, Vector3 spawnPoint, Vector3 spawnEulers, bool isActive)
        {
            _trapData = trapData;
            
            GameObject instance = GameObject.Instantiate(_trapData.TrapStruct.Prefab);

            instance.transform.eulerAngles = spawnEulers;
            instance.transform.position = spawnPoint;

            TrapModel trapModel = new TrapModel(instance, _trapData);
            _context.TrapModels.Add(instance.GetInstanceID(), trapModel);

            TrapBehaviour behavior = instance.GetComponent<TrapBehaviour>();
            behavior.Init(trapModel, _context);
            behavior.IsActive = isActive;

            InitColliders(instance);
        }

        private void SpawnAtLookPoint(TrapData trapData, bool isActive)
        {
            _trapData = trapData;
            _camera = Services.SharedInstance.CameraService.CharacterCamera.transform;
            Physics.Raycast(_camera.position, _camera.TransformDirection(Vector3.forward), 100, 9);
            Debug.DrawRay(_camera.position, _camera.TransformDirection(Vector3.forward), Color.yellow, 5);

            if (Physics.Raycast(_camera.position, _camera.TransformDirection(Vector3.forward), out hit, 150, 9))
            {
                if (hit.transform.tag == "Ground")
                {
                    SpawnTrapPosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(hit.point);
                    Debug.Log(SpawnTrapPosition);
                }
                else
                {
                    Debug.Log(hit.transform.tag + " It's not a ground");
                    return;
                }
            }
            else
            {
                Debug.Log(hit + " Not found any target");
                return;
            }

            GameObject instance = GameObject.Instantiate(_trapData.TrapStruct.Prefab, SpawnTrapPosition, Quaternion.identity);

            Vector3 cameraRotation = _camera.transform.eulerAngles;
            instance.transform.eulerAngles = new Vector3(0f, cameraRotation.y, 0f);
            instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y +
                _trapData.TrapStruct.HeightPlacing, instance.transform.position.z);

            TrapModel trapModel = new TrapModel(instance, _trapData);
            _context.TrapModels.Add(instance.GetInstanceID(), trapModel);

            TrapBehaviour behavior = instance.GetComponent<TrapBehaviour>();
            behavior.Init(trapModel, _context);
            behavior.IsActive = isActive;

            InitColliders(instance);
        }

        private void InitColliders(GameObject instance)
        {
            Collider[] instanceColliders = instance.GetComponents<Collider>();

            foreach (Collider collider in instanceColliders)
            {
                if (collider.isTrigger)
                {
                    if (collider.GetType() == typeof(CapsuleCollider))
                    {
                        (collider as CapsuleCollider).radius = _trapData.TrapStruct.Duration;
                    }
                    else if (collider.GetType() == typeof(SphereCollider))
                    {
                        (collider as SphereCollider).radius = _trapData.TrapStruct.Duration;
                    }
                    else if (collider.GetType() == typeof(BoxCollider))
                    {
                        (collider as BoxCollider).size = new Vector3(_trapData.TrapStruct.Duration,
                            _trapData.TrapStruct.Duration, _trapData.TrapStruct.Duration);
                    }
                }
            }
        }

        #endregion
    }
}
