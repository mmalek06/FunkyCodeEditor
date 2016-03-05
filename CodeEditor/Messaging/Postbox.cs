using System.Collections.Generic;

namespace CodeEditor.Messaging {
    internal static class Postbox {

        #region fields

        private static List<IMessageReceiver> receivers = new List<IMessageReceiver>();

        #endregion

        #region public methods

        public static void Subscribe(IMessageReceiver receiver) {
            receivers.Add(receiver);
        }

        public static void Send<TMessage>(TMessage message) {
            foreach(var receiver in receivers) {
                receiver.Receive(message);
            }
        }

        #endregion

    }
}
