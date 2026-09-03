using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to the EventSystem in Juego.unity.
/// On Awake: disables every other EventSystem found (e.g. StoryScene's).
/// On Destroy: re-enables them so StoryScene works normally after Juego unloads.
/// </summary>
[RequireComponent(typeof(EventSystem))]
public class SingletonEventSystem : MonoBehaviour
{
    private EventSystem[] _suppressed;

    void Awake()
    {
        EventSystem mine = GetComponent<EventSystem>();
        EventSystem[] all = FindObjectsOfType<EventSystem>();

        _suppressed = System.Array.FindAll(all, s => s != mine);
        foreach (EventSystem sys in _suppressed)
            sys.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (_suppressed == null) return;
        foreach (EventSystem sys in _suppressed)
            if (sys != null)
                sys.gameObject.SetActive(true);
    }
}
