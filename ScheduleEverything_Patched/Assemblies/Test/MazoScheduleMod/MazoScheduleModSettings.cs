using System;
using System.Collections.Generic;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x0200000B RID: 11
	public class MazoScheduleModSettings : ModSettings
	{
		// Token: 0x0400001C RID: 28
		public readonly List<string> worktypeOrder = new List<string>
		{
			"Mazo_Firefight",
			"Mazo_Patient",
			"Mazo_Doctor",
			"Mazo_Bedrest",
			"Mazo_Childcare",
			"Mazo_Basic",
			"Mazo_Warden",
			"Mazo_Handle",
			"Mazo_Cook",
			"Mazo_Hunt",
			"Mazo_Construct",
			"Mazo_Grow",
			"Mazo_Mine",
			"Mazo_PlantCut",
			"Mazo_Smith",
			"Mazo_Tailor",
			"Mazo_Art",
			"Mazo_Craft",
			"Mazo_Haul",
			"Mazo_Clean",
			"Mazo_Research"
		};

		// Token: 0x0400001D RID: 29
		public Dictionary<string, bool> worktypeEnabled = new Dictionary<string, bool>
		{
			{
				"Mazo_Firefight",
				false
			},
			{
				"Mazo_Patient",
				false
			},
			{
				"Mazo_Doctor",
				false
			},
			{
				"Mazo_Bedrest",
				false
			},
			{
				"Mazo_Childcare",
				false
			},
			{
				"Mazo_Basic",
				false
			},
			{
				"Mazo_Warden",
				false
			},
			{
				"Mazo_Handle",
				true
			},
			{
				"Mazo_Cook",
				true
			},
			{
				"Mazo_Hunt",
				true
			},
			{
				"Mazo_Construct",
				true
			},
			{
				"Mazo_Grow",
				true
			},
			{
				"Mazo_Mine",
				true
			},
			{
				"Mazo_PlantCut",
				true
			},
			{
				"Mazo_Smith",
				true
			},
			{
				"Mazo_Tailor",
				true
			},
			{
				"Mazo_Art",
				true
			},
			{
				"Mazo_Craft",
				true
			},
			{
				"Mazo_Haul",
				true
			},
			{
				"Mazo_Clean",
				true
			},
			{
				"Mazo_Research",
				true
			}
		};

		// Token: 0x0400001E RID: 30
		public Dictionary<string, int> worktypeTargetPriority = new Dictionary<string, int>();
	}
}
