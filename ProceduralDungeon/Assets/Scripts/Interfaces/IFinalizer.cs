namespace ProceduralDungeon
{
    public interface IFinalizer
    {
        public void Finalize(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard); 
    }
}