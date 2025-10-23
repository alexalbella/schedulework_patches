using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MazoCleaningMod
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(JobGiver_Work), "GetPriority")]
	public static class Patch_JobGiver_Work_GetPriority
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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
				WorkTypeDef workTypeDef;
				bool flag2 = Patch_JobGiver_Work_GetPriority.AssignmentToWorkType.TryGetValue(defName, out workTypeDef);
				if (flag2)
				{
					bool flag3 = pawn.workSettings != null && pawn.workSettings.GetPriority(workTypeDef) > 0;
					if (flag3)
					{
						__result = 9f;
					}
					else
					{
						__result = 0f;
					}
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		private static Dictionary<string, WorkTypeDef> AssignmentToWorkType = new Dictionary<string, WorkTypeDef>
		{
			{
				"Mazo_Firefight",
				WorkTypeDefOf.Firefighter
			},
			{
				"Mazo_Patient",
				DefDatabase<WorkTypeDef>.GetNamed("Patient", true)
			},
			{
				"Mazo_Doctor",
				WorkTypeDefOf.Doctor
			},
			{
				"Mazo_Bedrest",
				DefDatabase<WorkTypeDef>.GetNamed("PatientBedRest", true)
			},
			{
				"Mazo_Childcare",
				WorkTypeDefOf.Childcare
			},
			{
				"Mazo_Basic",
				DefDatabase<WorkTypeDef>.GetNamed("BasicWorker", true)
			},
			{
				"Mazo_Warden",
				WorkTypeDefOf.Warden
			},
			{
				"Mazo_Handle",
				WorkTypeDefOf.Handling
			},
			{
				"Mazo_Cook",
				DefDatabase<WorkTypeDef>.GetNamed("Cooking", true)
			},
			{
				"Mazo_Hunt",
				WorkTypeDefOf.Hunting
			},
			{
				"Mazo_Construct",
				WorkTypeDefOf.Construction
			},
			{
				"Mazo_Grow",
				WorkTypeDefOf.Growing
			},
			{
				"Mazo_Mine",
				WorkTypeDefOf.Mining
			},
			{
				"Mazo_PlantCut",
				WorkTypeDefOf.PlantCutting
			},
			{
				"Mazo_Smith",
				WorkTypeDefOf.Smithing
			},
			{
				"Mazo_Tailor",
				DefDatabase<WorkTypeDef>.GetNamed("Tailoring", true)
			},
			{
				"Mazo_Art",
				DefDatabase<WorkTypeDef>.GetNamed("Art", true)
			},
			{
				"Mazo_Craft",
				WorkTypeDefOf.Crafting
			},
			{
				"Mazo_Haul",
				WorkTypeDefOf.Hauling
			},
			{
				"Mazo_Clean",
				WorkTypeDefOf.Cleaning
			},
			{
				"Mazo_Research",
				WorkTypeDefOf.Research
			}
		};
	}
}
