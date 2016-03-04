namespace CodeEditor.Messaging {
    internal interface IMessageReceiver {
        void Receive<TMessage>(TMessage message);
    }
}
