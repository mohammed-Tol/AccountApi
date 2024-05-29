using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AccountsApiService.Controllers
{
    [Route("")]
    [EnableCors]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IRepository<Account, long> _repository;

        public AccountsController(IRepository<Account, long> repository)
        {
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet(template: "{CustId}")]
        [Authorize(Policy = "CustomerDataAccess")]
        public async Task<IActionResult> GetAllAccountsByCustomerId(int CustId)
        {
            var model = _repository.GetAllAccountsByCustomerID(CustId);
            if(model is null || !model.Any())
            {
                return NotFound("No Customer was found with the provided credentials.");
            }
            return Ok(model);
        }

        // GET: api/Acconts/AccId:5
        [HttpGet(template: "AccId/{AccId}")]
        [Authorize(Policy = "CustomerDataAccess")]
        public async Task<ActionResult<Account>> GetAccountByAccountId(long AccId)
        {
            var model = _repository.GetAccountByAccountID(AccId);
            if (model is null || model.accountID ==0)
            {
                return NotFound("No Account was found with the provided credentials.");
            }
            return model;
        }

        [HttpPost(template: "Create")]
        [Authorize(Policy = "CustomerDataAccess")]
        public ActionResult<Account> CreateAccount(Account model)
        {
            try{
                var id = _repository.CreateAccount(model);
                return Ok(new { message = "Account Added" , AccontId = id});
            }
            catch (SqlException sqlex)
            {
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(template: "Delete")]
        [Authorize(Policy = "CustomerDataAccess")]
        public async Task<ActionResult<Account>> DeleteAccount(long AccId)
        {
            try{
                _repository.DeleteAccountByAccountId(AccId);
                return Ok();
            }
            catch (SqlException sqlex)
            {
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(template: "Cheque")]
        [Authorize(Policy = "CustomerDataAccess")]
        public async Task<ActionResult<Account>> ApplyForCheque(long AccId)
        {
            try
            {
                _repository.ApplyForChequeBook(AccId);
                return Ok();
            }
            catch (SqlException sqlex)
            {
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(template: "Test")]
        [Authorize(Policy = "CustomerDataAccess")]
        public IActionResult Test()
        {
            return Ok("Api Connected and Up!");
        }
    }
}