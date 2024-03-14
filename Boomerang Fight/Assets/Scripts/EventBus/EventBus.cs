using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class EventBus<T> where T : IEvent
{
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
    static bool IsMine => PlayerOwnershipManager.Instance.IsMyPlayer;
    public static void Register(EventBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(T @event)
    {
        foreach (var binding in bindings)
        {
            if (HasOnlineValidation(binding))
            {
                //invoke only to my player
                if (CanInvokeLocal(binding))
                {
                    //if(OnPlayerHealthChangedEvent is IEvent) doesnt work???
                    binding.OnEvent.Invoke(@event);
                    binding.OnEventNoArgs.Invoke();
                }
                else //invoke to all players
                {
                    binding.OnEvent.Invoke(@event);
                    binding.OnEventNoArgs.Invoke();
                }
            }
        }

        static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
        }
        static bool HasOnlineValidation(IEventBinding<T> binding)
        {
            // is connected and local
            if (PhotonNetwork.IsConnected)
            {
                // Access IsMine as a property
                return true;
            }
            // not connected or local
            return false;
        }
        static bool CanInvokeLocal(IEventBinding<T> binding)
        {
            if (binding.UseLocal)
                if (IsMine)
                    return true;
                // marked as to use local but is not local
                else
                    Debug.LogError($"UseLocal is {binding.UseLocal} but IsMine is {IsMine}");
            return false;
        }
    }
}