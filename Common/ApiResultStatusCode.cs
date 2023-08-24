using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Success")]
        Success = 0,

        [Display(Name = "Server Error")]
        ServerError = 1,

        [Display(Name = "Bad Request")]
        BadRequest = 2,

        [Display(Name = "Not Found")]
        NotFound = 3,

        [Display(Name = "List Empty")]
        ListEmpty = 4,

        [Display(Name = "Server Logic Error")]
        LogicError = 5,

        [Display(Name = "UnAuthorized")]
        UnAuthorized = 6
    }
}
