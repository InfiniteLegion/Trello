using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Trello.Models;
using Trello.Services;

namespace Trello.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController: Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        // GET: api/status
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
    var statuses = _statusService.GetAllStatuses();
    return Ok(statuses);
        }

        // GET: api/status/{id}
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
 var status = _statusService.GetStatusById(id);
    if (status == null)
    {
        return NotFound();
    }
    return Ok(status);
        }

        // POST: api/status
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            _statusService.AddStatus(value);
    return CreatedAtAction(nameof(Get), new { id = value }, value);
        }

        // PUT: api/status/{id}
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            var statusExists = _statusService.StatusExists(id);
    if (!statusExists)
    {
        return NotFound();
    }
    _statusService.UpdateStatus(id, value);
    return NoContent();
        }

        // DELETE: api/status/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
var statusExists = _statusService.StatusExists(id);
    if (!statusExists)
    {
        return NotFound();
    }
    _statusService.DeleteStatus(id);
    return NoContent();
        }
    }
}
