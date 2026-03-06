namespace ProceduralDungeon
{
    public interface IDungeonDecorator
    {
        public void Decorate(TileType[,] tiles, Blackboard blackboard); 
    }
}