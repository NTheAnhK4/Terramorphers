using System;

using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    public static class ObserverManager<T> where T:Enum
    {
        private static Dictionary<T, Action<object>> _events = new Dictionary<T, Action<object>>();
       
        public static void Attach(T eventID,Action<object> callback)
        {
            if (callback == null)
            {
            
                Debug.LogWarning($"Callback for event {eventID.GetType().Name} is NULL.");
                return;
            }
    
            if (eventID == null)
            {
                Debug.LogWarning($"EventID is NULL. Cannot register event.");
                return;
            }
            if (!_events.TryAdd(eventID, callback))
            {
                _events[eventID] += callback;
            }
        }
    
        public static void Detach(T eventID,Action<object> callback)
        {
            if (_events.ContainsKey(eventID))
            {
                _events[eventID] -= callback;
                if (_events[eventID] == null)
                {
                    _events.Remove(eventID);
                }
            }
            else
            {
                Debug.LogWarning($"Event '{eventID}' not found in EventDispatcher_ "+typeof(T).Name);
            }
        }
    
        public static void Notify(T eventID, object param = null)
        {
            if (!_events.ContainsKey(eventID))
            {
                //Debug.LogWarning($"Event:{eventID.GetType().Name} " + eventID +"  has no Listener EventDispatcher_ "+typeof(T).Name);
                return;
            }
    
            if (_events[eventID] == null)
            {
                Debug.LogWarning($"Callback for event {eventID.GetType().Name} is NULL.");
                _events.Remove(eventID);
                return;
            }
            _events[eventID]?.Invoke(param);
        }
    
        public static void DetachAll()
        {
            _events.Clear();
        }
       
    }

}



