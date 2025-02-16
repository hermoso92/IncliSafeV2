let accelerationChart, orientationChart, stabilityChart;

window.initDobackCharts = function() {
    // Gráfica de Aceleraciones
    accelerationChart = echarts.init(document.getElementById('accelerationChart'));
    accelerationChart.setOption({
        tooltip: { trigger: 'axis' },
        legend: { data: ['X', 'Y', 'Z'] },
        xAxis: { type: 'category', data: [] },
        yAxis: { type: 'value' },
        series: [
            { name: 'X', type: 'line', smooth: true, data: [] },
            { name: 'Y', type: 'line', smooth: true, data: [] },
            { name: 'Z', type: 'line', smooth: true, data: [] }
        ]
    });

    // Gráfica de Orientación
    orientationChart = echarts.init(document.getElementById('orientationChart'));
    orientationChart.setOption({
        tooltip: { trigger: 'axis' },
        legend: { data: ['Roll', 'Pitch', 'Yaw'] },
        xAxis: { type: 'category', data: [] },
        yAxis: { type: 'value' },
        series: [
            { name: 'Roll', type: 'line', smooth: true, data: [] },
            { name: 'Pitch', type: 'line', smooth: true, data: [] },
            { name: 'Yaw', type: 'line', smooth: true, data: [] }
        ]
    });

    // Gráfica de Estabilidad
    stabilityChart = echarts.init(document.getElementById('stabilityChart'));
    stabilityChart.setOption({
        tooltip: {
            trigger: 'axis',
            axisPointer: { type: 'cross' }
        },
        legend: { data: ['Estabilidad', 'Velocidad'] },
        xAxis: { type: 'category', data: [] },
        yAxis: [
            { type: 'value', name: 'Índice de Estabilidad', min: 0, max: 1 },
            { type: 'value', name: 'Velocidad (km/h)' }
        ],
        series: [
            {
                name: 'Estabilidad',
                type: 'line',
                smooth: true,
                data: [],
                itemStyle: {
                    color: '#91cc75'
                },
                markLine: {
                    data: [
                        { yAxis: 0.6, lineStyle: { color: '#fac858' } },
                        { yAxis: 0.8, lineStyle: { color: '#91cc75' } }
                    ]
                }
            },
            {
                name: 'Velocidad',
                type: 'line',
                smooth: true,
                yAxisIndex: 1,
                data: [],
                itemStyle: {
                    color: '#5470c6'
                }
            }
        ]
    });

    window.addEventListener('resize', () => {
        accelerationChart.resize();
        orientationChart.resize();
        stabilityChart.resize();
    });
};

window.updateDobackCharts = function(accelerationData, orientationData, stabilityData) {
    // Actualizar gráfica de aceleraciones
    accelerationChart.setOption({
        xAxis: { data: [accelerationData.time] },
        series: [
            { data: [accelerationData.x] },
            { data: [accelerationData.y] },
            { data: [accelerationData.z] }
        ]
    });

    // Actualizar gráfica de orientación
    orientationChart.setOption({
        xAxis: { data: [orientationData.time] },
        series: [
            { data: [orientationData.roll] },
            { data: [orientationData.pitch] },
            { data: [orientationData.yaw] }
        ]
    });

    // Actualizar gráfica de estabilidad
    stabilityChart.setOption({
        xAxis: { data: [stabilityData.time] },
        series: [
            { data: [stabilityData.index] },
            { data: [stabilityData.speed] }
        ]
    });
}; 