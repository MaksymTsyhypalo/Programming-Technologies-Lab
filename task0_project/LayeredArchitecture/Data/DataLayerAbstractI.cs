namespace TP.InformationComputation.LayeredArchitecture.Data
{
    public abstract class DataLayerAbstract
    {
        // Add these new methods:
        public abstract int GetCategory1Count();
        public abstract int GetCategory2Count();

        // Keep existing Connect() and factory method:
        public abstract void Connect();
        public static DataLayerAbstract CreateLinq2SQL() => new DataLayerImplementation();

        private class DataLayerImplementation : DataLayerAbstract
        {
            public override void Connect() { /*...*/ }

            // Implement new methods:
            public override int GetCategory1Count() => 10; // Hardcoded example
            public override int GetCategory2Count() => 5;  // Hardcoded example
        }
    }
}