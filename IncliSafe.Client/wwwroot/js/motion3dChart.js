let motion3dChart;

window.initMotion3DChart = function(elementId, data) {
    motion3dChart = echarts.init(document.getElementById(elementId));
    
    const option = {
        tooltip: {},
        backgroundColor: '#fff',
        visualMap: {
            show: true,
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
            name: 'Roll (°)',
            axisLine: { lineStyle: { color: '#999' } },
            axisLabel: { formatter: '{value}°' }
        },
        yAxis3D: {
            type: 'value',
            name: 'Pitch (°)',
            axisLine: { lineStyle: { color: '#999' } },
            axisLabel: { formatter: '{value}°' }
        },
        zAxis3D: {
            type: 'value',
            name: 'Yaw (°)',
            axisLine: { lineStyle: { color: '#999' } },
            axisLabel: { formatter: '{value}°' }
        },
        grid3D: {
            viewControl: {
                projection: 'orthographic',
                autoRotate: true,
                autoRotateSpeed: 10,
                distance: 200
            },
            light: {
                main: {
                    intensity: 1.2,
                    shadow: true
                },
                ambient: {
                    intensity: 0.3
                }
            }
        },
        series: [
            {
                type: 'scatter3D',
                name: 'Orientación',
                data: [[data.roll, data.pitch, data.yaw]],
                symbolSize: 10,
                itemStyle: {
                    color: '#1890ff',
                    opacity: 0.8,
                    borderWidth: 1,
                    borderColor: '#fff'
                }
            },
            {
                type: 'line3D',
                name: 'Trayectoria',
                data: generateTrajectory(data),
                lineStyle: {
                    width: 4,
                    color: '#91cc75',
                    opacity: 0.5
                }
            }
        ]
    };

    motion3dChart.setOption(option);
    
    window.addEventListener('resize', () => {
        motion3dChart.resize();
    });
};

function generateTrajectory(data) {
    const points = [];
    const steps = 50;
    
    for (let i = 0; i < steps; i++) {
        const t = i / steps;
        points.push([
            data.roll * Math.cos(t * Math.PI * 2),
            data.pitch * Math.sin(t * Math.PI * 2),
            data.yaw * Math.cos(t * Math.PI)
        ]);
    }
    
    return points;
}

window.updateMotion3DChart = function(data) {
    if (!motion3dChart) return;

    const newData = {
        series: [
            {
                data: [[data.roll, data.pitch, data.yaw]]
            },
            {
                data: generateTrajectory(data)
            }
        ]
    };

    motion3dChart.setOption(newData);
};

// Funciones auxiliares para análisis avanzado
window.analyzeMotion = function(data) {
    const stabilityScore = calculateStabilityScore(data);
    const riskLevel = assessRiskLevel(data);
    const recommendations = generateRecommendations(data);

    return {
        stabilityScore,
        riskLevel,
        recommendations
    };
};

function calculateStabilityScore(data) {
    const rollFactor = Math.abs(data.roll) / 90;
    const pitchFactor = Math.abs(data.pitch) / 90;
    const yawFactor = Math.abs(data.yaw) / 180;
    
    return 1 - (rollFactor + pitchFactor + yawFactor) / 3;
}

function assessRiskLevel(data) {
    const maxAngle = Math.max(
        Math.abs(data.roll),
        Math.abs(data.pitch)
    );

    if (maxAngle > 45) return 'high';
    if (maxAngle > 30) return 'medium';
    return 'low';
}

function generateRecommendations(data) {
    const recommendations = [];
    
    if (Math.abs(data.roll) > 30) {
        recommendations.push('Reducir velocidad en curvas');
    }
    if (Math.abs(data.pitch) > 30) {
        recommendations.push('Precaución en pendientes');
    }
    if (Math.abs(data.yaw) > 90) {
        recommendations.push('Evitar giros bruscos');
    }

    return recommendations;
} 