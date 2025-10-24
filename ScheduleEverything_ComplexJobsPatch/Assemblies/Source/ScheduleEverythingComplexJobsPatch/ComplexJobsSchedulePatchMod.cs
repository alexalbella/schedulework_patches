using Verse;

namespace ScheduleEverythingComplexJobsPatch
{
    public class ComplexJobsSchedulePatchMod : Mod
    {
        public static ModContentPack ContentPack { get; private set; }

        public ComplexJobsSchedulePatchMod(ModContentPack content) : base(content)
        {
            ContentPack = content;
        }
    }
}
