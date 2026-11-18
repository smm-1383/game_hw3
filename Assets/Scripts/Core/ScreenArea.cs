using UnityEngine;

public static class ScreenArea
{
    public static float Left
    {
        get { return FromViewport(0f, 0f).x; }
    }

    public static float Right
    {
        get { return FromViewport(1f, 0f).x; }
    }

    public static float Bottom
    {
        get { return FromViewport(0f, 0f).y; }
    }

    public static float Top
    {
        get { return FromViewport(0f, 1f).y; }
    }

    public static Vector3 FromViewport(float x, float y)
    {
        Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0f));
        point.z = 0f;
        return point;
    }
}
