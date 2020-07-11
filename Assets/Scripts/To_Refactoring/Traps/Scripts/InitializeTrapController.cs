using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class InitializeTrapController
    {
        #region Field

        GameContext _context;
        private Transform _camera;
        RaycastHit hit;
        private Vector3 SpawnTrapPosition;

        #endregion


        #region ClassLifeCycle

        public InitializeTrapController(GameContext context, TrapData _trapData)
        {
            _context = context;
            this.OnAwake(_trapData);
        }

        #endregion


        #region IAwake

        public void OnAwake(TrapData _trapData)
        {
            _camera = Services.SharedInstance.CameraService.CharacterCamera.transform;
            Physics.Raycast(_camera.position, _camera.TransformDirection(Vector3.forward), 100, 9);
            Debug.DrawRay(_camera.position, _camera.TransformDirection(Vector3.forward), Color.yellow, 5);
            if (Physics.Raycast(_camera.position, _camera.TransformDirection(Vector3.forward),out hit, 150, 9))
            {
                if(hit.transform.tag == "Ground")
                {
                    SpawnTrapPosition = hit.point;
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
                var TrapData = _trapData;
            //Vector3 spawnPoint = Vector3.zero;
            //Services.SharedInstance.PhysicsService.FindGround(SpawnTrapPosition, out spawnPoint);
            GameObject instance = GameObject.Instantiate(TrapData.TrapStruct.Prefab, SpawnTrapPosition, Quaternion.identity);
            TrapModel TrapModel = new TrapModel(instance, TrapData);
            _context.TrapModel = TrapModel;
            instance.GetComponent<TrapBehaviour>().TrapStruct = TrapData.TrapStruct;
            //_context.AddTriggers(instance.GetComponent<InteractableObjectBehavior>().Type, instance.GetComponent<InteractableObjectBehavior>());
        }

        #endregion
    }
}
