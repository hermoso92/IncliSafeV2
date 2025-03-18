window.initStability3DChart = function (elementId, data) {
    const chart = echarts.init(document.getElementById(elementId));
    
    const option = {
        tooltip: {},
        backgroundColor: '#fff',
        visualMap: {
            show: false,
            dimension: 2,
            min: -180,
            max: 180,
            inRange: {
                color: ['#313695', '#4575b4', '#74add1', '#abd9e9', '#e0f3f8', '#ffffbf',
                       '#fee090', '#fdae61', '#f46d43', '#d73027', '#a50026']
            }
        },
        xAxis3D: {
            type: 'value',
            name: 'Roll'
        },
        yAxis3D: {
            type: 'value',
            name: 'Pitch'
        },
        zAxis3D: {
            type: 'value',
            name: 'Yaw'
        },
        grid3D: {
            viewControl: {
                projection: 'orthographic'
            }
        },
        series: [{
            type: 'scatter3D',
            data: [[data.roll, data.pitch, data.yaw]],
            symbolSize: 10,
            itemStyle: {
                opacity: 0.8
            }
        }]
    };

    chart.setOption(option);
    
    window.addEventListener('resize', () => {
        chart.resize();
    });
}; 