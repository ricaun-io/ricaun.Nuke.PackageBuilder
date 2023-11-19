# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.6.2] / 2023-11-19
### Updated
- Update `ricaun.Nuke` to `1.7.2`

## [1.6.1] / 2023-10-05
### Features
- Prerelease with unlist package.
### Updated
- Update `ricaun.Nuke` to `1.7.1`

## [1.6.0] / 2023-10-04
### Updated
- Update `ricaun.Nuke` to `1.7.0`
- Add `Title` in `IssConfiguration` class.
- Update `IssRevitBuilder` to abstract `IssPackageBuilder`.
- Update `IRevitPackageBuilder` to generic `T` and base `IssRevitBuilder`.
### Tests
- Tests build with PackageBuilder `prerelease` version

## [1.5.1] / 2023-07-24
### Updated
- Update `ricaun.Nuke` to `1.5.1`
### Tests
- Add `Tests` project
- Add `RevitExtension_Tests` with Example files

## [1.5.0] / 2023-05-31
### Updated
- Update `ricaun.Nuke` to `1.5.0`
### Fixed
- Fix `PathConstruction` to `Globbing`
- Fix `FileSystemTasks` to `AbsolutePathExtensions`
- Fix `ToolPathResolver` to `NuGetToolPathResolver`
- Fix `InstallationFiles` when folder.

## [1.4.3] / 2023-05-06
### Updated
- Update Readme
- Update `RevitContentsBuilder` add `MiddleVersions`

## [1.4.2] / 2023-03-30
### Updated
- Update `ricaun.Nuke` to `1.4.4`
- Update `InnoSetup.ScriptBuilder` to `1.3.0`
- Update `Build` project to `net7.0`
- Update `LAST_VERSION_PLUS_YEAR` to 1

## [1.4.1] / 2023-01-24
### Features
- Feature `DownloadFileRetry` to download `InstallationFiles`
### Updated
- Update `ricaun.Nuke` to `1.4.3`

## [1.4.0] / 2022-10-09
### Features
- Feature `IHazRelease.ReleaseNameVersion` is disable
### Updated
- Update `ricaun.Nuke` to `1.4.0`

## [1.3.5] / 2022-08-12
### Updated
- Update `ricaun.Nuke` - SignFile `base64`
- Update `ci`

## [1.3.4] / 2022-07-19
### Updated
- Update/Fix ReleaseNotes, GetReleaseNotes

## [1.3.3] / 2022-07-19
### Updated
- Updated version with `ricaun.Nuke`

## [1.3.2] / 2022-06-21
### Updated
- Remove null in RevitProjectAddInsBuilder

## [1.3.1] / 2022-06-15
- Update to net6.0

## [1.3.0] / 2022-06-14
### Update
- Update ricaun.Nuke
- Update to Visual Studio 2022 - net6.0

## [1.2.6] / 2022-05-23
### Added
- Add `HasRevitVersion` on `RevitExtension`
### Fixed
- Add `.addin` only if `HasRevitVersion` 
### Changed
- Change `resources.dll` with `HasRevitVersion`

## [1.2.5] / 2022-05-23
### Changed
- Change `.bundle.zip` to include BaseDirectory (Forge Compatibility)

## [1.2.4] / 2022-05-23
### Added
- Add Type on Addin Creator - Enable DBApplication feature

## [1.2.3] / 2022-05-14
### Fixed
- Update ricaun.Nuke to fix Error IPack without GitRepository

## [1.2.2] / 2022-04-06
### Changed
- Add Version on `bundle.zip` and `PackageBuilder.zip`
- Change `PackageBuilder`, TriggeredBy `Sign`

## [1.2.1] / 2022-04-06
### Changed
- Normalize version with `ricaun.Nuke`
- Sign `.dll` when create `.addin`

## [1.2.0] / 2022-04-04
### Changed
- Normalize version with `ricaun.Nuke`
### Fixed
- Sign Error TimeStamp Server

## [1.1.9] / 2022-03-24
### Fixed
- Fix `resources.dll` problem, remove from `.addin`
### Added
- Add `CreateRevitAddinOnProjectFiles` on `IRevitPackageBuilder`
- Add Exemple Resource `pt-BR`

## [1.1.8] / 2022-03-23
### Changed
- Change `PackageBuilder`, Before `Sign`

## [1.1.7] / 2022-03-23
### Changed
- On IHazInstallationFiles change `Folder` to `InstallationFiles`
- `InstallationFiles` download files if has a valid url

## [1.1.6] / 2022-03-18
### Changed
- Force `IRevitPackageBuilder` to compile if different from MainProject
- Change NewVersion default `true`
- Change ProjectVersionFolder default `true`
- Change ProjectNameFolder default `true`
- Change ReleasePackageBuilder default `true`
- Change ReleaseBundle default `true`

## [1.1.5] / 2022-03-10
### Changed
- IHazPackageBuilderProject.Name works with EndWith
### Bug Fixes
- Path Length 260 Limitation (Create a temp folder)

## [1.1.4] / 2022-03-09
- Add ProjectNameFolder Option
- Add ProjectVersionFolder Option
- Update Example Project

## [1.1.3] / 2022-03-08
- Add ReleaseBundle Option

## [1.1.2] / 2022-02-23
- Update Project Example
- Update ricaun.Nuke

## [1.1.1] / 2022-02-23
- Update ricaun.Nuke

## [1.1.0] / 2022-02-15
- Update Example Add ricaun.Revit.UI
- Update ricaun.Nuke
- Update Example to *.1.0
- Remove ValueInjectionUtility
- Update to 6.0.0 Nuke.Common

