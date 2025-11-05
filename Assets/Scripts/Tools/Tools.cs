using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Tools
{
    public static Vector3 V2ToV3(Vector2 v2)
    {
        return new Vector3(v2.x, 0, v2.y);
    }
    public static Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
    public static float Pow2(float input)
    {
        return input * input;
    }
    public static float Pow2(Vector2 input)
    {
        return input.magnitude * input.magnitude;
    }
    public static float CosRectToAngle(float rectSide, float hypotenuse)
    {
        return Mathf.Acos((rectSide * rectSide) / (hypotenuse * hypotenuse));
    }
    public static float SinRectToAngle(float rectSide, float hypotenuse)
    {
        return Mathf.Asin(rectSide * rectSide / hypotenuse * hypotenuse);
    }
    public static Vector2 RotateRight90(Vector2 input)
    {
        return new Vector2(input.y, -input.x);
    }
    public static Vector2 RotateLeft90(Vector2 input)
    {
        return new Vector2(-input.y, input.x);
    }
    public static float Det(Vector2 form,Vector2 to) //
    {
        float det = form.x * to.y - form.y * to.x;
        return form.x * to.y - form.y * to.x;
    }
    public static float GetDistance(Vector2 v1, Vector2 v2)
    {
        return Mathf.Sqrt(Pow2(v1.x - v2.x) + Pow2(v1.y - v2.y));
    }
    public static float GetDistance(Vector3 v1, Vector3 v2)
    {
        return GetDistance(V3ToV2(v1), V3ToV2(v2));
    }
    public static bool LeftOf(Vector2 a, Vector2 b, Vector2 c)
    {
        float aa = Det(a - c, b - a);
        return Det(a - c, b - a) <= 0f;
    }
    public static Vector2 GetIntersectionPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        Vector2 ab = b - a;
        Vector2 ac = c - a;
        float abXac = Det(ab, ac);

        Vector2 ad = d - a;
        float abXad = Det(ab, ad);
        if (abXac * abXad >= 0) 
        {
            return Vector2.zero;
        }
        Vector2 cd = d - c;
        Vector2 ca = a - c;
        Vector2 cb = b - c;

        float cdXca = Det(cd, ca);
        float cdXcb = Det(cd, cb);
        if (cdXca * cdXcb >= 0)
        {
            return Vector2.zero;
        }
        //计算交点坐标  
        float t = Det(a - c, d - c) / Det(d - c, b - a);
        float dx = t * (b.x - a.x);
        float dy = t * (b.y - a.y);

        return new Vector2() { x = a.x + dx, y = a.y + dy };
    }
    public static float PointToLineDistance(Vector2 point,Vector2 linePoint1,Vector2 linePoint2)
    {
        Vector2 v1 = linePoint1 - point;
        Vector2 v2 = linePoint2 - linePoint1;
        Vector3 p = Vector2.Dot(v1, v2.normalized) * v2.normalized;
        //return Mathf.Sqrt(Pow2(Vector2.Dot(v1, v2)) + Pow2(v1));
        float distance = Mathf.Sqrt(Pow2(v1) - Pow2(p));
        return distance;
    }
    
    /// <summary>
    /// 获取线段 AB 上距离点 P 最近的点
    /// </summary>
    /// <param name="pointP">点 P</param>
    /// <param name="pointA">线段起点 A</param>
    /// <param name="pointB">线段终点 B</param>
    /// <returns>线段 AB 上距离点 P 最近的点</returns>
    public static Vector2 GetClosestPointOnLineSegment(Vector2 pointP, Vector2 pointA, Vector2 pointB)
    {
        Vector2 ab = pointB - pointA;
        Vector2 ap = pointP - pointA;
        
        // 如果线段长度为 0，返回起点
        float abSqrMagnitude = ab.sqrMagnitude;
        if (abSqrMagnitude < 0.0001f)
        {
            return pointA;
        }
        
        // 计算 AP 在 AB 上的投影参数 t
        float t = Vector2.Dot(ap, ab) / abSqrMagnitude;
        
        // 限制 t 在 [0, 1] 范围内，确保在线段上
        t = Mathf.Clamp01(t);
        
        // 返回最近点
        return pointA + t * ab;
    }

    public static bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        // 使用GraphicRaycaster进行UI检测
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // 检查是否有真正的UI元素被点击
        foreach (var result in results)
        {
            GameObject obj = result.gameObject;

            // 只检测真正的UI组件，排除地形、3D对象等
            if (obj.GetComponent<UnityEngine.UI.Button>() != null ||
                obj.GetComponent<UnityEngine.UI.Image>() != null ||
                obj.GetComponent<UnityEngine.UI.Text>() != null ||
                obj.GetComponent<TMPro.TextMeshProUGUI>() != null ||
                obj.GetComponent<UnityEngine.UI.InputField>() != null ||
                obj.GetComponent<TMPro.TMP_InputField>() != null ||
                obj.GetComponent<UnityEngine.UI.Toggle>() != null ||
                obj.GetComponent<UnityEngine.UI.Slider>() != null ||
                obj.GetComponent<UnityEngine.UI.Scrollbar>() != null ||
                obj.GetComponent<UnityEngine.UI.ScrollRect>() != null ||
                obj.GetComponent<UnityEngine.UI.RawImage>() != null ||
                obj.GetComponent<UnityEngine.UI.Dropdown>() != null)
            {
                return true;
            }
        }

        return false;
    }

}
