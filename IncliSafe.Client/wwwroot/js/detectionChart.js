let detectionChart;

window.initDetectionHistoryChart = function(elementId, data) {
    detectionChart = echarts.init(document.getElementById(elementId));
    
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
            data: ['Confianza', 'Umbral', 'Valores Detectados']
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
                name: 'Confianza',
                min: 0,
                max: 1,
                position: 'left',
                axisLine: {
                    show: true,
                    lineStyle: {
                        color: '#1890ff'
                    }
                },
                axisLabel: {
                    formatter: '{value * 100}%'
                }
            },
            {
                type: 'value',
                name: 'Valores',
                position: 'right',
                axisLine: {
                    show: true,
                    lineStyle: {
                        color: '#52c41a'
                    }
                }
            }
        ],
        series: [
            {
                name: 'Confianza',
                type: 'line',
                smooth: true,
                yAxisIndex: 0,
                data: data.confidenceHistory,
                lineStyle: {
                    width: 3,
                    shadowColor: 'rgba(0,0,0,0.3)',
                    shadowBlur: 10
                },
                itemStyle: { color: '#1890ff' },
                markLine: {
                    silent: true,
                    data: [
                        {
                            yAxis: 0.8,
                            lineStyle: { color: '#52c41a' }
                        },
                        {
                            yAxis: 0.6,
                            lineStyle: { color: '#faad14' }
                        }
                    ]
                }
            },
            {
                name: 'Valores Detectados',
                type: 'line',
                smooth: true,
                yAxisIndex: 1,
                data: data.valueHistory,
                lineStyle: {
                    width: 2
                },
                itemStyle: { color: '#52c41a' }
            },
            {
                name: 'Umbral',
                type: 'line',
                smooth: true,
                yAxisIndex: 1,
                data: data.thresholdHistory,
                lineStyle: {
                    width: 2,
                    type: 'dashed'
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

    detectionChart.setOption(option);
    
    window.addEventListener('resize', () => {
        detectionChart.resize();
    });
};

window.updateDetectionChart = function(data) {
    if (!detectionChart) return;

    detectionChart.setOption({
        series: [
            {
                data: data.confidenceHistory
            },
            {
                data: data.valueHistory
            },
            {
                data: data.thresholdHistory
            }
        ]
    });
}; 