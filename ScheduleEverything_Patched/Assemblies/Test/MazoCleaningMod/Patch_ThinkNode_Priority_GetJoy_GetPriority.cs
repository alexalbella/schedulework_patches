using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MazoCleaningMod
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(ThinkNode_Priority_GetJoy), "GetPriority")]
	public static class Patch_ThinkNode_Priority_GetJoy_GetPriority
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002284 File Offset: 0x00000484
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
