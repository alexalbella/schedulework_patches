using System;
using HarmonyLib;
using Verse;

namespace MazoScheduleMod
{
	// Token: 0x02000008 RID: 8
	[StaticConstructorOnStartup]
	public static class TimetableFixPatch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000256F File Offset: 0x0000076F
		static TimetableFixPatch()
		{
			new Harmony("MazoScheduleMod.TimetableFix").PatchAll();
		}
	}
}
