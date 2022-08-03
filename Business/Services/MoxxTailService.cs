using Business.Factories;
using Models.Builder;

namespace Business.Services
{
    public class MoxxTailService : IMoxxTailService
    {
        private readonly IItemFactory<MoxxTail, BaseFactoryParameters> moxxTailFactory;

        public MoxxTailService(IItemFactory<MoxxTail, BaseFactoryParameters> moxxTailFactory)
        {
            this.moxxTailFactory = moxxTailFactory ?? throw new ArgumentNullException(nameof(moxxTailFactory));
        }

        public ItemWrapper<MoxxTail> RandomizeDrink(BaseFactoryParameters parameters)
        {
            return moxxTailFactory.Manufacture(parameters);
        }
    }
}