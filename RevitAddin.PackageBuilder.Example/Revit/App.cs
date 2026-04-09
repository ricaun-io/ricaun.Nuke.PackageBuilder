using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System;

namespace RevitAddin.PackageBuilder.Example.Revit
{
    [AppLoader]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        public Result OnStartup(UIControlledApplication application)
        {
            ribbonPanel = application.CreatePanel(GetRevitVersion());
            ribbonPanel.CreatePushButton<Commands.Command>(Properties.Resource.Text)
                .SetLargeImage("/UIFrameworkRes;component/ribbon/images/revit.ico")
                .SetLongDescription($"{ContextUtils.GetName()}\n{ContextUtils.GetNumber()}");

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
#elif Revit2023
            return "2023";
#elif Revit2024
            return "2024";
#elif Revit2025
            return "2025";
#elif Revit2026
            return "2026";
#elif Revit2027
            return "2027";
#else
            return "Undefined";
#endif
        }
    }
}