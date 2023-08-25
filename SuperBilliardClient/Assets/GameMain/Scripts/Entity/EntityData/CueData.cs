namespace SuperBilliard
{
    public class CueData : EntityData
    {
        public CueData(int entityId)
        {
            _serilzieId = entityId;
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}