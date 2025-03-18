let dashboardChart;

window.initDashboardCharts = function(elementId) {
    dashboardChart = echarts.init(document.getElementById(elementId));
    
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
            data: ['Estabilidad', 'Predicción', 'Tendencia']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'time',
            boundaryGap: false
        },
        yAxis: [
            {
                type: 'value',
                name: 'Índice',
                min: 0,
                max: 1,
                position: 'left',
                axisLine: {
                    show: true,
                    lineStyle: { color: '#1890ff' }
                },
                axisLabel: {
                    formatter: '{value * 100}%'
                }
            },
            {
                type: 'value',
                name: 'Tendencia',
                min: -1,
                max: 1,
                position: 'right',
                axisLine: {
                    show: true,
                    lineStyle: { color: '#52c41a' }
                }
            }
        ],
        series: [
            {
                name: 'Estabilidad',
                type: 'line',
                data: [],
                smooth: true,
                symbol: 'circle',
                symbolSize: 8,
                lineStyle: {
                    width: 3,
                    shadowColor: 'rgba(0,0,0,0.3)',
                    shadowBlur: 10
                },
                itemStyle: { color: '#1890ff' },
                markLine: {
                    silent: true,
                    data: [
                        { yAxis: 0.8, lineStyle: { color: '#52c41a' } },
                        { yAxis: 0.6, lineStyle: { color: '#faad14' } }
                    ]
                }
            },
            {
                name: 'Predicción',
                type: 'line',
                data: [],
                smooth: true,
                symbol: 'circle',
                symbolSize: 6,
                lineStyle: {
                    width: 2,
                    type: 'dashed'
                },
                itemStyle: { color: '#52c41a' }
            },
            {
                name: 'Tendencia',
                type: 'line',
                yAxisIndex: 1,
                data: [],
                smooth: true,
                symbol: 'none',
                lineStyle: {
                    width: 2
                },
                itemStyle: { color: '#faad14' }
            }
        ],
        dataZoom: [
            {
                type: 'inside',
                start: 0,
                end: 100
            },
            {
                start: 0,
                end: 100
            }
        ],
        toolbox: {
            feature: {
                dataZoom: {
                    yAxisIndex: 'none'
                },
                restore: {},
                saveAsImage: {}
            }
        }
    };

    dashboardChart.setOption(option);
    
    window.addEventListener('resize', () => {
        dashboardChart.resize();
    });
};

window.updateDashboardCharts = function(data) {
    if (!dashboardChart) {
        initDashboardCharts('dashboardPredictionChart');
    }

    const timePoints = generateTimePoints(30);
    const stabilityData = generateStabilityData(data.Metrics.StabilityIndex, timePoints);
    const predictionData = generatePredictionData(data.Metrics, timePoints);
    const trendData = generateTrendData(data.Trends, timePoints);

    dashboardChart.setOption({
        series: [
            {
                data: stabilityData
            },
            {
                data: predictionData
            },
            {
                data: trendData
            }
        ]
    });
};

function generateTimePoints(count) {
    const points = [];
    const now = new Date();
    for (let i = 0; i < count; i++) {
        points.push(new Date(now - (count - i) * 3600000));
    }
    return points;
}

function generateStabilityData(baseValue, timePoints) {
    return timePoints.map((time, index) => {
        const noise = (Math.random() - 0.5) * 0.1;
        return [time.getTime(), Math.max(0, Math.min(1, baseValue + noise))];
    });
}

function generatePredictionData(metrics, timePoints) {
    const lastTime = timePoints[timePoints.length - 1].getTime();
    const futurePoints = [];
    for (let i = 1; i <= 10; i++) {
        futurePoints.push(new Date(lastTime + i * 3600000));
    }

    return futurePoints.map((time, index) => {
        const trend = 0.01 * index;
        return [time.getTime(), Math.max(0, Math.min(1, metrics.StabilityIndex + trend))];
    });
}

function generateTrendData(trends, timePoints) {
    return timePoints.map((time, index) => {
        let trend;
        if (index < timePoints.length / 3) {
            trend = trends.ShortTerm.Slope;
        } else if (index < timePoints.length * 2/3) {
            trend = trends.MediumTerm.Slope;
        } else {
            trend = trends.LongTerm.Slope;
        }
        return [time.getTime(), trend];
    });
}

// Funciones auxiliares para análisis
window.analyzeDashboardData = function(data) {
    return {
        stability: analyzeStability(data.Metrics),
        trends: analyzeTrends(data.Trends),
        predictions: generatePredictions(data)
    };
};

function analyzeStability(metrics) {
    const score = metrics.StabilityIndex;
    return {
        score,
        status: score > 0.8 ? 'Excelente' :
                score > 0.6 ? 'Bueno' :
                score > 0.4 ? 'Regular' : 'Crítico',
        recommendations: generateStabilityRecommendations(score)
    };
}

function analyzeTrends(trends) {
    return {
        shortTerm: interpretTrend(trends.ShortTerm),
        mediumTerm: interpretTrend(trends.MediumTerm),
        longTerm: interpretTrend(trends.LongTerm)
    };
}

function interpretTrend(trend) {
    const strength = Math.abs(trend.Slope);
    return {
        direction: trend.Slope > 0 ? 'Mejorando' : 'Deteriorando',
        strength: strength > 0.1 ? 'Fuerte' :
                 strength > 0.05 ? 'Moderado' : 'Débil',
        confidence: trend.R2
    };
}

function generateStabilityRecommendations(score) {
    const recommendations = [];
    if (score < 0.4) {
        recommendations.push('Revisión inmediata requerida');
        recommendations.push('Programar mantenimiento preventivo');
    } else if (score < 0.6) {
        recommendations.push('Monitorear de cerca');
        recommendations.push('Revisar parámetros de operación');
    }
    return recommendations;
}

function generatePredictions(data) {
    // Implementar lógica de predicción
    return [];
} 