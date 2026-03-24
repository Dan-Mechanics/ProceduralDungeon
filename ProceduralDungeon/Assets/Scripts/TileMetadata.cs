namespace ProceduralDungeon
{
    [System.Serializable]
    public class TileMetadata
    {
        public int neighbours;
        public int steps;

        public TileMetadata(int neighbours, int steps)
        {
            this.neighbours = neighbours;
            this.steps = steps;
        }
    }
}
