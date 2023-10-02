using UnityEngine;

namespace Utilities
{
    public static class VectorExtentions
    {
        #region Vector2

        /// <summary> Distance to line that passses through pointA and pointB. </summary>
        public static float Distance(this Vector2 v, Vector2 a, Vector2 b) =>
            Mathf.Abs((b.y - a.y) * v.x - (b.x - a.x) * v.y + b.x * a.y - b.y * a.x) /
            Mathf.Sqrt(Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.x - a.x, 2));

        /// <summary> Square Distance to line that passses through pointA and pointB. </summary>
        public static float SqrDistance(this Vector2 v, Vector2 a, Vector2 b) =>
            Mathf.Pow((b.y - a.y) * v.x - (b.x - a.x) * v.y + b.x * a.y - b.y * a.x, 2) /
            (Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.x - a.x, 2));

        public static Vector3 XY0(this Vector2 v) => new(v.x, v.y, 0);
        public static Vector3 X0Z(this Vector2 v) => new(v.x, 0, v.y);

        #endregion

        #region Vector3

        /// <summary> Vector2 (x, y) </summary>
        public static Vector2 XY(this Vector3 v) => new Vector2(v.x, v.y);

        /// <summary> Vector2 (x, z) </summary>
        public static Vector2 XZ(this Vector3 v) => new Vector2(v.x, v.z);

        /// <summary> Vector2 (y, z) </summary>
        public static Vector2 YZ(this Vector3 v) => new Vector2(v.y, v.z);

        /// <summary> Vector3 (0, y, z) </summary>
        public static Vector3 OYZ(this Vector3 v) => new Vector3(0, v.y, v.z);

        /// <summary> Vector3 (x, 0, z) </summary>
        public static Vector3 XOZ(this Vector3 v) => new Vector3(v.x, 0, v.z);

        /// <summary> Vector3 (x, y, 0) </summary>
        public static Vector3 XYO(this Vector3 v) => new Vector3(v.x, v.y, 0);

        /// <summary> Vector3 (x, 0, 0) </summary>
        public static Vector3 XOO(this Vector3 v) => new Vector3(v.x, 0, 0);

        /// <summary> Vector3 (x, y, 0) </summary>
        public static Vector3 OYO(this Vector3 v) => new Vector3(0, v.y, 0);

        /// <summary> Vector3 (0, 0, z) </summary>
        public static Vector3 OOZ(this Vector3 v) => new Vector3(0, 0, v.z);

        /// <summary> Vector3 (value, y, z) </summary>
        public static Vector3 xYZ(this Vector3 v, float value) => new Vector3(value, v.y, v.z);

        /// <summary> Vector3 (x, value, z) </summary>
        public static Vector3 XyZ(this Vector3 v, float value) => new Vector3(v.x, value, v.z);

        /// <summary> Vector3 (x, y, value) </summary>
        public static Vector3 XYz(this Vector3 v, float value) => new Vector3(v.x, v.y, value);

        /// <summary> Vector3 (valueX, valueY, z) </summary>
        public static Vector3 xyZ(this Vector3 v, float valueX, float valueY) => new Vector3(valueX, valueY, v.z);

        public static Vector3 GetCrossProductWithFixedPlane(this Vector3 v, Vector3 fixedVector)
        {
            var perpendicularVector = Vector3.Cross(v, fixedVector);
            return perpendicularVector;
        }

        public static Vector3 GetDeviatedVectorWithFixedPlane(this Vector3 v, Vector3 fixedVector, float multiplier)
        {
            return ((GetCrossProductWithFixedPlane(v, fixedVector) * multiplier + v)).normalized;
        }

        /// <summary> Vector4 (x, y, z, 0) </summary>
        public static Vector4 XYZO(this Vector3 v) => new Vector4(v.x, v.y, v.z, 0);

        /// <summary> Vector4 (x, y, z, 1) </summary>
        public static Vector4 XYZ1(this Vector3 v) => new Vector4(v.x, v.y, v.z, 1);

        /// <summary> Vector4 (x, y, z, value) </summary>
        public static Vector4 XYZw(this Vector3 v, float value) => new Vector4(v.x, v.y, v.z, value);

        public static Vector3 ClampMagnitude(this Vector3 v, float min, float max)
        {
            double sqrMag = v.sqrMagnitude;
            double sqrMin = min * min;
            double sqrMax = max * max;

            if (sqrMag < sqrMin) return v.normalized * min;
            else if (sqrMag > sqrMax) return v.normalized * max;
            else return v;
        }

        #endregion

        #region Vector4

        /// <summary> Vector3 (x, y, z) </summary>
        public static Vector3 XYZ(this Vector4 v) => new Vector3(v.x, v.y, v.z);

        #endregion
    }
}