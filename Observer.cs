using System;
using System.Collections.Generic;
using UnityEngine;

namespace DATA.Scripts.Core
{
    public class Observer : Singleton<Observer>
    {
        private readonly Dictionary<string, List<Action>> _listeners = new Dictionary<string, List<Action>>();
        private readonly Dictionary<string, List<Action<List<object>>>> _listenersWithParam = new Dictionary<string, List<Action<List<object>>>>();
        public void RegisterObserver(string key, Action action)
        {
            List<Action> actions;
            if (_listeners.TryGetValue(key, out var listener))
            {
                actions = listener;
            }
            else
            {
                actions = new List<Action>();
                _listeners.Add(key, actions);
            }

            actions.Add(action);
        }
      
        public void RegisterObserver(string key, Action<List<object>> action)
        {
            List<Action<List<object>>> actions;
            if (_listenersWithParam.TryGetValue(key, out var listener))
            {
                actions = listener;
            }
            else
            {
                actions = new List<Action<List<object>>>();
                _listenersWithParam.Add(key, actions);
            }

            actions.Add(action);
        }
        
        public void NotifyObservers(string key, List<object> param)
        {
            if (_listenersWithParam.TryGetValue(key, out var listener))
            {
                foreach (Action<List<object>> a in listener)
                {
                    try
                    {
                        a?.Invoke(param);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("listener {0} not exist", key);
            }
        }

        public void NotifyObservers(string key)
        {
            if (_listeners.TryGetValue(key, value: out var listener))
            {
                foreach (Action a in listener)
                {
                    try
                    {
                        a?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("listener {0} not exist", key);
            }
        }
        
        public void RemoveObserver(string key, Action<List<object>> action)
        {
            if (_listenersWithParam.TryGetValue(key, out var listener))
            {
                listener.Remove(action);
            }
        }

        public void RemoveObserver(string key, Action action)
        {
            if (_listeners.TryGetValue(key, out var listener))
            {
                listener.Remove(action);
            }
        }
    }
}
