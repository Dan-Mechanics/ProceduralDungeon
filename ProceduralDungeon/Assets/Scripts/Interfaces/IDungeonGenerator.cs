namespace ProceduralDungeon
{
    public interface IDungeonGenerator
    {
        public TileType[,] Generate(Blackboard blackboard); 
    }
}