## [1.0.1] / 2022-01-19
- Clear Example Version
- Change product to Title Name

## [1.0.0] / 2022-01-19
- Update to public Repo

## [0.0.13] / 2022-01-12
- Add `NewVersions`
- Add `IHazRevitPackageBuilder`

## [0.0.12] / 2022-01-04
- Add Environment Variables
- Remove rtf files
- Change IssConfiguration to License.txt

## [0.0.11] / 2022-01-03
- Clear IssRevitBuilder
- Add IssLanguageLicence
- Add IssLanguage
- Add IssConfiguration
- Add Files on Installation Example
- Clear IssRevitBuilder
- Update Readme

## [0.0.10] / 2021-12-22
- Add [Parameter] again... (null error)
- Fix Vendor `null` error
- Remove [Parameter]
- Add VendorDescription
- Add VendorId
- Remove Projects
- Add GetApplication

## [0.0.9] / 2021-12-21
- Add ReleasePackageBuilder 

## [0.0.8] / 2021-12-21
- Test and Release
- Update `RevitAddin.PackageBuilder.Example` to version *.0.2 
- Add Documentation on the main project
- Add `xml` DocumentationFile
- Force to `ricaun.Nuke` Version - 0.0.12
- Change `Content` to `Release` on Example

## [0.0.7] / 2021-12-17
- Exemple Update Version
- Add icon.ico
- Copy InstallationFiles
- Add IHazInstallationFiles
- Add Icon/Image/License on IssRevitBuilder
- Remove `.addin` Example
- Set IHazMainProject on IHazPackageBuilderProject `Name`

## [0.0.6] / 2021-12-17
- Change name to `IRevitPackageBuilder`
- Test Example Release
- Add Configuration on Solution
- Add IHazInput
- Add Example
- Change `IExternalApplication` base name to `App`
- Add IHazPackageBuilderProject

## [0.0.5] / 2021-12-09
- Remove Parse Folder
- Add RevitExtension
- Auto detect Revit Version

## [0.0.4] / 2021-12-09
- Update Readme Add Label Nuget

## [0.0.3] / 2021-12-08
- Change `NugetApiUrl: ${{ secrets.NUGET_API_URL }}`
- Change `NugetApiKey: ${{ secrets.NUGET_API_KEY }}`
- Change To `https://api.nuget.org/v3/index.json`

## [0.0.3] / 2021-12-08
- ricaun.Nuke

## [0.0.2] / 2021-12-08
- Update IPublishRevit to public
- Update Readme

## [0.0.1] / 2021-12-08
- First Release

[vNext]: ../../compare/1.0.0...HEAD
[1.6.2]: ../../compare/1.6.1...1.6.2
[1.6.1]: ../../compare/1.6.0...1.6.1
[1.6.0]: ../../compare/1.5.1...1.6.0
[1.5.1]: ../../compare/1.5.0...1.5.1
[1.5.0]: ../../compare/1.4.3...1.5.0
[1.4.3]: ../../compare/1.4.2...1.4.3
[1.4.2]: ../../compare/1.4.1...1.4.2
[1.4.1]: ../../compare/1.4.0...1.4.1
[1.4.0]: ../../compare/1.3.5...1.4.0
[1.3.5]: ../../compare/1.3.4...1.3.5
[1.3.4]: ../../compare/1.3.3...1.3.4
[1.3.3]: ../../compare/1.3.2...1.3.3
[1.3.2]: ../../compare/1.3.1...1.3.2
[1.3.1]: ../../compare/1.3.0...1.3.1
[1.3.0]: ../../compare/1.2.6...1.3.0
[1.2.6]: ../../compare/1.2.5...1.2.6
[1.2.5]: ../../compare/1.2.4...1.2.5
[1.2.4]: ../../compare/1.2.3...1.2.4
[1.2.3]: ../../compare/1.2.2...1.2.3
[1.2.2]: ../../compare/1.2.1...1.2.2
[1.2.1]: ../../compare/1.2.0...1.2.1
[1.2.0]: ../../compare/1.1.9...1.2.0
[1.1.9]: ../../compare/1.1.8...1.1.9
[1.1.8]: ../../compare/1.1.7...1.1.8
[1.1.7]: ../../compare/1.1.6...1.1.7
[1.1.6]: ../../compare/1.1.5...1.1.6
[1.1.5]: ../../compare/1.1.4...1.1.5
[1.1.4]: ../../compare/1.1.3...1.1.4
[1.1.3]: ../../compare/1.1.2...1.1.3
[1.1.2]: ../../compare/1.1.1...1.1.2
[1.1.1]: ../../compare/1.1.0...1.1.1
[1.1.0]: ../../compare/1.0.1...1.1.0
[1.0.1]: ../../compare/1.0.0...1.0.1
[1.0.0]: ../../compare/0.0.13...1.0.0
[0.0.13]: ../../compare/0.0.12...0.0.13
[0.0.12]: ../../compare/0.0.11...0.0.12
[0.0.11]: ../../compare/0.0.10...0.0.11
[0.0.10]: ../../compare/0.0.9...0.0.10
[0.0.9]: ../../compare/0.0.8...0.0.9
[0.0.8]: ../../compare/0.0.7...0.0.8
[0.0.7]: ../../compare/0.0.6...0.0.7
[0.0.6]: ../../compare/0.0.5...0.0.6
[0.0.5]: ../../compare/0.0.4...0.0.5
[0.0.4]: ../../compare/0.0.3...0.0.4
[0.0.3]: ../../compare/0.0.2...0.0.3
[0.0.2]: ../../compare/0.0.1...0.0.2
[0.0.1]: ../../compare/0.0.1