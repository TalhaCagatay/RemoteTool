using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Madbox
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;
        private List<MainThreadCallback> _callbackList = new List<MainThreadCallback>();
        private List<MainThreadCallback> _executedListBuffer = new List<MainThreadCallback>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateDispatcher()
        {
            var mainThreadDispatcher = new GameObject("MainThreadDispatcher").AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(mainThreadDispatcher);
            mainThreadDispatcher.hideFlags = HideFlags.HideInHierarchy;
        }
        
        private class MainThreadCallback
        {
            private float _delay;
            private bool _useScaledTime;
            private Action _action;

            public MainThreadCallback(float delay, bool useScaledTime, Action action)
            {
                _delay = delay;
                _useScaledTime = useScaledTime;
                _action = action;
            }

            public bool Process()
            {
                _delay -= (_useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime);
                if (_delay <= 0)
                {
                    try
                    {
                        _action();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[SURF-ERROR] {e.Message}, Stack Trace: {e.StackTrace}\n{e}");
                    }

                    return true;
                }
                return false;
            }
        }

        private bool _isPaused = false;

        public bool IsPaused
        {
            get => _isPaused;
            private set
            {
                if(_isPaused != value)
                    _isPaused = value;
            }
        }

        public static void AddMainThreadCallback(Action callback, float delay = 0f, bool useScaledTime = false)
        {
            lock (_addBuffer)
            {
                if (callback != null)
                {
                    _addBuffer.Add(new MainThreadCallback(delay, useScaledTime, callback));
                }
            }
        }
        
        public Coroutine DoAfter(Func<bool> condition, Action callback)
        {
            return StartCoroutine(DoAfterHelper(condition, callback));
        }

        private IEnumerator DoAfterHelper(Func<bool> condition, Action callback)
        {
            yield return new WaitUntil(condition);
            callback?.Invoke();
        }

        private static List<MainThreadCallback> _addBuffer = new List<MainThreadCallback>();

        public void Update()
        {
            _executedListBuffer.Clear();
            foreach (var cb in _callbackList)
            {
                try
                {
                    if (cb != null && cb.Process())
                        _executedListBuffer.Add(cb);
                }
                catch(Exception e)
                {
                    Debug.LogError($"[SURF-ERROR] {e.Message}, Stack Trace: {e.StackTrace}\n{e}");
                }
            }
            _callbackList.RemoveAll((obj) => _executedListBuffer.Contains(obj));
            _callbackList.AddRange(_addBuffer);
            _addBuffer.Clear();
        }

        private void OnApplicationFocus(bool hasFocus) => IsPaused = !hasFocus;

        private void OnApplicationPause(bool pauseStatus) => IsPaused = pauseStatus;

        // TODO: We assume app is going to a pause state since they
        // are both the same for the PlatformSDK for the moment.
        // We might want to separate Quit from Pause/Focus.
        private void OnApplicationQuit() => IsPaused = true;
    }
}