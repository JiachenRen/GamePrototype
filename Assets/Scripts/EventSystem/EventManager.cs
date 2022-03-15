using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventSystem
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<Type, UnityEventBase> eventDictionary;

        private static EventManager _eventManager;

        private bool initialized = false;

        public static EventManager instance
        {
            get
            {
                if (_eventManager) return _eventManager;
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!_eventManager)
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                else
                    _eventManager.Init();

                return _eventManager;
            }
        }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance.initialized) {
                Destroy(gameObject);
            }

            initialized = true;
        }

        private void Init()
        {
            eventDictionary ??= new Dictionary<Type, UnityEventBase>();
        }


        public static void StartListening<T>(UnityAction listener) where T : UnityEvent, new()
        {
            if (instance.eventDictionary.TryGetValue(typeof(T), out var evt))
            {
                if (evt is T e)
                    e.AddListener(listener);
                else
                    LogError(typeof(T));
            }
            else
            {
                var e = new T();
                e.AddListener(listener);
                instance.eventDictionary.Add(typeof(T), e);
            }
        }


        public static void StartListening<T, T0>(UnityAction<T0> listener) where T : UnityEvent<T0>, new()
        {
            if (instance.eventDictionary.TryGetValue(typeof(T), out var evt))
            {
                if (evt is T e)
                    e.AddListener(listener);
                else
                    LogError(typeof(T));
            }
            else
            {
                var e = new T();
                e.AddListener(listener);
                instance.eventDictionary.Add(typeof(T), e);
            }
        }


        public static void StartListening<T, T0, T1>(UnityAction<T0, T1> listener)
            where T : UnityEvent<T0, T1>, new()
        {
            if (instance.eventDictionary.TryGetValue(typeof(T), out var evt))
            {
                if (evt is T e)
                    e.AddListener(listener);
                else
                    LogError(typeof(T));
            }
            else
            {
                var e = new T();
                e.AddListener(listener);
                instance.eventDictionary.Add(typeof(T), e);
            }
        }


        public static void StartListening<T, T0, T1, T2>(UnityAction<T0, T1, T2> listener)
            where T : UnityEvent<T0, T1, T2>, new()
        {
            if (instance.eventDictionary.TryGetValue(typeof(T), out var evt))
            {
                if (evt is T e)
                    e.AddListener(listener);
                else
                    LogError(typeof(T));
            }
            else
            {
                var e = new T();
                e.AddListener(listener);
                instance.eventDictionary.Add(typeof(T), e);
            }
        }


        public static void StartListening<T, T0, T1, T2, T3>(UnityAction<T0, T1, T2, T3> listener)
            where T : UnityEvent<T0, T1, T2, T3>, new()
        {
            if (instance.eventDictionary.TryGetValue(typeof(T), out var evt))
            {
                if (evt is T e)
                    e.AddListener(listener);
                else
                    LogError(typeof(T));
            }
            else
            {
                var e = new T();
                e.AddListener(listener);
                instance.eventDictionary.Add(typeof(T), e);
            }
        }


        public static void StopListening<T>(UnityAction listener) where T : UnityEvent
        {
            if (_eventManager == null)
                return;
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.RemoveListener(listener);
            else
                LogError(typeof(T));
        }

        public static void StopListening<T, T0>(UnityAction<T0> listener) where T : UnityEvent<T0>
        {
            if (_eventManager == null)
                return;
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.RemoveListener(listener);
            else
                LogError(typeof(T));
        }


        public static void StopListening<T, T0, T1>(UnityAction<T0, T1> listener) where T : UnityEvent<T0, T1>
        {
            if (_eventManager == null)
                return;
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.RemoveListener(listener);
            else
                LogError(typeof(T));
        }


        public static void StopListening<T, T0, T1, T2>(UnityAction<T0, T1, T2> listener)
            where T : UnityEvent<T0, T1, T2>
        {
            if (_eventManager == null)
                return;
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.RemoveListener(listener);
            else
                LogError(typeof(T));
        }


        public static void StopListening<T, T0, T1, T2, T3>(UnityAction<T0, T1, T2, T3> listener)
            where T : UnityEvent<T0, T1, T2, T3>
        {
            if (_eventManager == null)
                return;
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.RemoveListener(listener);
            else
                LogError(typeof(T));
        }


        public static void TriggerEvent<T>() where T : UnityEvent
        {
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.Invoke();
            else
                LogError(typeof(T));
        }

        public static void TriggerEvent<T, T0>(T0 t0) where T : UnityEvent<T0>
        {
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.Invoke(t0);
            else
                LogError(typeof(T));
        }


        public static void TriggerEvent<T, T0, T1>(T0 t0, T1 t1) where T : UnityEvent<T0, T1>
        {
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;
            if (evt is T e)
                e.Invoke(t0, t1);
            else
                LogError(typeof(T));
        }


        public static void TriggerEvent<T, T0, T1, T2>(T0 t0, T1 t1, T2 t2)
            where T : UnityEvent<T0, T1, T2>
        {
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;

            if (evt is T e)
                e.Invoke(t0, t1, t2);
            else
                LogError(typeof(T));
        }


        public static void TriggerEvent<T, T0, T1, T2, T3>(T0 t0, T1 t1, T2 t2, T3 t3)
            where T : UnityEvent<T0, T1, T2, T3>
        {
            if (!instance.eventDictionary.TryGetValue(typeof(T), out var evt)) return;

            if (evt is T e)
                e.Invoke(t0, t1, t2, t3);
            else
                LogError(typeof(T));
        }

        private static void LogError(System.Type errEvtType)
        {
            Debug.LogError("EventManager.TriggerEvent() FAILED! Event type " + errEvtType +
                           " could not be accessed for some strange reason.");
        }
    }
}