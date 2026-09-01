using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent _action;

    public void Interact()
    {
        _action?.Invoke();
    }
}
