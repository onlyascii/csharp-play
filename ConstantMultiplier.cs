namespace Architecture
{
    public class ConstantMultiplier : IManipulator<int, int>
    {
        private readonly int _constantValue;

        public ConstantMultiplier(IOptions options)
        {
            _constantValue = options.ConstantValue;
        }

        public int Manipulate(int data)
        {
            return data * _constantValue;
        }
    }
}