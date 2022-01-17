namespace Architecture
{
    public interface IManipulator<T, V>
    {
        V Manipulate(T data);
    }
}