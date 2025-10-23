using System;
using UnityEngine;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x0200000C RID: 12
	public class MazoScheduleMod : Mod
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002DB2 File Offset: 0x00000FB2
		public MazoScheduleMod(ModContentPack content) : base(content)
		{
			MazoScheduleMod.ModInstance = this;
			MazoScheduleMod.Settings = base.GetSettings<MazoScheduleModSettings>();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public override void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.GapLine(12f);
			Rect rect = listing_Standard.GetRect(30f, 1f);
			Widgets.Label(rect, "Work types visible / target priority");
			TooltipHandler.TipRegion(rect, "Enable/disable work types and set their default schedule priority (1–4)");
			listing_Standard.Gap(12f);
			foreach (string text in MazoScheduleMod.Settings.worktypeOrder)
			{
				string text2 = text.Replace("Mazo_", "");
				Rect rect2 = listing_Standard.GetRect(24f, 1f);
				Rect rect3;
				rect3..ctor(rect2.x, rect2.y, 100f, rect2.height);
				bool value = MazoScheduleMod.Settings.worktypeEnabled[text];
				Widgets.CheckboxLabeled(rect3, text2 ?? "", ref value, false, null, null, false, false);
				MazoScheduleMod.Settings.worktypeEnabled[text] = value;
				int num2;
				int num = MazoScheduleMod.Settings.worktypeTargetPriority.TryGetValue(text, out num2) ? num2 : 1;
				float num3 = (float)num;
				Rect rect4;
				rect4..ctor(rect3.xMax + 5f, rect2.y + 6f, 130f, 18f);
				num3 = Widgets.HorizontalSlider(rect4, num3, 1f, 4f, false, null, null, null, -1f);
				num = Mathf.RoundToInt(num3);
				MazoScheduleMod.Settings.worktypeTargetPriority[text] = num;
				string text3 = num.ToString();
				Vector2 vector = Text.CalcSize(text3);
				Rect rect5;
				rect5..ctor(rect4.xMax + 5f, rect2.y + (rect2.height - vector.y) / 2f, vector.x + 6f, vector.y);
				Widgets.Label(rect5, text3);
			}
			listing_Standard.End();
			base.DoSettingsWindowContents(inRect);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003004 File Offset: 0x00001204
		public override string SettingsCategory()
		{
			return "Mazo Schedule Mod";
		}

		// Token: 0x0400001F RID: 31
		public static MazoScheduleMod ModInstance;

		// Token: 0x04000020 RID: 32
		public static MazoScheduleModSettings Settings;
	}
}
