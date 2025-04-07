using TP.InformationComputation.LayeredArchitecture.Data;

namespace TP.InformationComputation.LayeredArchitecture.Logic.AbstractLayerInterface
{
    public abstract class LayerFactory
    {
        public static ILogic CreateLayer(DataLayerAbstract? data = default(DataLayerAbstract))
        {
            return new BusinessLogic(data == null ? DataLayerAbstract.CreateLinq2SQL() : data);
        }

        /// <summary>
        /// Class BusinessLogic - encapsulated implementation of the business logic layer
        /// </summary>
        private class BusinessLogic : ILogic
        {
            #region constructors

            public BusinessLogic(DataLayerAbstract dataLayerAPI)
            {
                MyDataLayer = dataLayerAPI;
                MyDataLayer.Connect();

                // Create the service chain with dependency injection
                var serviceC = new ServiceC(MyDataLayer);
                var serviceB = new ServiceB(serviceC, MyDataLayer);
                var serviceA = new ServiceA(serviceB);

                // Set up circular reference
                NextService = serviceA;
                NextService.Service.Service = NextService;
            }

            #endregion constructors

            #region ILogic

            public IService? NextService { get; private set; }

            #endregion ILogic

            #region private

            private readonly DataLayerAbstract MyDataLayer;

            #endregion private
        }
    }
}