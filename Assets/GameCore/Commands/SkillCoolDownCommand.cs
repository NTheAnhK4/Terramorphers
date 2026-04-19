using VitalRouter;

namespace GameCore.Commands
{
    public class SkillCoolDownCommand : ICommand
    {
        public int SkillID { get; set; }
        public int CoolDown { get; set; }
    }
}