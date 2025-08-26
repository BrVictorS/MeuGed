using SGD.Dtos.Response;

namespace SGD.Dtos.Lote
{
    public class ApiResponseDto : ServiceResponse<bool>
    {
        public bool status { get { return base.Status; } set { base.Status = value; } }
        public string msg { get { return base.Mensagem; }set { base.Mensagem = value; } }
    }
}
