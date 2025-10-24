using HarmonyLib;
using MazoScheduleMod;

namespace ScheduleEverythingComplexJobsPatch
{
    [HarmonyPatch(typeof(MazoScheduleModSettings))]
    internal static class MazoScheduleModSettingsPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(MethodType.Constructor)]
        private static void AfterConstructor(MazoScheduleModSettings __instance)
        {
            ComplexJobsSettingsUtility.EnsureSettingsEntries(__instance, ComplexJobsScheduleBootstrap.ComplexJobsWorkTypes);
        }
    }
}
