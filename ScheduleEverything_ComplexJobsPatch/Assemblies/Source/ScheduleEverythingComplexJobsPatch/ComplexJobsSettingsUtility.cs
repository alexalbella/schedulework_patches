using System.Collections.Generic;
using System.Linq;
using MazoScheduleMod;
using RimWorld;
using UnityEngine;
using Verse;

namespace ScheduleEverythingComplexJobsPatch
{
    internal static class ComplexJobsSettingsUtility
    {
        internal static void EnsureSettingsEntries(MazoScheduleModSettings settings, IEnumerable<WorkTypeDef> workTypes)
        {
            if (settings == null || workTypes == null)
            {
                return;
            }

            List<WorkTypeDef> workTypeList = workTypes
                .Where(workType => workType != null)
                .ToList();

            if (workTypeList.Count == 0)
            {
                ComplexJobsScheduleBootstrap.RefreshWorkTypesFromDefs();
                workTypeList = ComplexJobsScheduleBootstrap.ComplexJobsWorkTypes
                    .Where(workType => workType != null)
                    .ToList();
            }

            HashSet<string> validAssignmentKeys = new HashSet<string>(GameComponent_MazoSchedule.AssignmentWorkMap.Keys);

            settings.worktypeOrder.RemoveAll(key => !validAssignmentKeys.Contains(key));
            PruneDictionary(settings.worktypeEnabled, validAssignmentKeys);
            PruneDictionary(settings.worktypeTargetPriority, validAssignmentKeys);

            if (workTypeList.Count == 0)
            {
                return;
            }

            foreach (WorkTypeDef workType in workTypeList)
            {
                string assignmentDefName = ComplexJobsScheduleBootstrap.GetTimeAssignmentDefName(workType);
                if (string.IsNullOrEmpty(assignmentDefName))
                {
                    continue;
                }

                if (!settings.worktypeOrder.Contains(assignmentDefName))
                {
                    settings.worktypeOrder.Add(assignmentDefName);
                }

                EnsureDefaultSettings(settings, assignmentDefName, workType);
            }

            foreach (string assignmentKey in validAssignmentKeys)
            {
                if (!settings.worktypeEnabled.ContainsKey(assignmentKey) || !settings.worktypeTargetPriority.ContainsKey(assignmentKey))
                {
                    GameComponent_MazoSchedule.AssignmentWorkMap.TryGetValue(assignmentKey, out WorkTypeDef workType);
                    EnsureDefaultSettings(settings, assignmentKey, workType);
                }
            }
        }

        private static void EnsureDefaultSettings(MazoScheduleModSettings settings, string assignmentKey, WorkTypeDef workType)
        {
            if (!settings.worktypeEnabled.ContainsKey(assignmentKey))
            {
                settings.worktypeEnabled.Add(assignmentKey, ShouldEnableByDefault(workType));
            }

            if (!settings.worktypeTargetPriority.ContainsKey(assignmentKey))
            {
                settings.worktypeTargetPriority.Add(assignmentKey, GetDefaultPriority(workType));
            }
        }

        private static bool ShouldEnableByDefault(WorkTypeDef workType)
        {
            if (workType == null)
            {
                return true;
            }

            if (workType.alwaysStartActive)
            {
                return true;
            }

            return workType.visible;
        }

        private static int GetDefaultPriority(WorkTypeDef workType)
        {
            if (workType == null)
            {
                return 3;
            }

            float scaled = Mathf.InverseLerp(350f, 1600f, workType.naturalPriority);
            int rounded = Mathf.Clamp(Mathf.RoundToInt(scaled * 3f) + 1, 1, 4);
            return rounded;
        }

        private static void PruneDictionary<T>(Dictionary<string, T> dictionary, HashSet<string> validKeys)
        {
            List<string> keysToRemove = dictionary.Keys
                .Where(key => !validKeys.Contains(key))
                .ToList();

            foreach (string key in keysToRemove)
            {
                dictionary.Remove(key);
            }
        }
    }
}
