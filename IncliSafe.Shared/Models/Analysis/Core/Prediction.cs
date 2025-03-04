namespace IncliSafe.Shared.Models.Analysis.Core
{
    public class Prediction
    {
        public int Id { get; set; }
        public int AnalysisId { get; set; }
        public PredictionType PredictionType { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Confidence { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 