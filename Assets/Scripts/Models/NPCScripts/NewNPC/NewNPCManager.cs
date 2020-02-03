using UnityEngine;
using Models;


public class NewNPCManager : MonoBehaviour
{
    [SerializeField] private NewGuardianModel[] _guardianPull;
    private NewNPCController[] _npcControllers;

    public void Start()
    {
        _npcControllers = new NewNPCController[_guardianPull.Length];

        for (int i = 0; i < _npcControllers.Length; i++)
        {
            var prefab = Instantiate(_guardianPull[i].NPCPrefab, _guardianPull[i].NPCPosition, Quaternion.identity);     
            _npcControllers[i] = new NewNPCController();
            _npcControllers[i].Init(_guardianPull[i], prefab);
        }
    }

    public void Update()
    {
        foreach (var controller in _npcControllers)
        {
            controller.Tick();
        }
    }
}
