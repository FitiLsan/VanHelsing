using System;
using UnityEngine;


namespace BeastHunter
{
    public interface ITrigger : IInteractable
    {
        Predicate<Collider> OnFilterHandler { get; set; }
        Action<ITrigger, Collider> OnTriggerEnterHandler { get; set; }
        Action<ITrigger, Collider> OnTriggerExitHandler { get; set; }
        Action<ITrigger, InteractableObjectType> DestroyHandler { get; set; }
        GameObject GameObject { get; }
        InteractableObjectType Type { get; }
    }
}
