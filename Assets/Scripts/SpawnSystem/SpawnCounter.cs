namespace SpawnSystem
{
    public class SpawnCounter 
    {
        public int SpawnCount { get; private set; }
        
        public SpawnCounter(int spawnCount)
        {
            SpawnCount = spawnCount;
        }
        
        public void IncreaseSpawnCount()
        {
            if (SpawnCount >= 15) return;
            SpawnCount++;
        }
    }
}