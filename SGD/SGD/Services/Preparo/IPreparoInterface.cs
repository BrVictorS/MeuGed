using SGD.Dtos.Response;

namespace SGD.Services.Preparo
{
    public interface IPreparoInterface
    {
        ServiceResponse<string> GetProximaEtiqueta();
    }
}
