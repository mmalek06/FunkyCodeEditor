using System;
using System.Collections.Generic;

namespace CodeEditor.Messaging {
    internal class Postbox {

        #region fields

        private Dictionary<Type, List<Action<object>>> messageToMethodsMap;

        private Type lastMessageType;

        private static Postbox self;

        #endregion

        #region properties

        public static Postbox Instance {
            get {
                if (self == null) {
                    self = new Postbox();
                }

                return self;
            }
        }

        #endregion

        #region constructor

        private Postbox() {
            messageToMethodsMap = new Dictionary<Type, List<Action<object>>>();
        }

        #endregion

        #region public methods

        public Postbox For(Type messageType) {
            messageToMethodsMap[messageType] = new List<Action<object>>();
            lastMessageType = messageType;

            return this;
        }

        public Postbox Invoke(Action<object> action) {
            messageToMethodsMap[lastMessageType].Add(action);

            return this;
        }

        public Postbox Send<TMessage>(TMessage message) {
            if (messageToMethodsMap.ContainsKey(typeof(TMessage))) {
                foreach (var action in messageToMethodsMap[typeof(TMessage)]) {
                    action(message);
                }
            }

            return this;
        }

        #endregion

    }
}
