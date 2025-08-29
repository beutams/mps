using UnityEngine;

public static class ColorTool
{
    public static float[] ToFloat(Color color)
    {
        return new float[] { color.r, color.g, color.b, color.a };
    } 
    public static Color ToColor(float[] color)
    {
        if(color.Length == 4)
            return new Color(color[0], color[1], color[2], color[3]);
        return Color.white;
    }
}