namespace GameCore.Utility.Shape
{
    public struct Hex
    {
        public float q { get; }
        public float r { get; }

        public Hex(float q, float r)
        {
            this.q = q;
            this.r = r;
        }

        public static Cube ToCube(Hex hex)
        {
            var q = hex.q;
            var r = hex.r;
            var s = -q - r;
            return new Cube(q, r, s);
        }
    }
}