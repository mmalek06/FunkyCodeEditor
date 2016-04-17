using System.Reflection;

namespace CodeEditor.Tests.ScenarioTests {
    internal static class PrivateMembersHelper {

        #region public methods

        public static object GetPropertyValue(object obj, string propName) {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var fieldInfo = obj.GetType().GetProperty("VisualChildrenCount", bindFlags);
            var value = fieldInfo.GetValue(obj);

            return value;
        }

        public static object InvokeMethod(object obj, string methodName, object[] methodParams) {
            var methodInfo = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            return methodInfo.Invoke(obj, methodParams);
        }

        #endregion

    }
}
