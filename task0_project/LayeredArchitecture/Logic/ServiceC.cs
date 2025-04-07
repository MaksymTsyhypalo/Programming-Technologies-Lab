

using TP.InformationComputation.LayeredArchitecture.Data;
using TP.InformationComputation.LayeredArchitecture.Logic.AbstractLayerInterface;

namespace TP.InformationComputation.LayeredArchitecture.Logic
{

    public class ServiceC : IService
    {
        private readonly DataLayerAbstract _data;
        public IService? Service { get; set; }

        public ServiceC(DataLayerAbstract data)
        {
            _data = data;
            _data.Connect();
        }

        public int GetProductCount()
        {
            return _data.GetCategory1Count(); 
        }
    }
}