

using TP.InformationComputation.LayeredArchitecture.Data;
using TP.InformationComputation.LayeredArchitecture.Logic.AbstractLayerInterface;

namespace TP.InformationComputation.LayeredArchitecture.Logic
{

    public class ServiceB : IService
    {
        private readonly DataLayerAbstract _data;
        public IService? Service { get; set; }

        public ServiceB(ServiceC service, DataLayerAbstract data)
        {
            Service = service;
            _data = data;
        }

        public int GetProductCount()
        {
            return Service!.GetProductCount() + _data.GetCategory2Count(); 
        }
    }
}