using System;

namespace GUIGUI17F.JsonModelCreator
{
    /// <summary>
    /// signal for events without parameters
    /// </summary>
    public class Signal
    {
        private event Action _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        public void Dispatch()
        {
            _signalEvent?.Invoke();
        }
    }

    /// <summary>
    /// signal for events with one parameter
    /// </summary>
    /// <typeparam name="T">type for the parameter</typeparam>
    public class Signal<T>
    {
        private event Action<T> _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action<T> listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action<T> listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        /// <param name="arg">signal parameter</param>
        public void Dispatch(T arg)
        {
            _signalEvent?.Invoke(arg);
        }
    }

    /// <summary>
    /// signal for events with two parameters
    /// </summary>
    /// <typeparam name="T0">type for parameter 0</typeparam>
    /// <typeparam name="T1">type for parameter 1</typeparam>
    public class Signal<T0, T1>
    {
        private event Action<T0, T1> _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action<T0, T1> listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action<T0, T1> listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        /// <param name="arg0">signal parameter 0</param>
        /// <param name="arg1">signal parameter 1</param>
        public void Dispatch(T0 arg0, T1 arg1)
        {
            _signalEvent?.Invoke(arg0, arg1);
        }
    }

    /// <summary>
    /// signal for events with three parameters
    /// </summary>
    /// <typeparam name="T0">type for parameter 0</typeparam>
    /// <typeparam name="T1">type for parameter 1</typeparam>
    /// <typeparam name="T2">type for parameter 2</typeparam>
    public class Signal<T0, T1, T2>
    {
        private event Action<T0, T1, T2> _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action<T0, T1, T2> listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action<T0, T1, T2> listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        /// <param name="arg0">signal parameter 0</param>
        /// <param name="arg1">signal parameter 1</param>
        /// <param name="arg2">signal parameter 2</param>
        public void Dispatch(T0 arg0, T1 arg1, T2 arg2)
        {
            _signalEvent?.Invoke(arg0, arg1, arg2);
        }
    }

    /// <summary>
    /// signal for events with four parameters
    /// </summary>
    /// <typeparam name="T0">type for parameter 0</typeparam>
    /// <typeparam name="T1">type for parameter 1</typeparam>
    /// <typeparam name="T2">type for parameter 2</typeparam>
    /// <typeparam name="T3">type for parameter 3</typeparam>
    public class Signal<T0, T1, T2, T3>
    {
        private event Action<T0, T1, T2, T3> _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action<T0, T1, T2, T3> listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action<T0, T1, T2, T3> listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        /// <param name="arg0">signal parameter 0</param>
        /// <param name="arg1">signal parameter 1</param>
        /// <param name="arg2">signal parameter 2</param>
        /// <param name="arg3">signal parameter 3</param>
        public void Dispatch(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            _signalEvent?.Invoke(arg0, arg1, arg2, arg3);
        }
    }

    /// <summary>
    /// signal for events with five parameters
    /// </summary>
    /// <typeparam name="T0">type for parameter 0</typeparam>
    /// <typeparam name="T1">type for parameter 1</typeparam>
    /// <typeparam name="T2">type for parameter 2</typeparam>
    /// <typeparam name="T3">type for parameter 3</typeparam>
    /// <typeparam name="T4">type for parameter 4</typeparam>
    public class Signal<T0, T1, T2, T3, T4>
    {
        private event Action<T0, T1, T2, T3, T4> _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action<T0, T1, T2, T3, T4> listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action<T0, T1, T2, T3, T4> listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        /// <param name="arg0">signal parameter 0</param>
        /// <param name="arg1">signal parameter 1</param>
        /// <param name="arg2">signal parameter 2</param>
        /// <param name="arg3">signal parameter 3</param>
        /// <param name="arg4">signal parameter 4</param>
        public void Dispatch(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _signalEvent?.Invoke(arg0, arg1, arg2, arg3, arg4);
        }
    }

    /// <summary>
    /// signal for events with six parameters
    /// </summary>
    /// <typeparam name="T0">type for parameter 0</typeparam>
    /// <typeparam name="T1">type for parameter 1</typeparam>
    /// <typeparam name="T2">type for parameter 2</typeparam>
    /// <typeparam name="T3">type for parameter 3</typeparam>
    /// <typeparam name="T4">type for parameter 4</typeparam>
    /// <typeparam name="T5">type for parameter 5</typeparam>
    public class Signal<T0, T1, T2, T3, T4, T5>
    {
        private event Action<T0, T1, T2, T3, T4, T5> _signalEvent;

        /// <summary>
        /// add signal listener
        /// </summary>
        public void AddListener(Action<T0, T1, T2, T3, T4, T5> listener)
        {
            if (null == _signalEvent)
            {
                _signalEvent += listener;
            }
            else if (!Array.Exists(_signalEvent.GetInvocationList(), invocation => invocation.Equals(listener)))
            {
                _signalEvent += listener;
            }
        }

        /// <summary>
        /// remove signal listener
        /// </summary>
        public void RemoveListener(Action<T0, T1, T2, T3, T4, T5> listener)
        {
            _signalEvent -= listener;
        }

        /// <summary>
        /// remove all signal listeners
        /// </summary>
        public void ClearListeners()
        {
            _signalEvent = null;
        }

        /// <summary>
        /// dispatch the signal
        /// </summary>
        /// <param name="arg0">signal parameter 0</param>
        /// <param name="arg1">signal parameter 1</param>
        /// <param name="arg2">signal parameter 2</param>
        /// <param name="arg3">signal parameter 3</param>
        /// <param name="arg4">signal parameter 4</param>
        /// <param name="arg5">signal parameter 5</param>
        public void Dispatch(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            _signalEvent?.Invoke(arg0, arg1, arg2, arg3, arg4, arg5);
        }
    }
}