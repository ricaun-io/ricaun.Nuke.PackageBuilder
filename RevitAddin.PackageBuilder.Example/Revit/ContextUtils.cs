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
    }
}