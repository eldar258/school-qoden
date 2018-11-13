using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    // TODO 5: unauthorized users should receive 401 status code and should be redirected to Login endpoint   
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public ValueTask<Account> Get()
        {
            return _service.LoadOrCreateAsync(null /* TODO 3: Get user id from cookie */);
        }
        
        //TODO 6: Get user id from cookie
        //TODO 7: Endpoint should works only for users with "Admin" Role
        [Authorize]
        [HttpGet]
        public Account GetByInternalId([FromQuery] int internalId)
        {
            return _service.GetFromCache(internalId);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task.
            var account = await Get();
            account.Counter++;            
        }
    }
}