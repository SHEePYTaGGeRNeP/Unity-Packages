using System;

namespace Events
{
    public class Program
    {
        private enum CustomEnum
        {
            One,
            Two
        }

        // Added a new Enum with the same values to check if it all works.
        // It does :)
        public enum CustomEnum2
        {
            One
        }

        private static readonly PhysicsEngine _physicsEngine = new PhysicsEngine();
        private static readonly object _sender = new object();
        private static readonly EventLogger _ev1 = new EventLogger("1");
        private static readonly EventLogger _ev2 = new EventLogger("2");
        private static readonly EventLogger _ev3 = new EventLogger("3");
        
        private static void Main()
        {
            EventManager.Subscribe(_ev1, new Enum[] {PhysicsEngine.PhysicsEvents.Fall, PhysicsEngine.PhysicsEvents.Jump});
            EventManager.Subscribe(_ev2,
                new Enum[] {PhysicsEngine.PhysicsEvents.Fall, PhysicsEngine.PhysicsEvents.Jump, CustomEnum2.One});
            EventManager.Subscribe(_ev3, new Enum[] {CustomEnum.One, PhysicsEngine.PhysicsEvents.Fall});
            _physicsEngine.FirePhysicsEvent();
            EventManager.Notify(_sender, new EventManager.GenericEventArgs(CustomEnum.One, null, 1));
            EventManager.Notify(_sender, new EventManager.GenericEventArgs(CustomEnum2.One, null, 2));
            EventManager.Unsubscribe(_ev1, new Enum[] {PhysicsEngine.PhysicsEvents.Fall});
            EventManager.UnsubscribeFromAll(_ev2);
            Console.WriteLine("Unsubscribed");
            _physicsEngine.FirePhysicsEvent();
            EventManager.Notify(_sender, new EventManager.GenericEventArgs(CustomEnum.One, _ev1, "hello"));
            EventManager.Notify(_sender, new EventManager.GenericEventArgs(CustomEnum.Two));
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private class EventLogger : IOnEvent
        {
            private readonly string _text;

            public EventLogger(string text)
            {
                this._text = text;
            }

            public void OnEvent(object sender, EventManager.GenericEventArgs args)
            {
                Console.WriteLine(String.Format("{0} Sender: {1} Args: {2}", this._text, sender, args));
            }
        }
    }

    public class PhysicsEngine
    {
        public enum PhysicsEvents
        {
            Fall,
            Jump
        }

        //  shouldn't be public, but for the sake of ease it is.
        public void FirePhysicsEvent()
        {
            EventManager.Notify(this, new EventManager.GenericEventArgs(PhysicsEvents.Fall, null, "dead"));
            System.Threading.Thread.Sleep(250);
            EventManager.Notify(this, new EventManager.GenericEventArgs(PhysicsEvents.Jump, null, 10));
            System.Threading.Thread.Sleep(250);
        }
    }
}