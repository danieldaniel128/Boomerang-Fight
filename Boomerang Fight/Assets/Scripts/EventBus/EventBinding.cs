using System;

public interface IEventBinding<T>{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
    public bool UseLocal { get; } // New property for Photon ownership check
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent {
    Action<T> onEvent = _ => { };
    Action onEventNoArgs = () => { };
    public bool UseLocal { get; private set; } // Implementation of the new property
    Action<T> IEventBinding<T>.OnEvent {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventBinding<T>.OnEventNoArgs {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    public EventBinding(Action<T> onEvent, bool useLocal = false)
    {
        this.onEvent = onEvent;
        UseLocal = useLocal;
    }
    public EventBinding(Action onEventNoArgs, bool useLocal = false)
    {
        this.onEventNoArgs = onEventNoArgs;
        UseLocal = useLocal;
    }
    
    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;
    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;

}