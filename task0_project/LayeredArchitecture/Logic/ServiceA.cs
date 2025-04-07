

using TP.InformationComputation.LayeredArchitecture.Logic.AbstractLayerInterface;

namespace TP.InformationComputation.LayeredArchitecture.Logic
{

    public class ServiceA : IService
    {
        public IService? Service { get; set; }

        public ServiceA(ServiceB service) => Service = service;

        public int GetProductCount()
        {
            return Service!.GetProductCount(); 
        }
    }
}