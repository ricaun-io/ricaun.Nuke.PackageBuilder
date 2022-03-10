using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System;

namespace RevitAddin.PackageBuilder.Example.Revit
{
    [Console]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            ribbonPanel = application.CreatePanel(GetRevitVersion());
            ribbonPanel.AddPushButton<Commands.Command>("-");
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            return Result.Succeeded;
        }

        public static string GetRevitVersion()
        {
#if Revit2017 
            return "2017";
#elif Revit2018
            return "2018";
#elif Revit2019
            return "2019";
#elif Revit2020
            return "2020";
#elif Revit2021
            return "2021";
#elif Revit2022
            return "2022";
#else
            return "Undefined";
#endif
        }
    }
}