using System;
using System.Collections.Generic;

namespace InfrastructureLight.Wpf.Common.Messaging
{
    public class Messenger : IMessenger
    {
        private Dictionary<Type, List<Action<IMessage>>> _subscribers
            = new Dictionary<Type, List<Action<IMessage>>>();
        
        private static readonly object _locked = new object();

        public void Register<TMessage>(Action<TMessage> action) where TMessage : IMessage
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            Action<IMessage> concreteAction = m => action((TMessage)m);
            Type messageType = typeof(TMessage);

            if (_subscribers.ContainsKey(messageType)) {
                if (_subscribers[messageType] != null) {
                    _subscribers[messageType].Add(concreteAction);
                }
                else {
                    _subscribers[messageType] = new List<Action<IMessage>> { concreteAction };
                }
            }
            else {
                _subscribers.Add(messageType, new List<Action<IMessage>> { concreteAction });
            }
        }
        public void Send<TMessage>(TMessage message) where TMessage : IMessage
        {
            Type messageType = typeof(TMessage);
            if (_subscribers.ContainsKey(messageType)) {
                foreach (var action in _subscribers[messageType]) {
                    action.Invoke(message);
                }
            }
        }
    }
}
