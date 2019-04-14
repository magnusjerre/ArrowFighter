using UnityEngine;
using System.Collections.Generic;

namespace Jerre.Events
{
    public class AFEventManager
    {
        private static AFEventManager instance;
        public static AFEventManager INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new AFEventManager();
                }
                return instance;
            }
        }

        private List<IAFEventListener> listeners = new List<IAFEventListener> ();

        public void PostEvent(AFEvent afEvent)
        {
            for (var i = 0; i < listeners.Count; i++)
            {
                var listener = listeners[i];
                if (listener.HandleEvent(afEvent))
                {
                    return;
                }
            }
        }
        
        public bool AddListener(IAFEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
                return true;
            }
            return false;
        }

        public bool RemoveListener(IAFEventListener listener)
        {
            return listeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            listeners.Clear();
        }
    }
}
