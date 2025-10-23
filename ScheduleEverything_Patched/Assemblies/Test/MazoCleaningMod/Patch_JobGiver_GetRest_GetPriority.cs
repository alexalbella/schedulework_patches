using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MazoCleaningMod
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(JobGiver_GetRest), "GetPriority")]
	public static class Patch_JobGiver_GetRest_GetPriority
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000022F0 File Offset: 0x000004F0
		private static bool Prefix(Pawn pawn, ref float __result)
		{
			bool flag = pawn == null || pawn.timetable == null || pawn.timetable.CurrentAssignment == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				string defName = pawn.timetable.CurrentAssignment.defName;
				bool flag2 = defName != null && defName.StartsWith("Mazo_");
				if (flag2)
				{
					__result = 0f;
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}
}
