using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldEventListener : MonoBehaviour
{
    [Tooltip("Mismo id que se pasa a RaiseWorldEvent() en el Ink.")]
    [SerializeField] private string eventId;

    [Tooltip("Qué hacer cuando se recibe el evento.")]
    public UnityEvent OnEvent;

    private void OnEnable()  => WorldEventBus.Register(eventId, HandleEvent);
    private void OnDisable() => WorldEventBus.Unregister(eventId, HandleEvent);

    private void HandleEvent() => OnEvent?.Invoke();
}
