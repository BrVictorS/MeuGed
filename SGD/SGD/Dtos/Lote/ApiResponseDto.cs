using SGD.Dtos.Response;

namespace SGD.Dtos.Lote
{
    public class ApiResponseDto : ServiceResponse<string>
    {

        public ApiResponseDto()
        {
            
        }

        public ApiResponseDto(string msg)
        {
            base.Mensagem  = msg;
        }
        public ApiResponseDto(string mensagem, bool erro)
        {
            if (erro)
            {
                base.Status = false;
            }
           base.Mensagem = mensagem;
        }
    }
}
