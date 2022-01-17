namespace Architecture
{
    public class ConstantMultiplier : IManipulator<int, int>
    {
        private readonly int _constantValue;

        public ConstantMultiplier(int constantValue)
        {
            _constantValue = constantValue;
        }

        public int Manipulate(int data)
        {
            return data * _constantValue;
        }
    }
}