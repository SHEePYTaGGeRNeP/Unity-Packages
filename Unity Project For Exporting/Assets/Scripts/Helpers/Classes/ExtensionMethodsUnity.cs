using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// ReSharper disable once CheckNamespace
public static class ExtensionMethodsUnity
{
    public static float[] GetColorRGB(this Color color)
    {
        return new[] {color.r, color.g, color.b};
    }

    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void ResetLocalTransformation(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.MinMaxCurve rate = emission.rateOverTime;
        rate.constantMax = emissionRate;
        emission.rateOverTime = rate;
    }

    public static void SetStartColor(this ParticleSystem particleSystem, Color startColor)
    {
        ParticleSystem.MainModule settings = particleSystem.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(startColor);
    }

    public static bool IsLayerInLayerMask(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool IsTransformInMyParents(this Transform t, Transform parent)
    {
        if (t == parent) return true;
        Transform p = t;
        while ((p = p.parent) != null)
        {
            if (p == parent)
                return true;
        }
        return false;
    }


    #region Vector extensions

    public static Vector2 Xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 Xz(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 Yz(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithYZ(this Vector3 v, float y, float z)
    {
        return new Vector3(v.x, y, z);
    }

    public static Vector3 WithXY(this Vector3 v, float x, float y)
    {
        return new Vector3(x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 WithXZ(this Vector3 v, float x, float z)
    {
        return new Vector3(x, v.y, z);
    }

    public static Vector2 WithX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 WithZ(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }


    public static Vector3 AddX(this Vector3 v, float x)
    {
        return new Vector3(v.x + x, v.y, v.z);
    }

    public static Vector3 AddY(this Vector3 v, float y)
    {
        return new Vector3(v.x, v.y + y, v.z);
    }

    public static Vector3 AddZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, v.z + z);
    }

    public static Vector3 Abs(this Vector3 a)
    {
        return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }

    public static Vector3 Inverse(this Vector3 a)
    {
        return new Vector3(1 / a.x, 1 / a.y, 1 / a.z);
    }

    #endregion

    #region SetRecursively

    // Set the layer of this GameObject and all of its children.
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
            t.gameObject.SetLayerRecursively(layer);
    }

    public static void SetRenderersRecursively(this GameObject gameObject, bool enabled)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
            renderer.enabled = enabled;
    }

    public static void SetCollisionRecursively(this GameObject gameObject, bool tf)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = tf;
    }

    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag) where T : Component
    {
        List<T> results = new List<T>();

        if (gameObject.CompareTag(tag))
            results.Add(gameObject.GetComponent<T>());

        foreach (Transform t in gameObject.transform)
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

        return results.ToArray();
    }

    #endregion


    #region Unity Colors

    public static Color WithR(this Color color, float r)
    {
        return new Color(r, color.g, color.b, color.a);
    }

    public static Color AddR(this Color color, float r)
    {
        return new Color(color.r + r, color.g, color.b, color.a);
    }

    public static Color WithG(this Color color, float g)
    {
        return new Color(color.r, g, color.b, color.a);
    }

    public static Color AddG(this Color color, float g)
    {
        return new Color(color.r, color.g + g, color.b, color.a);
    }

    public static Color WithB(this Color color, float b)
    {
        return new Color(color.r, color.g, b, color.a);
    }

    public static Color AddB(this Color color, float b)
    {
        return new Color(color.r, color.g, color.b + b, color.a);
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Color AddAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, color.a + alpha);
    }

    #endregion

    #region Rects

    public static Rect WithX(this Rect rect, float x)
    {
        return new Rect(x, rect.y, rect.width, rect.height);
    }

    public static Rect AddX(this Rect rect, float x)
    {
        return new Rect(rect.x + x, rect.y, rect.width, rect.height);
    }

    public static Rect WithY(this Rect rect, float y)
    {
        return new Rect(rect.x, y, rect.width, rect.height);
    }

    public static Rect AddY(this Rect rect, float y)
    {
        return new Rect(rect.x, rect.y + y, rect.width, rect.height);
    }

    public static Rect WithWidth(this Rect rect, float width)
    {
        return new Rect(rect.x, rect.y, width, rect.height);
    }

    public static Rect AddWidth(this Rect rect, float width)
    {
        return new Rect(rect.x, rect.y, rect.width + width, rect.height);
    }

    public static Rect WithHeight(this Rect rect, float height)
    {
        return new Rect(rect.x, rect.y, rect.width, height);
    }

    public static Rect AddHeight(this Rect rect, float height)
    {
        return new Rect(rect.x, rect.y, rect.width, rect.height + height);
    }

    #endregion


    public static void DebugDrawRay(this Ray ray, Color color, float duration = 1f, float directionMultiplier = 1f)
    {
        Debug.DrawRay(ray.origin, ray.direction * directionMultiplier, color, duration);
    }
}