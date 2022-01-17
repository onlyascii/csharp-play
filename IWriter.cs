namespace Architecture
{
    public interface IWriter<T>
    {
        void Write(T data);
    }
}