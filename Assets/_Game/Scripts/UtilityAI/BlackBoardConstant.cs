using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UtilityAI
{
    public static class BlackBoardConstant
    {
       public static IEnumerable<string> GetAllKeys()
       {
           var fields = typeof(BlackBoardConstant)
               .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
               .Where(f => f.Name.EndsWith("_RATIO"));
       
           foreach (var field in fields)
           {
               yield return (string)field.GetRawConstantValue();
           }
       }

        #region Entitty

        public const string MANA_AVAILABILITY_RATIO = "mana_availability_ratio";
        public const string REMAIN_MANA_KEY = "remain_mana";
        
        public const string REMAIN_STAMINA_KEY = "remain_stamina";
        public const string OWNER_KEY = "owner";


        public const string ENTITY_HEALTH_FULLNESS_RATIO = "{0}_health_fullness_ratio";
        #endregion

        #region Tile

        public const string TILES_EVALUATION_KEY = "tile_evaluation";
        public const string ENTITY_TO_TILE_DISTANCE_KEY = "{0}_to_tile_distance";
        public const string ENTITY_NEARNESS_RATIO = "{0}_nearness_ratio";

        #endregion
        #region Skill

        public const string SKILL_EVALUATION_KEY = "skill_evaluation";
        public const string MANA_AFFORDABILITY_RATIO = "mana_affordalbility_ratio";
        public const string PHYSICAL_DAMAGE = "physical_damage";
        public const string MAGICAL_DAMAGE = "magical_damage";
        public const string NEUTRAL_DAMAGE = "neutral_damage";

        public const string SKILL_RANGE_AVAILABILITY_RATIO = "{0}_range_avaiability_ratio";

        public const string ENTITY_VISIBILITY_RATIO = "{0}_visibility_ratio";
        #endregion

        public const string ALLY_REASONER = "ally_reasoner";
        public const string SELF_REASONER = "self_reasoner";
        public const string ENEMY_REASONER = "enemy_reasoner";

    }   
}