namespace ProceduralDungeon
{
    [System.Serializable]
    public class TileMetadata
    {
        public int neighbours;
        public int stepsToGetThere;

        public TileMetadata(int stepsToGetThere)
        {
            this.stepsToGetThere = stepsToGetThere;
        }
    }
}
