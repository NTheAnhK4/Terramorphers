using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Utility.Shape
{
    public readonly struct Cube
    {
        public float q { get; }
        public float r { get; }
        public float s { get; }

        public Cube(float q, float r, float s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public static Hex ToHex(Cube cube)
        {
            var q = cube.q;
            var r = cube.r;
            return new Hex(q, r);
        }
        public static Cube Lerp(Cube a, Cube b, float t) => new Cube(Mathf.Lerp(a.q, b.q, t),
            Mathf.Lerp(a.r, b.r, t),
            Mathf.Lerp(a.s, b.s, t));

        #region Operator

        public static Cube operator - (Cube a, Cube b) => new Cube(a.q - b.q, a.r - b.r, a.s - b.s);
        public static Cube operator +(Cube a, Cube b) => new Cube(a.q + b.q, a.r + b.r, a.s + b.s);
        public static Cube operator *(Cube a, float b) => new Cube(a.q * b, a.r * b, a.s * b);
        public static Cube operator *(Cube a, int b) => new Cube(a.q * b, a.r * b, a.s * b);
        public static Cube operator *(float b, Cube a) => new Cube(a.q * b, a.r * b, a.s * b);
        public static Cube operator *(int b, Cube a) => new Cube(a.q * b, a.r * b, a.s * b);
        public static bool operator ==(Cube a, Cube b) => Mathf.Abs(Cube.Distance(a, b)) <= 0.0001f;
        public static bool operator !=(Cube a, Cube b) => Mathf.Abs(Cube.Distance(a, b)) > 0.0001f;

        #endregion
     
        public static float Distance(Cube a, Cube b)
        {
            var vec = a - b;
            return (Mathf.Abs(vec.q) + Mathf.Abs(vec.r) + Mathf.Abs(vec.s)) / 2;
        }

        public static Cube Round(Cube a)
        {
            var rq = Mathf.Round(a.q);
            var rr = Mathf.Round(a.r);
            var rs = Mathf.Round(a.s);

            var q_diff = Mathf.Abs(rq - a.q);
            var r_diff = Mathf.Abs(rr - a.r);
            var s_diff = Mathf.Abs(rs - a.s);

            if (q_diff > r_diff && q_diff > s_diff) rq = -rr - rs;
            else if (r_diff > s_diff) rr = -rq - rs;
            else rs = -rq - rr;
            return new Cube(rq, rr, rs);
        }

        public bool IsValid() => q + r + s == 0;
        public override string ToString() => $"[{q} {r} {s}]";

        public Cube Rotate60Clockwise() => new Cube(-r,-s,-q);
        public Cube Rotate60CounterClockwise() => new Cube(-s,-q,-r);
        
    }
}