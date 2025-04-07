//__________________________________________________________________________________________
//
//  Copyright 2022 Mariusz Postol LODZ POLAND.

namespace TP.InformationComputation.LayeredArchitecture.Logic.AbstractLayerInterface
{
    public interface IService
    {
        IService? Service { get; set; }
        int GetProductCount();
    }
}