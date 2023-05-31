namespace WeRaven.Api.Tools
{
    public static class MathTool
    {
        private static readonly Random _random = new();
        public static int GenerateRandom(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static int GenerateCode()
        {
            return GenerateRandom(100000, 999999);
        }
    }
}
