using GameCore.Domain.Entity;
using GameCore.Respository.Shared;

namespace GameCore.Respository.Entity
{
    public class EntityRepository : BaseRepository<int, BaseEntityMetadata, IEntityDatabase>, IEntityRepository
    {
        public override string AddressPath => "EntityDatabase";
    }
}