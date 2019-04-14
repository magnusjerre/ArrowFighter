using UnityEngine;
using UnityEngine.EventSystems;

namespace Jerre.MainMenu
{
    public class PreventDeselectionGroup : MonoBehaviour
    {
        EventSystem evt;
        GameObject selected;
        
        void Start()
        {
            evt = EventSystem.current;
        }

        void Update()
        {
            if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != selected)
            {
                selected = evt.currentSelectedGameObject;
            } else
            {
                evt.SetSelectedGameObject(selected);
            }
        }
    }
}
