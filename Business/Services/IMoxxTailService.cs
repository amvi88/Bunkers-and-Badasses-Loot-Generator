using Models.Builder;

namespace Business.Services
{
    public interface IMoxxTailService
    {
        ItemWrapper<MoxxTail> RandomizeDrink(BaseFactoryParameters parameters);
    }
}