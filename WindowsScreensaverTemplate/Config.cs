namespace WindowsScreensaverTemplate
{
    public static class Config
    {
        public static readonly string FRAMENUMFORMAT = "000";
        public static readonly string FRAMEPREFIX = "frame_";
        public static readonly int OUTERBUFFER = 2;
        public static readonly int COUNTERSTART = 1;
        public static readonly int MOUSEMOVESENSITIVITY = 3;
        public static readonly int TARGETFRAMERATE = 30;

        public static string GetAssetString(int counter)
        {
            return $"{FRAMEPREFIX}{counter.ToString(FRAMENUMFORMAT)}";
        }
    }
}
