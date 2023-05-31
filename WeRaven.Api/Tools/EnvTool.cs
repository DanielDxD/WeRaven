namespace WeRaven.Api.Tools
{
    public static class EnvTool
    {
        public static bool IsDebug()
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }
}
