using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Methods;
using Microsoft.AspNetCore.Cors;

namespace MatrixSolverWebAPI.Controllers
{
    [EnableCors("AllowMethods")]
    [Route("api/solve")]
    [ApiController]
    public class MatrixSolverController : ControllerBase
    {
        
        [HttpPost("matrixmethod")]
        public async Task<IActionResult> MatrixMethodAsync([FromBody] ReceivedData data)
        {
            if (data is null || data.A is null || data.B is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "data is null" });
            }
            else
            {
                try
                {
                    Matrix matrixMethods = new Matrix();
                    var result = await matrixMethods.MatrixMethod(data.A, data.B);
                    return Ok(new { result = result, history = matrixMethods.history.ToString()});
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
        }
        [HttpPost("cramersmethod")]
        public IActionResult CramersMethodAsync([FromBody] ReceivedData data)
        {
            if (data is null || data.A is null || data.B is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "data is null" });
            }
            else
            {
                try
                {
                    Matrix matrixMethods = new Matrix();
                    var result = matrixMethods.CramersMethod(data.A, data.B);
                    return Ok(new { result = result, history = matrixMethods.history.ToString() });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }
            }
        }
    }
}
public class ReceivedData
{
    public double[][]? A { get; set; }

    public double[]? B { get; set; }
    public string? Method { get; set; }
}