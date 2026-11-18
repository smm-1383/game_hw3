using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntryPath", menuName = "ChickenShooter/Entry Path")]
public class EntryPathSO : ScriptableObject
{
    public Vector2[] points;

    public void FillCurve(List<Vector3> result, Vector3 end, int steps)
    {
        result.Clear();
        int total = points.Length + 1;
        for (int i = 0; i < total - 1; i++)
        {
            Vector3 a = GetPoint(i - 1, end);
            Vector3 b = GetPoint(i, end);
            Vector3 c = GetPoint(i + 1, end);
            Vector3 d = GetPoint(i + 2, end);
            for (int s = 0; s < steps; s++)
            {
                result.Add(Spline(a, b, c, d, (float)s / steps));
            }
        }
        result.Add(end);
    }

    private Vector3 GetPoint(int index, Vector3 end)
    {
        if (index >= points.Length)
        {
            return end;
        }
        Vector2 viewport = points[Mathf.Max(index, 0)];
        return ScreenArea.FromViewport(viewport.x, viewport.y);
    }

    private Vector3 Spline(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * (2f * b + (c - a) * t + (2f * a - 5f * b + 4f * c - d) * t2 + (3f * b - a - 3f * c + d) * t3);
    }
}
