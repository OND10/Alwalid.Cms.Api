using System.Collections.Concurrent;

namespace Alwalid.Cms.Api;

public class StateMachine<TState, TTrigger> where TState : notnull where TTrigger : notnull
{
    public sealed record Transition(TState From, TState To, TTrigger Trigger, Func<bool>? Guard = null, Action? OnTransition = null);

    private readonly ConcurrentDictionary<(TState, TTrigger), Transition> _map = new();

    public StateMachine<TState, TTrigger> Permit(TState from, TState to, TTrigger trigger, Func<bool>? guard = null, Action? onTransition = null)
    {
        var key = (from, trigger);
        if (_map.ContainsKey(key))
            throw new InvalidOperationException($"Transition from {from} on {trigger} is already defined.");

        var transition = new Transition(from, to, trigger, guard, onTransition);
        _map[key] = transition;
        return this;
    }
    
    public bool CanFire(TState currentState, TTrigger trigger)
    {
        var key = (currentState, trigger);
        if (_map.TryGetValue(key, out var transition))
        {
            return transition.Guard?.Invoke() ?? true;
        }
        return false;
    }

    public TState Fire(TState currentState, TTrigger trigger)
    {
        var key = (currentState, trigger);
        if (_map.TryGetValue(key, out var transition))
        {
            if (transition.Guard?.Invoke() ?? true)
            {
                transition.OnTransition?.Invoke();
                return transition.To;
            }
            throw new InvalidOperationException($"Guard condition for transition from {currentState} on {trigger} failed.");
        }
        throw new InvalidOperationException($"No transition defined from {currentState} on {trigger}.");
    }

    public IEnumerable<(TTrigger trigger, TState to)> GetPermittedTriggers(TState currentState) => _map
        .Where(kvp => EqualityComparer<TState>.Default.Equals(kvp.Key.Item1, currentState))
        .Select(kvp => (kvp.Key.Item2, kvp.Value.To));
}
