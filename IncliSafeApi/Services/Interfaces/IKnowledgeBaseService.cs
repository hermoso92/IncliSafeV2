using IncliSafe.Shared.Models.Patterns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IncliSafeApi.Services.Interfaces
{
    public interface IKnowledgeBaseService
    {
        Task<bool> ProcessDataAsync(string data);
        Task<List<KnowledgePattern>> GetPatterns();
        Task<KnowledgePattern?> GetPattern(int id);
        Task<KnowledgePattern> AddPattern(KnowledgePattern pattern);
        Task<bool> UpdatePattern(KnowledgePattern pattern);
        Task<bool> DeletePattern(int id);
    }
} 