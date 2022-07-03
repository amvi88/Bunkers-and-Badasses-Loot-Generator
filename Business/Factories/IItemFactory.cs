using Business.Models.Builder;

namespace Business.Factories
{
    public interface IItemFactory<I, P> 
        where I: Item 
        where P: BaseFactoryParameters
    {
        public I Manufacture(P factoryParameters);
    }

}