using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEventEnum
{
    Hand1Selected,
    Hand2Selected,
    ZoomValueChanged,
    EditModeChanged,
    PinchZoom
  }
public static class EventManager
{
    private static Dictionary<eEventEnum, Action<object>> eventTable = new Dictionary<eEventEnum, Action<object>>();
    
    public static void AddHandler(eEventEnum p_event, Action<object> p_action)
    {
        if(!eventTable.ContainsKey(p_event))
        {
            eventTable[p_event] = p_action;
        }
        else
        {
            eventTable[p_event] += p_action;
        }
    }

    public static void Broadcast(eEventEnum p_event, object p_value)
    {
        if(eventTable[p_event] != null)
        {
            eventTable[p_event](p_value);
        }
    }
}
