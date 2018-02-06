using System;
using System.Collections.Generic;
using System.Linq;

namespace Events
{
    public interface IOnEvent
    {
        void OnEvent(object sender, EventManager.GenericEventArgs args);
    }

    public class EventManager
    {
        public class GenericEventArgs : EventArgs
        {
            public Enum Event { get; private set; }
            public object Target { get; private set; }
            public object Value { get; private set; }

            public GenericEventArgs(Enum ev)
            {
                this.Event = ev;
            }

            public GenericEventArgs(Enum ev, object target, object value) : this(ev)
            {
                this.Target = target;
                this.Value = value;
            }

            public override string ToString()
            {
                return String.Format("{0} {1} {2}", this.Event.GetFullName(), this.Target, this.Value);
            }
        }

        private readonly Dictionary<Enum, EventHandler<GenericEventArgs>> _dict =
            new Dictionary<Enum, EventHandler<GenericEventArgs>>();

        private static readonly EventManager _instance = new EventManager();

        private EventManager()
        {
        }

        public static void Subscribe(IOnEvent listener, IEnumerable<Enum> events)
        {
            foreach (var e in events)
            {
                if (!_instance._dict.ContainsKey(e))
                    _instance._dict[e] = listener.OnEvent;
                else
                    _instance._dict[e] += listener.OnEvent;
            }
        }

        public static void Unsubscribe(IOnEvent listener, IEnumerable<Enum> events)
        {
            foreach (var e in events)
            {
                if (!_instance._dict.ContainsKey(e)) continue;

                _instance._dict[e] -= listener.OnEvent;
                if (_instance._dict[e] == null)
                    _instance._dict.Remove(e);
            }
        }

        public static void UnsubscribeFromAll(IOnEvent listener)
        {
            List<Enum> enums = new List<Enum>();
            foreach (KeyValuePair<Enum, EventHandler<GenericEventArgs>> kvp in _instance._dict)
            {
                if (kvp.Value.GetInvocationList().Any(subscriber => subscriber.Target == listener))
                    enums.Add(kvp.Key);
            }
            Unsubscribe(listener, enums);
        }

        public static void Notify(object sender, GenericEventArgs args)
        {
            if (!_instance._dict.ContainsKey(args.Event)) return;
            _instance._dict[args.Event].Invoke(sender, args);
        }
    }

    public static class Extensions
    {
        public static string GetFullName(this Enum myEnum)
        {
            return String.Format("{0}.{1}", myEnum.GetType().Name, myEnum.ToString());
        }
    }
}