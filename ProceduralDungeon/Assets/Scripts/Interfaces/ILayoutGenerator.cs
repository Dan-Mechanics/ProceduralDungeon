namespace ProceduralDungeon
{
    public interface ILayoutGenerator
    {
        public TileType[,] Generate(Blackboard blackboard);
    }
}