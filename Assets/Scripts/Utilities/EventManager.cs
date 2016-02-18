/*
    Use this only when there is one way message passing to many recivers quicky
    Order is not improtant
    its just data being sent around

    For cases where order is improtant, and there is execution of other function to control, use command pattern

    Register event : 
    EventManager.RegisterEvent (EventManager.eGameEvents.LoginCompleted, OnLoginCompleted);

    De-Register event :
    EventManager.DeRegisterEvent (EventManager.eGameEvents.LoginCompleted, OnLoginCompleted);

    Trigger event :
    EventManager.TriggerEvent(EventManager.eGameEvents.LoginCompleted,false,0,"name","Id");

    Callback function
    void OnLoginCompleted(object[] a_arrObj)
	{		
		bool result = (bool)a_arrObj [0];
		int errorCode = (int)a_arrObj [1];
        string userName = (string)a_arrObj [2];
        string id = (string)a_arrObj [3];
	}    



    Author - Rohit Bhosle
*/
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourInject
{
    public class EventManager
    {
        public enum eEvents
        {
            //LoginCompleted = 5002,        Example
        };        

        private EventManager()
        {
            _dicEventRegistry = new Dictionary<eEvents, EventDelegate>();
        }

        public delegate void EventDelegate(params object[] args);
        Dictionary<eEvents, EventDelegate> _dicEventRegistry;

        public void RegisterEvent(eEvents @event, EventDelegate delListener)
        {
            if (!_dicEventRegistry.ContainsKey(@event))
            {
                _dicEventRegistry.Add(@event, delListener);
                return;
            }

            _dicEventRegistry[@event] += delListener;

        }

        public void DeRegisterEvent(eEvents @event, EventDelegate _delListener)
        {
            if (!_dicEventRegistry.ContainsKey(@event))
                return;

            _dicEventRegistry[@event] -= _delListener;
        }

        public void TriggerEvent(eEvents @event, params object[] args)
        {
            string strEventKey = @event.ToString();
            EventDelegate d;

            if (!_dicEventRegistry.TryGetValue(@event, out d)) return;            
            if (d != null)
                d(args);
            else
                Debug.LogError("Could not trigger event: " + strEventKey);
        }

    }
}
