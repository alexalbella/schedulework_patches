using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MazoScheduleMod;
using RimWorld;
using UnityEngine;
using Verse;

namespace ScheduleEverythingComplexJobsPatch
{
    [StaticConstructorOnStartup]
    public static class ComplexJobsScheduleBootstrap
    {
        private const string ScheduleModId = "Mazo.Schedules";
        private const string ComplexJobsId = "FrozenSnowFox.ComplexJobs";
        private const string HarmonyId = "ScheduleEverythingComplexJobsPatch";
        private const string ComplexJobsSchedulePatchModPackageId = "ScheduleEverything.ComplexJobs.Patch";

        internal static readonly List<WorkTypeDef> ComplexJobsWorkTypes = new List<WorkTypeDef>();

        static ComplexJobsScheduleBootstrap()
        {
            if (!IsModActive(ScheduleModId) || !IsModActive(ComplexJobsId))
            {
                return;
            }

            new Harmony(HarmonyId).PatchAll();
            LongEventHandler.ExecuteWhenFinished(Initialize);
        }

        private static void Initialize()
        {
            RefreshWorkTypesFromDefs();

            if (ComplexJobsWorkTypes.Count == 0)
            {
                return;
            }

            MazoScheduleModSettings settings = MazoScheduleMod.MazoScheduleMod.Settings;
            if (settings != null)
            {
                ComplexJobsSettingsUtility.EnsureSettingsEntries(settings, ComplexJobsWorkTypes);
            }
        }

        internal static void RefreshWorkTypesFromDefs()
        {
            ComplexJobsWorkTypes.Clear();
            foreach (WorkTypeDef workType in DefDatabase<WorkTypeDef>.AllDefsListForReading)
            {
                if (workType?.modContentPack == null)
                {
                    continue;
                }

                if (!string.Equals(workType.modContentPack.PackageId, ComplexJobsId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                ComplexJobsWorkTypes.Add(workType);
            }

            RemoveMissingAssignments();

            if (ComplexJobsWorkTypes.Count == 0)
            {
                return;
            }

            ComplexJobsWorkTypes.Sort((a, b) => string.Compare(
                a?.labelShort ?? a?.label ?? a?.defName,
                b?.labelShort ?? b?.label ?? b?.defName,
                StringComparison.OrdinalIgnoreCase));

            foreach (WorkTypeDef workTypeDef in ComplexJobsWorkTypes)
            {
                EnsureScheduleHookup(workTypeDef);
            }
        }

        internal static void EnsureScheduleHookup(WorkTypeDef workType)
        {
            string timeAssignmentDefName = GetTimeAssignmentDefName(workType);
            if (string.IsNullOrEmpty(timeAssignmentDefName))
            {
                return;
            }

            TimeAssignmentDef timeAssignment = DefDatabase<TimeAssignmentDef>.GetNamedSilentFail(timeAssignmentDefName);
            if (timeAssignment == null)
            {
                timeAssignment = CreateTimeAssignmentDef(workType, timeAssignmentDefName);
                timeAssignment.ResolveReferences();
                timeAssignment.PostLoad();
                DefDatabase<TimeAssignmentDef>.Add(timeAssignment);
            }

            if (!GameComponent_MazoSchedule.AssignmentWorkMap.ContainsKey(timeAssignmentDefName))
            {
                GameComponent_MazoSchedule.AssignmentWorkMap.Add(timeAssignmentDefName, workType);
            }
        }

        private static TimeAssignmentDef CreateTimeAssignmentDef(WorkTypeDef workType, string defName)
        {
            string labelSource = workType.labelShort ?? workType.label ?? workType.defName;
            string label = labelSource?.Trim().ToLowerInvariant() ?? defName.ToLowerInvariant();
            string gerund = workType.gerundLabel ?? labelSource ?? defName;

            Color color = GenerateColorFromName(workType.defName);

            ModContentPack contentPack = ComplexJobsSchedulePatchMod.ContentPack ??
                LoadedModManager.RunningModsListForReading.FirstOrDefault(mod =>
                    string.Equals(mod.PackageId, ComplexJobsSchedulePatchModPackageId, StringComparison.OrdinalIgnoreCase));

            return new TimeAssignmentDef
            {
                defName = defName,
                label = label,
                description = "Schedule time for " + gerund,
                color = color,
                modContentPack = contentPack
            };
        }

        internal static string GetTimeAssignmentDefName(WorkTypeDef workType)
        {
            if (workType == null)
            {
                return null;
            }

            return "Mazo_" + workType.defName;
        }

        private static void RemoveMissingAssignments()
        {
            List<KeyValuePair<string, WorkTypeDef>> staleAssignments = GameComponent_MazoSchedule.AssignmentWorkMap
                .Where(pair => pair.Value?.modContentPack != null &&
                               string.Equals(pair.Value.modContentPack.PackageId, ComplexJobsId, StringComparison.OrdinalIgnoreCase) &&
                               !ComplexJobsWorkTypes.Contains(pair.Value))
                .ToList();

            if (staleAssignments.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<string, WorkTypeDef> staleAssignment in staleAssignments)
            {
                GameComponent_MazoSchedule.AssignmentWorkMap.Remove(staleAssignment.Key);

                TimeAssignmentDef existingDef = DefDatabase<TimeAssignmentDef>.GetNamedSilentFail(staleAssignment.Key);
                if (existingDef != null && existingDef.modContentPack == ComplexJobsSchedulePatchMod.ContentPack)
                {
                    DefDatabase<TimeAssignmentDef>.Remove(existingDef);
                }
            }
        }

        private static Color GenerateColorFromName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new Color(0.35f, 0.35f, 0.35f);
            }

            int hash = GenText.StableStringHash(name);
            float hue = (hash % 360) / 360f;
            float saturation = 0.55f + (hash % 97) / 400f;
            float value = 0.65f + (hash % 53) / 300f;
            Color color = Color.HSVToRGB(hue, Mathf.Clamp01(saturation), Mathf.Clamp01(value));
            color.a = 1f;
            return color;
        }

        private static bool IsModActive(string packageId)
        {
            return LoadedModManager.RunningModsListForReading.Any(mod =>
                string.Equals(mod.PackageId, packageId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
