using System.Web.Http;

namespace MoviesApi2022.Services
{
    public class ActorService
    {
    }


    [Route("/error")]
    public class Error
    {
        public string error()
        {
            return "Trang bi loi";
        }
    }
}
