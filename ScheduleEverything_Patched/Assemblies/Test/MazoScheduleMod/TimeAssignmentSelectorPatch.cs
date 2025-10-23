using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(TimeAssignmentSelector), "DrawTimeAssignmentSelectorGrid")]
	public static class TimeAssignmentSelectorPatch
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000236C File Offset: 0x0000056C
		public static void Postfix(Rect rect)
		{
			MazoScheduleModSettings settings = MazoScheduleMod.Settings;
			bool flag = settings == null;
			if (!flag)
			{
				bool flag2 = TimeAssignmentSelectorPatch.selectedMazoDef == null;
				if (flag2)
				{
					TimeAssignmentSelectorPatch.selectedMazoDef = TimeAssignmentDefOf_Mazo.Mazo_Clean;
				}
				Rect rect2 = rect;
				rect2.width /= 2f;
				rect2.height /= 2f;
				rect2.x += (ModsConfig.RoyaltyActive ? (rect2.width * 6f) : (rect2.width * 5f));
				Rect rect3 = GenUI.ContractedBy(rect2, 2f);
				GUI.DrawTexture(rect3, TimeAssignmentSelectorPatch.selectedMazoDef.ColorTexture);
				bool flag3 = Mouse.IsOver(rect3);
				if (flag3)
				{
					Widgets.DrawHighlight(rect3);
				}
				Text.Anchor = 4;
				Widgets.Label(rect3, TimeAssignmentSelectorPatch.selectedMazoDef.LabelCap);
				Text.Anchor = 0;
				bool flag4 = TimeAssignmentSelector.selectedAssignment == TimeAssignmentSelectorPatch.selectedMazoDef;
				if (flag4)
				{
					Widgets.DrawBox(rect3, 2, null);
				}
				bool flag5 = Widgets.ButtonInvisible(rect3, true);
				if (flag5)
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (string text in settings.worktypeOrder)
					{
						bool flag7;
						bool flag6 = !settings.worktypeEnabled.TryGetValue(text, out flag7) || !flag7;
						if (!flag6)
						{
							TimeAssignmentDef def = DefDatabase<TimeAssignmentDef>.GetNamedSilentFail(text);
							bool flag8 = def == null;
							if (!flag8)
							{
								list.Add(new FloatMenuOption(def.LabelCap, delegate()
								{
									TimeAssignmentSelectorPatch.SetMazoAssignment(def);
								}, 4, null, null, 0f, null, null, true, 0));
							}
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002560 File Offset: 0x00000760
		private static void SetMazoAssignment(TimeAssignmentDef def)
		{
			TimeAssignmentSelectorPatch.selectedMazoDef = def;
			TimeAssignmentSelector.selectedAssignment = def;
		}

		// Token: 0x04000017 RID: 23
		private static TimeAssignmentDef selectedMazoDef;
	}
}
