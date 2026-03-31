using System.Collections.Generic;
using GameCore.Domain.Skill;
using VitalRouter;

namespace Terramorphers.Command
{
    public class SetSkillApplicableTilesCommand : ICommand
    {
        public TerramorphersEntity Entity { get; set; }
        public IReadOnlyList<ESkillTargetType> SkillTargetTypes { get; set; } 
      
        public int Distance { get; set; }
    }
}