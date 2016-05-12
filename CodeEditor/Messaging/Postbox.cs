using System;
using System.Collections.Generic;

namespace CodeEditor.Messaging {
    internal class Postbox {

        #region fields

        private Dictionary<Type, List<Action<object>>> messageToMethodsMap;

        private Type lastMessageType;

        private static Dictionary<int, Postbox> instances;

        #endregion

        #region constructor

        private Postbox() {
            messageToMethodsMap = new Dictionary<Type, List<Action<object>>>();
        }

        public static Postbox InstanceFor(int code) {
            if (instances == null) {
                instances = new Dictionary<int, Postbox>();
            }
            if (!instances.ContainsKey(code)) {
                instances[code] = new Postbox();
            }

            return instances[code];
        }

        #endregion

        #region public methods

        public Postbox For(Type messageType) {
            if (!messageToMethodsMap.ContainsKey(messageType)) {
                messageToMethodsMap[messageType] = new List<Action<object>>();
            }

            lastMessageType = messageType;

            return this;
        }

        public Postbox Invoke(Action<object> action) {
            messageToMethodsMap[lastMessageType].Add(action);

            return this;
        }

        public Postbox Put<TMessage>(TMessage message) {
            if (messageToMethodsMap.ContainsKey(typeof(TMessage))) {
                foreach (var action in messageToMethodsMap[typeof(TMessage)]) {
                    action(message);
                }
            }

            return this;
        }

        public Postbox RemoveListener(Type messageType) {
            if (messageToMethodsMap.ContainsKey(messageType)) {
                messageToMethodsMap.Remove(messageType);
            }
            if (messageType == lastMessageType) {
                lastMessageType = null;
            }

            return this;
        }

        #endregion

    }
}
