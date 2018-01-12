using System;
namespace InfrastructureLight.Wpf.Common.Messaging
{
    public interface IMessenger
    {
        void Register<TMessage>(Action<TMessage> action) where TMessage : IMessage;
        void Send<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
