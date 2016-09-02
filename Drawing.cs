using System;
using UnityEngine;

public static class Drawing
{
    public static Texture2D lineTex = null;
    
    public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB) { DrawLine(cam, pointA, pointB, GUI.contentColor, 1.0f); }
    public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB, Color color) { DrawLine(cam, pointA, pointB, color, 1.0f); }
    public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB, float width) { DrawLine(cam, pointA, pointB, GUI.contentColor, width); }
    public static void DrawLine(Camera cam, Vector3 pointA, Vector3 pointB, Color color, float width)
    {
        Vector2 p1 = Vector2.zero;
        p1.x = cam.WorldToScreenPoint(pointA).x;
        p1.y = cam.WorldToScreenPoint(pointA).y;
        Vector2 p2 = Vector2.zero;
        p2.x = cam.WorldToScreenPoint(pointB).x;
        p2.y = cam.WorldToScreenPoint(pointB).y;
        DrawLine(p1, p2, color, width);
    }

    public static void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
    public static void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
    public static void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
    public static void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
    {
        pointA.x = (int)pointA.x; pointA.y = (int)pointA.y;
        pointB.x = (int)pointB.x; pointB.y = (int)pointB.y;

        if (!lineTex) { lineTex = new Texture2D(1, 1); }
        Color savedColor = GUI.color;
        GUI.color = color;

        Matrix4x4 matrixBackup = GUI.matrix;

        float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;
        float length = (pointA - pointB).magnitude;
        GUIUtility.RotateAroundPivot(angle, pointA);
        GUI.DrawTexture(new Rect(pointA.x, pointA.y, length, width), lineTex);

        GUI.matrix = matrixBackup;
        GUI.color = savedColor;        
    }

    public static void DrawCircle(Vector2 center, float radius, Color color, float width, int segmentsPerQuarter)
    {
        float rh = (float)radius * 0.551915024494f;

        Vector2 p1 = new Vector2(center.x, center.y - radius);
        Vector2 p1_tan_a = new Vector2(center.x - rh, center.y - radius);
        Vector2 p1_tan_b = new Vector2(center.x + rh, center.y - radius);

        Vector2 p2 = new Vector2(center.x + radius, center.y);
        Vector2 p2_tan_a = new Vector2(center.x + radius, center.y - rh);
        Vector2 p2_tan_b = new Vector2(center.x + radius, center.y + rh);

        Vector2 p3 = new Vector2(center.x, center.y + radius);
        Vector2 p3_tan_a = new Vector2(center.x - rh, center.y + radius);
        Vector2 p3_tan_b = new Vector2(center.x + rh, center.y + radius);

        Vector2 p4 = new Vector2(center.x - radius, center.y);
        Vector2 p4_tan_a = new Vector2(center.x - radius, center.y - rh);
        Vector2 p4_tan_b = new Vector2(center.x - radius, center.y + rh);

        DrawBezierLine(p1, p1_tan_b, p2, p2_tan_a, color, width, segmentsPerQuarter);
        DrawBezierLine(p2, p2_tan_b, p3, p3_tan_b, color, width, segmentsPerQuarter);
        DrawBezierLine(p3, p3_tan_a, p4, p4_tan_b, color, width, segmentsPerQuarter);
        DrawBezierLine(p4, p4_tan_a, p1, p1_tan_a, color, width, segmentsPerQuarter);
    }


    public static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, int segments)
    {
        Vector2 lastV = CubeBezier(start, startTangent, end, endTangent, 0);
        for (int i = 1; i < segments + 1; ++i)
        {
            Vector2 v = CubeBezier(start, startTangent, end, endTangent, i / (float)segments);
            DrawLine(lastV, v, color, width);
            lastV = v;
        }
    }

    private static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
    {
        float rt = 1 - t;
        return rt * rt * rt * s + 3 * rt * rt * t * st + 3 * rt * t * t * et + t * t * t * e;
    }
}
