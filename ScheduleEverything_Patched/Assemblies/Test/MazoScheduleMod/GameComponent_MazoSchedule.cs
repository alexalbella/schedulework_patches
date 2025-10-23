using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x0200000A RID: 10
	public class GameComponent_MazoSchedule : GameComponent
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000026B6 File Offset: 0x000008B6
		public GameComponent_MazoSchedule(Game game)
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026E0 File Offset: 0x000008E0
		public override void GameComponentTick()
		{
			int num = GenLocalDate.HourOfDay(Find.CurrentMap);
			bool flag = num == this.lastHour;
			if (!flag)
			{
				this.lastHour = num;
				this.ApplyPrioritiesForCurrentHour();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002718 File Offset: 0x00000918
		private void ApplyPrioritiesForCurrentHour()
		{
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned.ToList<Pawn>())
			{
				bool flag = ((pawn != null) ? pawn.workSettings : null) == null || !pawn.workSettings.EverWork || pawn.timetable == null;
				if (!flag)
				{
					TimeAssignmentDef currentAssignment = pawn.timetable.CurrentAssignment;
					bool flag2 = currentAssignment == null || string.IsNullOrEmpty(currentAssignment.defName);
					if (!flag2)
					{
						foreach (KeyValuePair<string, WorkTypeDef> keyValuePair in GameComponent_MazoSchedule.AssignmentWorkMap)
						{
							string key = keyValuePair.Key;
							WorkTypeDef value = keyValuePair.Value;
							bool flag3 = currentAssignment.defName == key;
							if (flag3)
							{
								this.ApplyPriority(pawn, value);
							}
							else
							{
								this.RestorePriority(pawn, value);
							}
						}
					}
				}
			}
			foreach (ValueTuple<Pawn, WorkTypeDef> key2 in this.keysToRemove)
			{
				this.originalPriorities.Remove(key2);
			}
			this.keysToRemove.Clear();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000028A0 File Offset: 0x00000AA0
		private void ApplyPriority(Pawn pawn, WorkTypeDef workType)
		{
			ValueTuple<Pawn, WorkTypeDef> key = new ValueTuple<Pawn, WorkTypeDef>(pawn, workType);
			bool flag = !this.originalPriorities.ContainsKey(key);
			if (flag)
			{
				this.originalPriorities[key] = pawn.workSettings.GetPriority(workType);
			}
			bool flag2 = !pawn.WorkTypeIsDisabled(workType);
			if (flag2)
			{
				string key2 = GameComponent_MazoSchedule.AssignmentWorkMap.FirstOrDefault((KeyValuePair<string, WorkTypeDef> kv) => kv.Value == workType).Key;
				int num;
				bool flag3 = key2 != null && MazoScheduleMod.Settings.worktypeTargetPriority.TryGetValue(key2, out num);
				if (flag3)
				{
					pawn.workSettings.SetPriority(workType, num);
				}
				else
				{
					pawn.workSettings.SetPriority(workType, 1);
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002980 File Offset: 0x00000B80
		private void RestorePriority(Pawn pawn, WorkTypeDef workType)
		{
			ValueTuple<Pawn, WorkTypeDef> valueTuple = new ValueTuple<Pawn, WorkTypeDef>(pawn, workType);
			int num;
			bool flag = this.originalPriorities.TryGetValue(valueTuple, out num);
			if (flag)
			{
				pawn.workSettings.SetPriority(workType, num);
				this.keysToRemove.Add(valueTuple);
			}
		}

		// Token: 0x04000018 RID: 24
		private readonly Dictionary<ValueTuple<Pawn, WorkTypeDef>, int> originalPriorities = new Dictionary<ValueTuple<Pawn, WorkTypeDef>, int>();

		// Token: 0x04000019 RID: 25
		private readonly List<ValueTuple<Pawn, WorkTypeDef>> keysToRemove = new List<ValueTuple<Pawn, WorkTypeDef>>();

		// Token: 0x0400001A RID: 26
		private int lastHour = -1;

		// Token: 0x0400001B RID: 27
		internal static readonly Dictionary<string, WorkTypeDef> AssignmentWorkMap = new Dictionary<string, WorkTypeDef>
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
				DefDatabase<WorkTypeDef>.GetNamed("Childcare", true)
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
				"Mazo_Haul",
				WorkTypeDefOf.Hauling
			},
			{
				"Mazo_Clean",
				WorkTypeDefOf.Cleaning
			},
			{
				"Mazo_Craft",
				WorkTypeDefOf.Crafting
			},
			{
				"Mazo_Research",
				WorkTypeDefOf.Research
			}
		};
	}
}
