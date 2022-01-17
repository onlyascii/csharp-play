namespace Architecture
{
    public class Squarer : IManipulator<int, int>
    {

        public int Manipulate(int data)
        {
            return data * data;
        }
    }
}