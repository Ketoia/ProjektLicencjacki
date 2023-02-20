using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager //simple call
{
    private static Dictionary<Guid, UnityEvent> eventDictionary = new();

    public static void StartListening(Guid eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Guid eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Invoke(Guid eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent);
            thisEvent.Invoke();
        }
    }
}

public static class EventManager<T> // Call with parameters
{
    private static Dictionary<Guid, UnityEvent<T>> eventDictionaryWithParameters = new();

    public static void StartListening(Guid eventName, UnityAction<T> listener)
    {
        if (eventDictionaryWithParameters.TryGetValue(eventName, out UnityEvent<T> thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<T>();
            thisEvent.AddListener(listener);
            eventDictionaryWithParameters.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Guid eventName, UnityAction<T> listener)
    {
        if (eventDictionaryWithParameters.TryGetValue(eventName, out UnityEvent<T> thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Invoke(Guid eventName, T obj)
    {
        if (eventDictionaryWithParameters.ContainsKey(eventName))
        {
            eventDictionaryWithParameters.TryGetValue(eventName, out UnityEvent<T> thisEvent);
            thisEvent.Invoke(obj);
        }
    }
}
