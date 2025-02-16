let trendChart;

window.initTrendChart = function(elementId) {
    trendChart = echarts.init(document.getElementById(elementId));
    
    const option = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross',
                label: {
                    backgroundColor: '#6a7985'
                }
            }
        },
        legend: {
            data: ['Estabilidad', 'Predicción', 'Límite Superior', 'Límite Inferior']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            boundaryGap: false,
            data: []
        },
        yAxis: {
            type: 'value'
        },
        series: [
            {
                name: 'Estabilidad',
                type: 'line',
                smooth: true,
                lineStyle: {
                    width: 3,
                    shadowColor: 'rgba(0,0,0,0.3)',
                    shadowBlur: 10
                },
                itemStyle: { color: '#1890ff' },
                areaStyle: {
                    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                        { offset: 0, color: 'rgba(24,144,255,0.3)' },
                        { offset: 1, color: 'rgba(24,144,255,0.1)' }
                    ])
                },
                data: []
            },
            {
                name: 'Predicción',
                type: 'line',
                smooth: true,
                lineStyle: {
                    width: 2,
                    type: 'dashed'
                },
                itemStyle: { color: '#52c41a' },
                data: []
            },
            {
                name: 'Límite Superior',
                type: 'line',
                smooth: true,
                lineStyle: {
                    width: 1,
                    type: 'dotted'
                },
                itemStyle: { color: '#faad14' },
                data: []
            },
            {
                name: 'Límite Inferior',
                type: 'line',
                smooth: true,
                lineStyle: {
                    width: 1,
                    type: 'dotted'
                },
                itemStyle: { color: '#faad14' },
                data: []
            }
        ]
    };

    trendChart.setOption(option);
    
    window.addEventListener('resize', () => {
        trendChart.resize();
    });
};

window.updateTrendChart = function(elementId, data) {
    if (!trendChart) {
        initTrendChart(elementId);
    }

    const option = {
        xAxis: {
            data: data.TimePoints
        }
    };

    switch (data.Type) {
        case 'stability':
            option.yAxis = {
                name: 'Índice de Estabilidad',
                min: 0,
                max: 1
            };
            break;
        case 'performance':
            option.yAxis = {
                name: 'Rendimiento',
                min: 0,
                max: 100
            };
            break;
        case 'patterns':
            option.yAxis = {
                name: 'Patrones Detectados',
                minInterval: 1
            };
            break;
    }

    // Calcular predicciones y límites
    const predictions = calculatePredictions(data.Values);
    const bounds = calculateConfidenceBounds(data.Values);

    option.series = [
        {
            name: 'Estabilidad',
            data: data.Values
        },
        {
            name: 'Predicción',
            data: predictions
        },
        {
            name: 'Límite Superior',
            data: bounds.upper
        },
        {
            name: 'Límite Inferior',
            data: bounds.lower
        }
    ];

    trendChart.setOption(option);
};

function calculatePredictions(values) {
    // Implementar modelo de predicción simple
    const predictions = [...values];
    const lastValue = values[values.length - 1];
    const trend = (lastValue - values[values.length - 2]) / lastValue;
    
    // Añadir 5 predicciones
    for (let i = 0; i < 5; i++) {
        predictions.push(lastValue * (1 + trend * (i + 1)));
    }
    
    return predictions;
}

function calculateConfidenceBounds(values) {
    const upper = [];
    const lower = [];
    const stdDev = calculateStdDev(values);
    
    values.forEach(value => {
        upper.push(value + stdDev * 2);
        lower.push(value - stdDev * 2);
    });
    
    return { upper, lower };
}

function calculateStdDev(values) {
    const mean = values.reduce((a, b) => a + b) / values.length;
    const squareDiffs = values.map(value => Math.pow(value - mean, 2));
    const avgSquareDiff = squareDiffs.reduce((a, b) => a + b) / squareDiffs.length;
    return Math.sqrt(avgSquareDiff);
}

// Funciones de análisis avanzado
window.analyzeTrends = function(data) {
    const analysis = {
        trend: calculateTrend(data.Values),
        seasonality: detectSeasonality(data.Values),
        anomalies: detectAnomalies(data.Values),
        forecast: generateForecast(data.Values)
    };

    return analysis;
};

function calculateTrend(values) {
    // Calcular tendencia usando regresión lineal simple
    const n = values.length;
    let sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
    
    values.forEach((y, x) => {
        sumX += x;
        sumY += y;
        sumXY += x * y;
        sumX2 += x * x;
    });
    
    const slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
    return slope;
}

function detectSeasonality(values) {
    // Implementar detección de patrones estacionales
    // usando autocorrelación
    return {
        detected: false,
        period: 0,
        strength: 0
    };
}

function detectAnomalies(values) {
    const mean = values.reduce((a, b) => a + b) / values.length;
    const stdDev = calculateStdDev(values);
    const threshold = stdDev * 2;
    
    return values.map((value, index) => ({
        index,
        value,
        isAnomaly: Math.abs(value - mean) > threshold
    })).filter(point => point.isAnomaly);
}

function generateForecast(values) {
    const trend = calculateTrend(values);
    const lastValue = values[values.length - 1];
    
    return {
        nextValue: lastValue + trend,
        confidence: 0.85,
        range: {
            min: lastValue + trend * 0.9,
            max: lastValue + trend * 1.1
        }
    };
} 