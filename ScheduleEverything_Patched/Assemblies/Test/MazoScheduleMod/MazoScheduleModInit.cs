using System;
using HarmonyLib;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x02000005 RID: 5
	[StaticConstructorOnStartup]
	public static class MazoScheduleModInit
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002359 File Offset: 0x00000559
		static MazoScheduleModInit()
		{
			new Harmony("mazo.schedulemenu").PatchAll();
		}
	}
}
