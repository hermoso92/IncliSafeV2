let predictionChart;

window.initPredictionChart = function(elementId) {
    predictionChart = echarts.init(document.getElementById(elementId));
    
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
            data: ['Valor Real', 'Predicción', 'Límite Superior', 'Límite Inferior']
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
        yAxis: {
            type: 'value',
            name: 'Valor',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: 'Valor Real',
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
                itemStyle: { color: '#1890ff' }
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
                name: 'Límite Superior',
                type: 'line',
                data: [],
                smooth: true,
                symbol: 'none',
                lineStyle: {
                    width: 1,
                    type: 'dotted'
                },
                itemStyle: { color: '#faad14' },
                areaStyle: {
                    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                        { offset: 0, color: 'rgba(250,173,20,0.3)' },
                        { offset: 1, color: 'rgba(250,173,20,0.1)' }
                    ])
                }
            },
            {
                name: 'Límite Inferior',
                type: 'line',
                data: [],
                smooth: true,
                symbol: 'none',
                lineStyle: {
                    width: 1,
                    type: 'dotted'
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

    predictionChart.setOption(option);
    
    window.addEventListener('resize', () => {
        predictionChart.resize();
    });
};

window.updatePredictionChart = function(elementId, data) {
    if (!predictionChart) {
        initPredictionChart(elementId);
    }

    const option = {
        yAxis: getYAxisConfig(data.Type),
        series: [
            {
                data: formatTimeData(data.TimePoints, data.Predictions)
            },
            {
                data: formatTimeData(data.TimePoints.slice(-10), data.Predictions.slice(-10))
            },
            {
                data: formatTimeData(data.TimePoints.slice(-10), data.UpperBound)
            },
            {
                data: formatTimeData(data.TimePoints.slice(-10), data.LowerBound)
            }
        ]
    };

    predictionChart.setOption(option);
};

function getYAxisConfig(type) {
    switch (type) {
        case 'stability':
            return {
                name: 'Índice de Estabilidad',
                min: 0,
                max: 1,
                axisLabel: {
                    formatter: '{value * 100}%'
                }
            };
        case 'performance':
            return {
                name: 'Rendimiento',
                min: 0,
                max: 100,
                axisLabel: {
                    formatter: '{value}%'
                }
            };
        case 'risk':
            return {
                name: 'Nivel de Riesgo',
                min: 0,
                max: 1,
                axisLabel: {
                    formatter: '{value * 100}%'
                }
            };
        default:
            return {
                name: 'Valor',
                axisLabel: {
                    formatter: '{value}'
                }
            };
    }
}

function formatTimeData(timePoints, values) {
    return timePoints.map((time, index) => [
        new Date(time).getTime(),
        values[index]
    ]);
} 