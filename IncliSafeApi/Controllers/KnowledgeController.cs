using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IncliSafeApi.Data;
using IncliSafe.Shared.Models.Patterns;
using IncliSafeApi.Services.Interfaces;

namespace IncliSafeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KnowledgeController : ControllerBase
    {
        private readonly IKnowledgeBaseService _knowledgeService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KnowledgeController> _logger;

        public KnowledgeController(IKnowledgeBaseService knowledgeService, ApplicationDbContext context, ILogger<KnowledgeController> logger)
        {
            _knowledgeService = knowledgeService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<KnowledgePattern>>> GetPatterns()
        {
            var patterns = await _knowledgeService.GetPatterns();
            return Ok(patterns);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KnowledgePattern>> GetPattern(int id)
        {
            var pattern = await _knowledgeService.GetPattern(id);
            if (pattern == null)
                return NotFound();
            return Ok(pattern);
        }

        [HttpPost]
        public async Task<ActionResult<KnowledgePattern>> CreatePattern(KnowledgePattern pattern)
        {
            pattern.CreatedAt = DateTime.UtcNow;
            pattern.IsActive = true;
            var result = await _knowledgeService.AddPattern(pattern);
            return CreatedAtAction(nameof(GetPattern), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePattern(int id, KnowledgePattern pattern)
        {
            if (id != pattern.Id)
                return BadRequest();

            pattern.LastModified = DateTime.UtcNow;
            var success = await _knowledgeService.UpdatePattern(pattern);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePattern(int id)
        {
            var success = await _knowledgeService.DeletePattern(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> EvaluateData([FromBody] string data)
        {
            var result = await _knowledgeService.ProcessDataAsync(data);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<KnowledgeStats>> GetStats()
        {
            var patterns = await _context.KnowledgePatterns.ToListAsync();
            var totalDetections = await _context.PatternDetections.CountAsync();
            var averageConfidence = await _context.KnowledgePatterns.Select(p => p.Confidence).DefaultIfEmpty(0M).AverageAsync();

            var stats = new KnowledgeStats
            {
                TotalPatterns = patterns.Count,
                TotalDetections = totalDetections,
                AverageConfidence = averageConfidence,
                LastUpdate = DateTime.UtcNow
            };

            return stats;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
} 