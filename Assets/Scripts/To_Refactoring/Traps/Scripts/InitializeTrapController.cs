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

        public InitializeTrapController(GameContext context, TrapData _trapData)
        {
            _context = context;
            this.OnAwake(_trapData);
        }

        #endregion


        #region IAwake

        public void OnAwake(TrapData trapData)
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

            TrapModel TrapModel = new TrapModel(instance, _trapData);
            _context.TrapModels.Add(instance.GetInstanceID(), TrapModel);
            instance.GetComponent<TrapBehaviour>().Init(TrapModel, _context);

            Collider[] instanceColliders = instance.GetComponents<Collider>();

            foreach (Collider collider in instanceColliders)
            {
                if(collider.isTrigger)
                {
                    if(collider.GetType() == typeof(CapsuleCollider))
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
