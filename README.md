# ricaun.Nuke.PackageBuilder

This package is to simplify the build automation system using to RevitAddin Application. 
- [ricaun.Nuke](https://www.nuget.org/packages/ricaun.Nuke) 
- [Autodesk.PackageBuilder](https://www.nuget.org/packages/Autodesk.PackageBuilder/)
- [InnoSetup.ScriptBuilder](https://www.nuget.org/packages/InnoSetup.ScriptBuilder/)

[![Revit 2017](https://img.shields.io/badge/Revit-2017+-blue.svg)](../..)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](../../actions/workflows/Build.yml/badge.svg)](../../actions)
[![Release](https://img.shields.io/nuget/v/ricaun.Nuke.PackageBuilder?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.Nuke.PackageBuilder)

# Example

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishRevit
{
    // string IHazRevitPackageBuilder.Application => "Revit.App";
    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
}
```

## Environment Variables

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD }}
    InstallationFiles: ${{ secrets.INSTALLATION_FILES }}
```

## License

This package is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this package? Please [star this project on GitHub](../../stargazers)!

---

Copyright © 2022 ricaun

