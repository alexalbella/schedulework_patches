# Building Schedule Everything! – Complex Jobs Bridge

This project targets .NET Framework 4.7.2 and depends on assemblies that ship with RimWorld and Harmony. To compile the DLL:

1. Install the .NET Framework 4.7.2 developer pack (or Visual Studio with MSBuild support).
2. Copy the following assemblies from your RimWorld installation into a `lib/` directory next to this project:
   - `Assembly-CSharp.dll`
   - `UnityEngine.CoreModule.dll`
   - `UnityEngine.IMGUIModule.dll`
   - `UnityEngine.TextRenderingModule.dll`
   - `Verse.dll`
3. Copy `0Harmony.dll` from the Harmony mod (or use the version bundled with RimWorld).
4. Ensure `ScheduleEverything_Patched/Assemblies/Schedules.dll` has been built from the upstream Schedule Everything! project and sits at the relative path referenced in the `.csproj` file.
5. Run `msbuild ScheduleEverythingComplexJobsPatch.csproj /p:Configuration=Release` from this directory.
6. Copy the resulting `bin/Release/ScheduleEverythingComplexJobsPatch.dll` into `ScheduleEverything_ComplexJobsPatch/Assemblies/` for distribution.

> **Note**
> The container used for automated validation does not ship with the proprietary RimWorld assemblies or MSBuild, so compilation must be performed locally with the game installed.
