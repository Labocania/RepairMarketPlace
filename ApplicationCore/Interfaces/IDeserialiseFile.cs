namespace RepairMarketPlace.ApplicationCore.Interfaces
{
    public interface IDeserializeFile<T>
    {
        public T DeserializeFile(string filePath);
    }
}
