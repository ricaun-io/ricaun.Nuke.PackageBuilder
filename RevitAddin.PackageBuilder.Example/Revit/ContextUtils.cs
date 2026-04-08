namespace RevitAddin.PackageBuilder.Example.Revit
{
    public static class ContextUtils
    {
        public static string GetName()
        {
#if NET
            var context = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(typeof(ContextUtils).Assembly);
            return context.Name;
#else
            return "Default";
#endif
        }

        public static string GetNumber()
        {
#if NET
            var context = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(typeof(ContextUtils).Assembly);
            var split = context.ToString().Split('#');
            return split[split.Length - 1];
#else
            return "0";
#endif
        }
    }
}