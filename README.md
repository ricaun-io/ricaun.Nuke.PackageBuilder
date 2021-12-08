# ricaun.Nuke.PackageBuilder

[![Publish](https://github.com/ricaun-io/ricaun.Nuke.PackageBuilder/actions/workflows/Publish.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke.PackageBuilder/actions)
[![Develop](https://github.com/ricaun-io/ricaun.Nuke.PackageBuilder/actions/workflows/Develop.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke.PackageBuilder/actions)

[![Release](https://img.shields.io/nuget/v/ricaun.Nuke.PackageBuilder?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.Nuke.PackageBuilder)

# Example

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishRevit
{
    string IHazPackageBuilder.Application => "Revit.App";
    string IHazPackageBuilder.Folder => "PackageBuilder";
    string IHazOutput.Folder => "Output";
    public static int Main() => Execute<Build>(x => x.From<IPublishRevit>().Build);
}
```

---

Copyright © 2021 ricaun

