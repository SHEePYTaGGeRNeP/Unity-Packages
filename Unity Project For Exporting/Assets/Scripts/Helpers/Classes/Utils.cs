using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
#if (UNITY_EDITOR)
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Assertions;

public static class Utils
{
    public static Color GetColorRGB(float[] colors)
    {
        if (colors.IsNullOrEmpty())
            return Color.white;
        Assert.IsTrue(colors.Length == 3);
        return new Color(colors[0], colors[1], colors[2], 1f);
    }


    public static bool IsSameOrSubclass(Type potentialDescendant, Type baseType)
    {
        return potentialDescendant.IsSubclassOf(baseType)
               || potentialDescendant == baseType;
    }

#if (UNITY_EDITOR)
    public static T GetActualObjectForSerializedProperty<T>(FieldInfo fieldInfo, SerializedProperty property)
        where T : class
    {
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        if (obj == null)
        {
            return null;
        }
        T actualObject;
        if (obj.GetType().IsArray || obj.GetType().IsGenericType)
        {
            var index = Convert.ToInt32(new string(property.propertyPath.Where(char.IsDigit).ToArray()));
            IEnumerable<T> ienumerable = (IEnumerable<T>) obj;
            actualObject = ienumerable.ElementAt(index);
            //actualObject = ((T[]) obj)[index];
        }
        else
        {
            actualObject = obj as T;
        }
        return actualObject;
    }
#endif
    public static T RandomChance<T>(KeyValuePair<float, T>[] items)
    {
        float sum = items.Sum(item => item.Key);
        items = items.OrderBy(item => item.Key).ToArray();
        float randomPosition = UnityEngine.Random.Range(0, sum);
        float top = 0;
        for (int i = 0; i < items.Length; i++)
        {
            top += items[i].Key;
            if (randomPosition < top)
            {
                return items[i].Value;
            }
        }
        throw new Exception("RandomChange<T>(KeyValuePair<float,T>[])");
    }

    public static KeyValuePair<float, T>[] CreateChanceArray<T>(float[] keys, T[] items)
    {
        List<KeyValuePair<float, T>> dropChances = new List<KeyValuePair<float, T>>();
        for (int i = 0; i < keys.Length; ++i)
        {
            dropChances.Add(new KeyValuePair<float, T>(keys[i], items[i]));
        }
        return dropChances.ToArray();
    }

    public static T Max<T>(T x, T y)
    {
        return (Comparer<T>.Default.Compare(x, y) > 0) ? x : y;
    }

    public static T Min<T>(T x, T y)
    {
        return (Comparer<T>.Default.Compare(x, y) < 0) ? x : y;
    }

    public static float VelocityToMph(Vector3 velocity)
    {
        return velocity.magnitude * 2.23693629f;
    }

    public static float VelocityToKph(Vector3 velocity)
    {
        return velocity.magnitude * 3.6f;
    }

    /// <summary>
    /// TimeSpan time = Utils.TimeAction(() =>
    ///{
    ///    // Do some work
    ///});
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static TimeSpan TimeAction(Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    private static readonly StringBuilder _builder = new StringBuilder();

    public static string GetTime(float seconds)
    {
        int intTime = (int) seconds;
        int minutes = intTime / 60;
        int secondsint = intTime % 60;
        float fraction = seconds * 1000;
        fraction = (fraction % 1000);
        _builder.ClearContents();
        _builder.Append(minutes.ToString("00"));
        _builder.Append(":");
        _builder.Append(secondsint.ToString("00"));
        _builder.Append(":");
        _builder.Append(fraction.ToString("000"));
        return _builder.ToString();
    }

    /// <summary>
    /// Not all switch cases present
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string PathForDocumentsFile(string filename = "")
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
            {
                string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Path.Combine(path, "Documents"), filename);
            }
            case RuntimePlatform.Android:
            {
                string path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, filename);
            }
            // Not all switch cases present
            default:
            {
                string path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, filename);
            }
        }
    }

    /// <summary>
    /// Returns parent.name:parent.name:transform.name
    /// </summary>
    /// <param name="transform"></param>
    /// <returns>parent.name:parent.name:transform.name</returns>
    public static string GetGameObjectFullName(Transform transform)
    {
        string name = String.Empty;
        Transform t = transform;
        while (t.parent != null)
        {
            name += t.parent.name + ":";
            t = t.parent;
        }
        name += transform.name;
        return name;
    }

    public static Vector2 RandomizedVector2(float magnitude = 1f)
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector2 returnVector = v * magnitude;
        return returnVector;
    }

    public static Vector2 RandomizedVector2(Vector2 min, Vector2 max)
    {
        if (min == max)
            return new Vector2(min.x, max.y);
        Vector2 newVector2 =
            new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
        return newVector2;
    }

    public static Vector3 RandomizedVector3(float magnitude = 1f)
    {
        Vector3 v = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector3 returnVector = v * magnitude;
        return returnVector;
    }

    public static Vector3 RandomizedVector3(Vector3 min, Vector3 max)
    {
        if (min == max)
            return new Vector3(min.x, min.y);
        Vector3 newVector3 = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z));
        return newVector3;
    }

    public static float VolumeToDb(float volume)
    {
        if (volume > 0f)
            return (float) (20f * Math.Log10(volume));
        return -80f;
    }

    public static float DbToVolume(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return RandomizedVector3(bounds.min, bounds.max);
    }

    public static void FovAction(Vector3 up, Vector3 forward, int nrOfRays, float fov,
        Action<Vector3, int> raycastAction)
    {
        float angleAdd = fov / (nrOfRays - 1);
        // first left then right.            
        bool secondRay = false;
        float currentAngle = 0;
        // we first raycast forwards then at an angle for the left and the right.
        for (int i = 0; i < nrOfRays; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(secondRay ? currentAngle : -currentAngle, up) * forward;

            raycastAction(direction, i);

            if (i == 0)
                currentAngle += angleAdd;
            else if (i > 0 && !secondRay)
                secondRay = true;
            else
            {
                secondRay = false;
                currentAngle += angleAdd;
            }
        }
    }

    /// <summary>
    /// Returns Angle. Left is negative. Right is positive.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static float GetAngleOfRay(int index, int nrOfRays, float fov)
    {
        Assert.IsTrue(index >= 0 && index < nrOfRays);
        if (index == 0) return 0;
        float maxAngle = fov / 2;
        int sideRays = nrOfRays / 2;
        bool rightRay = index % 2 == 0;
        float anglePerRay = maxAngle / sideRays;
        // right ray is always one more
        float angleIndex = anglePerRay * ((rightRay ? index : index + 1) / 2);
        return rightRay ? angleIndex : -angleIndex;
    }

    /// <summary>
    /// x Less than 0 is left, x more than 0 is right
    /// z less than 0 is behind, z more than 0 is forward
    /// y less than 0 is under, y more than 0 is above
    /// </summary>
    public static Vector3 ObjectSide(Transform t1, Vector3 target)
    {
        return t1.InverseTransformPoint(target);
//        if (relativePoint.x < 0.0)
//            print ("Object is to the left");
//        else if (relativePoint.x > 0.0)
//            print ("Object is to the right");
//        else
//            print ("Object is directly ahead");
    }

    public static T ParseEnum<T>(string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

    public static void AddColorToString(string s, Color color, out string newString)
    {
        string colorString = ColorUtility.ToHtmlStringRGB(color);
        colorString = "<color=#" + colorString + ">" + s + "</color>";
        newString = colorString;
    }
}