using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(Pawn_TimetableTracker), "ExposeData")]
	public static class Patch_Pawn_TimetableTracker_ExposeData
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002584 File Offset: 0x00000784
		public static void Postfix(Pawn_TimetableTracker __instance)
		{
			bool flag = __instance.times == null;
			if (flag)
			{
				__instance.times = new List<TimeAssignmentDef>();
			}
			for (int i = 0; i < __instance.times.Count; i++)
			{
				TimeAssignmentDef timeAssignmentDef = __instance.times[i];
				bool flag2 = timeAssignmentDef == null || !DefDatabase<TimeAssignmentDef>.AllDefsListForReading.Contains(timeAssignmentDef);
				if (flag2)
				{
					Log.Warning(string.Format("[MazoScheduleMod] Invalid or missing time assignment at hour {0}, replacing with 'Anything'.", i));
					__instance.times[i] = TimeAssignmentDefOf.Anything;
				}
			}
			while (__instance.times.Count < 24)
			{
				Log.Warning(string.Format("[MazoScheduleMod] Timetable had only {0} entries. Adding 'Anything' for missing hour.", __instance.times.Count));
				__instance.times.Add(TimeAssignmentDefOf.Anything);
			}
			bool flag3 = __instance.times.Count > 24;
			if (flag3)
			{
				Log.Warning(string.Format("[MazoScheduleMod] Timetable had {0} entries. Trimming to 24.", __instance.times.Count));
				__instance.times.RemoveRange(24, __instance.times.Count - 24);
			}
		}
	}
}
