using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class EventBus<T> where T : IEvent {
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
    static bool IsMine => GetIsMine();
    public static void Register(EventBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(T @event) {
        foreach (var binding in bindings) 
        {
            if (HasOnlineValidation(binding))
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
            //else
            //{
            //    binding.OnEvent.Invoke(@event);
            //    binding.OnEventNoArgs.Invoke();
            //}
        }
    }

    static void Clear() {
        Debug.Log($"Clearing {typeof(T).Name} bindings");
        bindings.Clear();
    }
    static bool HasOnlineValidation(IEventBinding<T> binding)
    {
        // is connected and local
        if (PhotonNetwork.IsConnected && binding.UseLocal)
        {
            // Access IsMine as a property
            if (IsMine)
                return true;
            else
            {
                // marked as to use local but is not local
                Debug.LogError($"UseLocal is {binding.UseLocal} but IsMine is {IsMine}");
                return false;
            }
        }
        // not connected or local
        return false;
    }
    // Method to check if the local player owns the object
    static bool GetIsMine()
    {
        var ownershipManager = PlayerOwnershipManager.Instance;
        if (ownershipManager == null)
        {
            Debug.LogError("PlayerOwnershipManager is not initialized.");
            return false;
        }

        var playerOwnershipMap = ownershipManager.PlayerOwnershipMap;
        var localPlayerActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        
        return playerOwnershipMap.ContainsKey(localPlayerActorNumber) && playerOwnershipMap[localPlayerActorNumber];
    }
}