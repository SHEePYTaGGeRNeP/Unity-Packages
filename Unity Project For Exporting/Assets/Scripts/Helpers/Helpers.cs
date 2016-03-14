namespace Assets
{
    using System;
    using UnityEngine;

    public static class LogHelper
    {
        /// <summary>Writes text to Unity Console with a different color for the class.</summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void Log(Type classname, string message)
        { Debug.Log("<color=teal>" + classname.Name + ":</color> " + message); }

        /// <summary>Writes text to Unity Console with a different color for the class + method.</summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void Log(Type classname, string method, string message)
        { Debug.Log("<color=teal>" + classname.Name + "." + method +"():</color> " + message); }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogError(Type classname, string method, string message)
        { Debug.LogError("<color=red>ERROR in </color>" + classname.Name + "." + method + "():<color=red> " + message + "</color>"); }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogError(Type classname, string method, Exception exception)
        { Debug.LogError("<color=red>ERROR in </color>" + classname.Name + "." + method + "():<color=red> " + exception.Message + "stacktrace:" + exception.StackTrace + "</color>"); }
        /// <summary>
        /// Writes the warning message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogWarning(Type classname, string method, string message)
        { Debug.LogWarning("<color=orange>WARNING in </color>" + classname.Name + "." + method + "():<color=orange> " + message + "</color>"); }

    }
}