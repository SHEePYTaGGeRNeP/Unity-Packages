using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Helpers.Classes
{
    public static class LogHelper
    {
        /// <summary>Writes text to Unity Console with a different color for the class.</summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void Log(Type classname, string message)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.Log(Time.time + " <color=teal>" + classname.Name + ":</color> " + message);
#endif
        }

        /// <summary>Writes text to Unity Console with a different color for the class + method.</summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void Log(Type classname, string method, string message)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.Log(Time.time + " <color=teal>" + classname.Name + "." + method + "():</color> " + message);
#endif
        }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogError(Type classname, string message)
        {
            Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + ":<color=red> " + message +
                           "</color>");
        }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogError(Type classname, string message, Exception exception)
        {
            Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + ":<color=red> " + message +
                           "\n" + exception.Message + "stacktrace:" + exception.StackTrace + "</color>");
        }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogError(Type classname, Exception exception)
        {
            Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + ":<color=red> " +
                           exception.Message + "stacktrace:" + exception.StackTrace + "</color>");
        }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogError(Type classname, string method, string message)
        {
            Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + "." + method +
                           "():<color=red> " + message + "</color>");
        }

        /// <summary>
        /// Writes the warning message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogWarning(Type classname, string message)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogWarning(Time.time + " <color=#FF9933>WARNING in </color>" + classname.Name + ":<color=#FF9933> " +
                             message + "</color>");
#endif
        }

        /// <summary>
        /// Writes the warning message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogWarning(Type classname, string method, string message)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogWarning(Time.time + " <color=#FF9933>WARNING in </color>" + classname.Name + "." + method +
                             "():<color=#FF9933> " + message + "</color>");
#endif
        }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void LogWarning(Type classname, string message, Exception exception)
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            Debug.LogError(Time.time + " <color=#FF9933>ERROR in </color>" + classname.Name + ":<color=#FF9933> " +
                           message + "\n" + exception.Message + "stacktrace:" + exception.StackTrace + "</color>");
#endif
        }
    }
}