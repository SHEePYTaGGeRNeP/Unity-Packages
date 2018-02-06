using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshHelper
{
    [Serializable]
    public class SideInfo
    {
        public enum Side
        {
            Top,
            Bottom,
            Front,
            Back,
            Right,
            Left,
            Middle
        }

        public Side Sides;
        public Vector3 Offset;
    }

    public Vector3 GetMeshPosition(SideInfo.Side[] sides, GameObject gameObject)
    {
        Dictionary<SideInfo.Side, Vector3> dict = GetMeshPositions(gameObject);
        Vector3 vector = Vector3.zero;
        foreach (SideInfo.Side side in sides)
        {
            vector += dict[side];
        }
        return vector;
    }

    public static Dictionary<SideInfo.Side, Vector3> GetMeshPositions(GameObject gameObject)
    {
        Dictionary<SideInfo.Side, Vector3> dict = Enum.GetValues(typeof(SideInfo.Side)).Cast<SideInfo.Side>().ToDictionary(side => side, side => Vector3.zero);

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        foreach (Vector3 vertPosition in meshFilter.mesh.vertices)
        {
            Vector3 current = dict[SideInfo.Side.Back];
            bool useNewValue = Mathf.Min(current.z, vertPosition.z).AboutEqualTo(vertPosition.z);
            if (useNewValue)
                dict[SideInfo.Side.Back] = Vector3.zero.WithZ(vertPosition.z);

            current = dict[SideInfo.Side.Front];
            useNewValue = Mathf.Max(current.z, vertPosition.z).AboutEqualTo(vertPosition.z);
            if (useNewValue)
                dict[SideInfo.Side.Front] = Vector3.zero.WithZ(vertPosition.z);

            current = dict[SideInfo.Side.Left];
            useNewValue = Mathf.Min(current.x, vertPosition.x).AboutEqualTo(vertPosition.x);
            if (useNewValue)
                dict[SideInfo.Side.Left] = Vector3.zero.WithX(vertPosition.x);

            current = dict[SideInfo.Side.Right];
            useNewValue = Mathf.Max(current.x, vertPosition.x).AboutEqualTo(vertPosition.x);
            if (useNewValue)
                dict[SideInfo.Side.Right] = Vector3.zero.WithX(vertPosition.x);

            current = dict[SideInfo.Side.Top];
            useNewValue = Mathf.Max(current.y, vertPosition.y).AboutEqualTo(vertPosition.y);
            if (useNewValue)
                dict[SideInfo.Side.Top] = Vector3.zero.WithY(vertPosition.y);

            current = dict[SideInfo.Side.Bottom];
            useNewValue = Mathf.Min(current.y, vertPosition.y).AboutEqualTo(vertPosition.y);
            if (useNewValue)
                dict[SideInfo.Side.Bottom] = Vector3.zero.WithY(vertPosition.y);
        }

        dict[SideInfo.Side.Middle] = ((dict[SideInfo.Side.Bottom] + dict[SideInfo.Side.Top]) / 2);


        return dict;
    }
}