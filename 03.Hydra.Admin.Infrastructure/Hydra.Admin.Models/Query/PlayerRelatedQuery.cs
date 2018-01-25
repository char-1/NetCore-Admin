namespace Hydra.Admin.Models.Query
{
    public class PlayerRelatedQuery : BaseQuery
    {
        public PlayerRelatedQuery()
        {
            this.typeId = 0;
        }
        public int? typeId { get; set; }
        public string playerId { get; set; }
        public string gameId { get; set; }
    }
}
