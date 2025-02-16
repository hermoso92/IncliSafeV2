using System.Numerics;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.LinearRegression;

public interface IPredictiveAnalysisService
{
    Task<PredictionResult> PredictStability(int vehicleId, DateTime startDate, DateTime endDate);
    Task<List<Anomaly>> DetectAnomalies(int vehicleId, DateTime startDate, DateTime endDate);
    Task<TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate);
    Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate);
}

public class PredictiveAnalysisService : IPredictiveAnalysisService
{
    private readonly IDobackAnalysisService _dobackService;
    private readonly ILogger<PredictiveAnalysisService> _logger;

    public PredictiveAnalysisService(
        IDobackAnalysisService dobackService,
        ILogger<PredictiveAnalysisService> logger)
    {
        _dobackService = dobackService;
        _logger = logger;
    }

    public async Task<PredictionResult> PredictStability(int vehicleId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var historicalData = await _dobackService.GetHistoricalData(vehicleId, startDate, endDate);
            
            // Preparar datos para el modelo
            var timePoints = historicalData.Select((d, i) => (double)i).ToArray();
            var stabilityValues = historicalData.Select(d => (double)d.StabilityIndex).ToArray();

            // Realizar regresión lineal simple
            var regression = SimpleRegression.Fit(timePoints, stabilityValues);
            
            // Calcular predicciones
            var predictions = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                var prediction = regression.Item1 + regression.Item2 * (timePoints.Length + i);
                predictions.Add(prediction);
            }

            // Calcular intervalos de confianza
            var stdDev = stabilityValues.StandardDeviation();
            var confidenceInterval = stdDev * 1.96; // 95% intervalo de confianza

            return new PredictionResult
            {
                Predictions = predictions,
                UpperBound = predictions.Select(p => p + confidenceInterval).ToList(),
                LowerBound = predictions.Select(p => p - confidenceInterval).ToList(),
                Confidence = CalculateConfidence(regression.Item2, stdDev),
                Trend = regression.Item2
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting stability");
            throw;
        }
    }

    public async Task<List<Anomaly>> DetectAnomalies(int vehicleId, DateTime startDate, DateTime endDate)
    {
        var data = await _dobackService.GetHistoricalData(vehicleId, startDate, endDate);
        var anomalies = new List<Anomaly>();

        // Calcular estadísticas básicas
        var stabilityValues = data.Select(d => (double)d.StabilityIndex).ToArray();
        var mean = stabilityValues.Mean();
        var stdDev = stabilityValues.StandardDeviation();
        var threshold = stdDev * 2; // 2 desviaciones estándar

        // Detectar anomalías
        for (int i = 0; i < data.Count; i++)
        {
            var value = (double)data[i].StabilityIndex;
            if (Math.Abs(value - mean) > threshold)
            {
                anomalies.Add(new Anomaly
                {
                    Timestamp = data[i].Timestamp,
                    Value = value,
                    ExpectedValue = mean,
                    Deviation = Math.Abs(value - mean) / stdDev,
                    Type = value > mean ? AnomalyType.High : AnomalyType.Low
                });
            }
        }

        return anomalies;
    }

    public async Task<TrendAnalysis> AnalyzeTrends(int vehicleId, DateTime startDate, DateTime endDate)
    {
        var data = await _dobackService.GetHistoricalData(vehicleId, startDate, endDate);
        
        // Análisis de tendencias
        var trends = new TrendAnalysis
        {
            ShortTerm = CalculateTrend(data.TakeLast(10).ToList()),
            MediumTerm = CalculateTrend(data.TakeLast(30).ToList()),
            LongTerm = CalculateTrend(data),
            Seasonality = DetectSeasonality(data),
            Cycles = DetectCycles(data)
        };

        return trends;
    }

    public async Task<List<Pattern>> DetectPatterns(int vehicleId, DateTime startDate, DateTime endDate)
    {
        var data = await _dobackService.GetHistoricalData(vehicleId, startDate, endDate);
        var patterns = new List<Pattern>();

        // Detectar patrones usando ventana deslizante
        const int windowSize = 5;
        for (int i = 0; i <= data.Count - windowSize; i++)
        {
            var window = data.Skip(i).Take(windowSize).ToList();
            var pattern = IdentifyPattern(window);
            if (pattern != null)
            {
                patterns.Add(pattern);
            }
        }

        return patterns;
    }

    private decimal CalculateConfidence(double trend, double stdDev)
    {
        // Calcular confianza basada en la fuerza de la tendencia y la variabilidad
        var trendStrength = Math.Abs(trend);
        var variability = stdDev / trendStrength;
        return (decimal)(1 / (1 + Math.Exp(-trendStrength + variability)));
    }

    private Trend CalculateTrend(List<DobackAnalysis> data)
    {
        var values = data.Select(d => (double)d.StabilityIndex).ToArray();
        var timePoints = data.Select((d, i) => (double)i).ToArray();
        var regression = SimpleRegression.Fit(timePoints, values);

        return new Trend
        {
            Slope = regression.Item2,
            Intercept = regression.Item1,
            R2 = CalculateR2(timePoints, values, regression)
        };
    }

    private double CalculateR2(double[] x, double[] y, Tuple<double, double> regression)
    {
        var yMean = y.Mean();
        var totalSS = y.Sum(yi => Math.Pow(yi - yMean, 2));
        var residualSS = y.Zip(x, (yi, xi) => 
            Math.Pow(yi - (regression.Item1 + regression.Item2 * xi), 2)).Sum();
        
        return 1 - (residualSS / totalSS);
    }

    private Seasonality DetectSeasonality(List<DobackAnalysis> data)
    {
        // Implementar detección de estacionalidad usando autocorrelación
        return new Seasonality();
    }

    private List<Cycle> DetectCycles(List<DobackAnalysis> data)
    {
        // Implementar detección de ciclos usando análisis espectral
        return new List<Cycle>();
    }

    private Pattern? IdentifyPattern(List<DobackAnalysis> window)
    {
        // Implementar identificación de patrones usando reglas predefinidas
        return null;
    }
} 