namespace ProceduralDungeon
{
    public class TileMetadata
    {
        public int neighbours;
        public int steps;
        public int x;
        public int y;

        public TileMetadata(int x, int y, int neighbours)
        {
            this.x = x;
            this.y = y;
            this.neighbours = neighbours;
        }

        public override string ToString() => $"({x},{y}) --> {steps}";
    }
}
