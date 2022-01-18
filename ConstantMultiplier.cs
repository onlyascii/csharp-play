namespace Architecture
{
    public class ConstantMultiplier : IManipulator<int, int>
    {
        private readonly int _constantValue;

        public ConstantMultiplier(IConfig config)
        {
            _constantValue = config.ConstantValue;
        }

        public int Manipulate(int data)
        {
            return data * _constantValue;
        }
    }
}