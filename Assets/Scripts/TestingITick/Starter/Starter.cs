using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    public List<ManagerBase> managers = new List<ManagerBase>();
    public GameObject Scene;

    void Awake()
    {
        foreach (var managerBase in managers)
        {
            Toolbox.Add(managerBase);
        }
        Scene.SetActive(true);
    }
}
