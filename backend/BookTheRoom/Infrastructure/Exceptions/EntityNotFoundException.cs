namespace Infrastructure.Exceptions
{
    public class EntityNotFoundException<T> : Exception where T : class
    {
        private readonly string typeName;
        public EntityNotFoundException() : base()
        {
            typeName = typeof(T).Name;
        }

        public override string Message => $"Object of {typeName} type is not found.";
    }
}
