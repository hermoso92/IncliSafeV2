using IncliSafe.Shared.Models;

public interface IKnowledgeBaseService
{
    Task<List<KnowledgePattern>> GetPatterns();
    Task<KnowledgePattern> GetPattern(int id);
    Task<KnowledgePattern> CreatePattern(KnowledgePattern pattern);
    Task<KnowledgePattern> UpdatePattern(KnowledgePattern pattern);
    Task DeletePattern(int id);
    Task<KnowledgeStats> GetStats();
    Task<Dictionary<string, double>> GetPatternDistribution();
    Task<List<PatternDetection>> GetPatternDetections(int patternId);
} 