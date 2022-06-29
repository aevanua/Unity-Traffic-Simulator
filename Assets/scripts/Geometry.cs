using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Geometry
{
    static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
    public float Vector_SkalarMulti(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }
    public float Vector_Multi(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }
    public bool IsOnLane(Vector3 v1, Vector3 v2, Vector3 pos)
    {
        return (Vector_SkalarMulti(v2 - v1, pos - v1) >= 0 && Vector_SkalarMulti(v1 - v2, pos - v2) >= 0);
    }
    public float GetHeight(Vector3 v1, Vector3 v2, Vector3 pos)
    {
        Vector3 FirstVector = v1 - pos;
        Vector3 SecondVector = v2 - pos;
        return Mathf.Abs(Vector_Multi(FirstVector, SecondVector)) / Vector3.Distance(FirstVector, SecondVector);
    }
    public Vector3 GetPerpendicularIntersection(Vector3 v1, Vector3 v2, Vector3 pos)
    {
        float dacab = (pos.x - v1.x) * (v2 - v1).x + (pos.y - v1.y) * (v2 - v1).y;
        float dab = (v2 - v1).x * (v2 - v1).x + (v2 - v1).y * (v2 - v1).y;
        float t = dacab / dab;
        return v1 + (v2 - v1) * t;
    }
    public Quaternion CalculateRotation(Vector3 node1, Vector3 node2, float offset)
    {
        if (node1 == node2) return Quaternion.Euler(0, 0, 0);
        float z;
        Vector3 node1Temp = new Vector3(node1.x, node1.y, 0);
        Vector3 node2Temp = new Vector3(node2.x, node2.y, 0);
        float sin = Mathf.Abs(node1.y - node2.y) / Vector3.Distance(node1Temp, node2Temp);
        float alpha = Mathf.Asin(sin);
        if ((node1Temp.x < node2Temp.x && node1Temp.y > node2Temp.y) || (node1Temp.x > node2Temp.x && node1Temp.y < node2Temp.y))
            z = -alpha * Mathf.Rad2Deg;
        else
            z = alpha * Mathf.Rad2Deg;
        if (z < 0)
            z += 360f;
        Quaternion rotation = Quaternion.Euler(0, 0, z + offset);
        return rotation;
    }
    public Vector3 RotateRound(Vector3 position, Vector3 center, float angle)
    {
        Vector3 axis = new Vector3(0, 0, 1);
        Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
        Vector3 resultVec3 = center + point;
        return resultVec3;
    }
    //?
    public Vector3 GetPerpendicularPoint(Vector3 v1, Vector3 pos, float dist)
    {
        float k = Mathf.Sqrt(dist * dist / ((pos.x - v1.x) * (pos.x - v1.x) + (pos.y - v1.y) * (pos.y - v1.y)));
        Vector3 Res = new Vector3(v1.x + (pos.x - v1.x) * k, v1.y + (pos.y - v1.y) * k);
        return Res;
    }
    public Vector3 GetPerpendicularPointTemp(Vector3 axis, Vector3 pos, float dist)
    {
        float DAxis=Mathf.Sqrt(axis.x*axis.x+ axis.y * axis.y);
        Vector3 Res= new Vector3((dist * axis.y) / DAxis, -(dist * axis.x) / DAxis);
        return pos-Res;
    }
    public bool IsBetweenPoints(Vector3 v1, Vector3 v2, Vector3 check)
    {
        float x1 = v1.x, x2 = v2.x, y1 = v1.y, y2 = v2.y;
        if (v1.x > v2.x)
            Swap<float>(ref x1, ref x2);
        if (v1.y > v2.y)
            Swap<float>(ref y1, ref y2);
        return ((x1 <= check.x && check.x <= x2) && (y1 <= check.y && check.y <= y2));
    }
    public bool IsRightDirection(Vector3 v1, Vector3 v2)
    {
        if (v1.x != v2.x)
            return (v1.x < v2.x);
        return (v1.y > v2.y);
    }
}
