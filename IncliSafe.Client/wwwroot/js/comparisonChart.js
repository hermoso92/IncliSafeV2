let comparisonChart;

window.initComparisonChart = function(elementId) {
    comparisonChart = echarts.init(document.getElementById(elementId));
    
    const option = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross'
            }
        },
        legend: {
            data: ['Base', 'Comparación']
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
                name: 'Base',
                type: 'line',
                data: [],
                smooth: true,
                lineStyle: {
                    width: 3,
                    shadowColor: 'rgba(0,0,0,0.3)',
                    shadowBlur: 10,
                    shadowOffsetY: 8
                },
                itemStyle: {
                    color: '#1890ff'
                },
                areaStyle: {
                    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                        { offset: 0, color: 'rgba(24,144,255,0.3)' },
                        { offset: 1, color: 'rgba(24,144,255,0.1)' }
                    ])
                }
            },
            {
                name: 'Comparación',
                type: 'line',
                data: [],
                smooth: true,
                lineStyle: {
                    width: 3,
                    shadowColor: 'rgba(0,0,0,0.3)',
                    shadowBlur: 10,
                    shadowOffsetY: 8
                },
                itemStyle: {
                    color: '#52c41a'
                },
                areaStyle: {
                    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                        { offset: 0, color: 'rgba(82,196,26,0.3)' },
                        { offset: 1, color: 'rgba(82,196,26,0.1)' }
                    ])
                }
            }
        ]
    };

    comparisonChart.setOption(option);
    
    window.addEventListener('resize', () => {
        comparisonChart.resize();
    });
};

window.updateComparisonChart = function(elementId, data) {
    if (!comparisonChart) {
        initComparisonChart(elementId);
    }

    const timePoints = generateTimePoints(20);
    const baseData = getMetricData(data.BaseAnalysis.Data, data.Metric);
    const comparisonData = getMetricData(data.ComparisonAnalysis.Data, data.Metric);

    const option = {
        xAxis: {
            data: timePoints
        },
        series: [
            {
                name: 'Base',
                data: baseData
            },
            {
                name: 'Comparación',
                data: comparisonData
            }
        ]
    };

    if (data.Metric === 'stability') {
        option.yAxis = {
            name: 'Índice de Estabilidad',
            min: 0,
            max: 1
        };
    } else if (data.Metric === 'acceleration') {
        option.yAxis = {
            name: 'Aceleración (m/s²)'
        };
    } else if (data.Metric === 'orientation') {
        option.yAxis = {
            name: 'Ángulo (°)'
        };
    }

    comparisonChart.setOption(option);
};

function generateTimePoints(count) {
    const points = [];
    const now = new Date();
    for (let i = 0; i < count; i++) {
        points.push(new Date(now - (count - i) * 1000).toLocaleTimeString());
    }
    return points;
}

function getMetricData(data, metric) {
    switch (metric) {
        case 'stability':
            return generateDataPoints(data.StabilityIndex, 20);
        case 'acceleration':
            return generateDataPoints(Math.max(
                Math.abs(data.AccelerationX),
                Math.abs(data.AccelerationY),
                Math.abs(data.AccelerationZ)
            ), 20);
        case 'orientation':
            return generateDataPoints(Math.max(
                Math.abs(data.Roll),
                Math.abs(data.Pitch),
                Math.abs(data.Yaw)
            ), 20);
        default:
            return [];
    }
}

function generateDataPoints(baseValue, count) {
    const points = [];
    for (let i = 0; i < count; i++) {
        points.push(baseValue + (Math.random() - 0.5) * baseValue * 0.1);
    }
    return points;
}

// Funciones de análisis
window.analyzeComparison = function(baseData, comparisonData) {
    const analysis = {
        stabilityDiff: calculateStabilityDifference(baseData, comparisonData),
        accelerationDiff: calculateAccelerationDifference(baseData, comparisonData),
        orientationDiff: calculateOrientationDifference(baseData, comparisonData),
        recommendations: []
    };

    // Generar recomendaciones basadas en diferencias
    if (Math.abs(analysis.stabilityDiff) > 0.2) {
        analysis.recommendations.push({
            type: 'stability',
            message: analysis.stabilityDiff > 0 
                ? 'Mejora significativa en estabilidad'
                : 'Deterioro en estabilidad - Revisar técnica de conducción'
        });
    }

    if (Math.abs(analysis.accelerationDiff) > 0.3) {
        analysis.recommendations.push({
            type: 'acceleration',
            message: 'Cambio significativo en patrones de aceleración'
        });
    }

    if (Math.abs(analysis.orientationDiff) > 0.25) {
        analysis.recommendations.push({
            type: 'orientation',
            message: 'Variación importante en ángulos de operación'
        });
    }

    return analysis;
};

function calculateStabilityDifference(baseData, comparisonData) {
    return (comparisonData.StabilityIndex - baseData.StabilityIndex) / baseData.StabilityIndex;
}

function calculateAccelerationDifference(baseData, comparisonData) {
    const baseAcc = Math.max(
        Math.abs(baseData.AccelerationX),
        Math.abs(baseData.AccelerationY),
        Math.abs(baseData.AccelerationZ)
    );
    const compAcc = Math.max(
        Math.abs(comparisonData.AccelerationX),
        Math.abs(comparisonData.AccelerationY),
        Math.abs(comparisonData.AccelerationZ)
    );
    return (compAcc - baseAcc) / baseAcc;
}

function calculateOrientationDifference(baseData, comparisonData) {
    const baseAngle = Math.max(
        Math.abs(baseData.Roll),
        Math.abs(baseData.Pitch),
        Math.abs(baseData.Yaw)
    );
    const compAngle = Math.max(
        Math.abs(comparisonData.Roll),
        Math.abs(comparisonData.Pitch),
        Math.abs(comparisonData.Yaw)
    );
    return (compAngle - baseAngle) / baseAngle;
} 