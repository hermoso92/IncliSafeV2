using IncliSafe.Shared.Models.Patterns;
using IncliSafeApi.Services.Interfaces;
using IncliSafeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IncliSafeApi.Services
{
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KnowledgeBaseService> _logger;

        public KnowledgeBaseService(ApplicationDbContext context, ILogger<KnowledgeBaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ProcessDataAsync(string data)
        {
            try
            {
                var patterns = await _context.KnowledgePatterns
                    .Where(p => p.IsActive)
                    .ToListAsync();

                foreach (var pattern in patterns)
                {
                    if (await EvaluatePatternAsync(data, pattern))
                    {
                        await CreateDetectionAsync(pattern);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing data");
                return false;
            }
        }

        private async Task<bool> EvaluatePatternAsync(string data, KnowledgePattern pattern)
        {
            if (string.IsNullOrEmpty(pattern.Pattern))
            {
                _logger.LogWarning("Pattern {PatternId} has no pattern defined", pattern.Id);
                return false;
            }

            await Task.Yield();
            try
            {
                return Regex.IsMatch(data, pattern.Pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating pattern {PatternId}", pattern.Id);
                return false;
            }
        }

        private async Task CreateDetectionAsync(KnowledgePattern pattern)
        {
            var detection = new PatternDetection
            {
                PatternName = pattern.Name,
                KnowledgePatternId = pattern.Id,
                ConfidenceScore = Convert.ToDecimal(pattern.Confidence),
                DetectionTime = DateTime.UtcNow
            };

            _context.PatternDetections.Add(detection);
            await _context.SaveChangesAsync();
        }

        public async Task<List<KnowledgePattern>> GetPatterns()
        {
            return await _context.KnowledgePatterns
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<KnowledgePattern?> GetPattern(int id)
        {
            return await _context.KnowledgePatterns.FindAsync(id);
        }

        public async Task<KnowledgePattern> AddPattern(KnowledgePattern pattern)
        {
            _context.KnowledgePatterns.Add(pattern);
            await _context.SaveChangesAsync();
            return pattern;
        }

        public async Task<bool> UpdatePattern(KnowledgePattern pattern)
        {
            _context.Entry(pattern).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePattern(int id)
        {
            var pattern = await _context.KnowledgePatterns.FindAsync(id);
            if (pattern == null) return false;
            
            pattern.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 