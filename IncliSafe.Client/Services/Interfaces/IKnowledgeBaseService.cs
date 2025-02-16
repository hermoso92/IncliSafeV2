using IncliSafe.Shared.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Shared.Models.Patterns;
using IncliSafe.Shared.Models.Knowledge;

namespace IncliSafe.Client.Services.Interfaces
{
    public interface IKnowledgeBaseService
    {
        Task<bool> ProcessDataAsync(string data);
        Task<List<KnowledgePattern>> GetPatterns();
        Task<KnowledgePattern> GetPattern(int id);
        Task<KnowledgePattern> CreatePattern(KnowledgePattern pattern);
        Task<KnowledgePattern> UpdatePattern(KnowledgePattern pattern);
        Task DeletePattern(int id);
        Task<KnowledgeStats> GetStats();
        Task<List<PatternDetection>> GetPatternDetections(int patternId);
        Task<PatternDetection> DetectPatternAsync(int patternId);
    }
} 