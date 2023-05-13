using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

using RestAPI.Services;
using Newtonsoft.Json.Linq;

namespace RestAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController()
    {
        _orderService = new OrderService();
    }

    [HttpGet("")]
    async public Task<IActionResult> Get()
    {
        try
        {
            DataTable dt = await _orderService.Get();
            if (dt.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dt));
            else
                return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("")]
    public IActionResult Post([FromBody] JsonElement value)
    {
        try
        {
            string msg = String.Empty;
            int querySuccess = _orderService.Post(value);
            if (querySuccess == 1)
                msg = "Record inserted with the value as " + value;
            else
                msg = "Try again. No data inserted";

            return Ok(msg);
        }
        catch (Exception ex) 
        {
            return StatusCode(404, ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] JsonElement value)
    {
        try
        {
            string msg = String.Empty;
            _orderService.Put(id, value);
            msg = "Record updated with the value as " + value + "\nand id as " + id;
            return Ok(msg);
        }
        catch (Exception ex)
        {
            return StatusCode(404, ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            string msg = String.Empty;
            _orderService.Delete(id);
            msg = "Record deleted with the id as " + id;
            return Ok(msg);
        }
        catch (Exception ex)
        {
            return StatusCode(404, ex.Message);
        }
    }
}




