using UnityEngine;

public static class Ease
{
    public static float EaseOutBack(float _t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(_t - 1, 3) + c1 * Mathf.Pow(_t - 1, 2);
    }

    public static float EaseOutSine(float _t)
    {
        return Mathf.Sin((_t * Mathf.PI) / 2);
    }
}