window.exportToExcel = function(fileName, data) {
    const workbook = XLSX.utils.book_new();
    
    // Convertir datos a formato de hoja de cálculo
    const worksheet = XLSX.utils.json_to_sheet(flattenData(data));
    
    // Añadir la hoja al libro
    XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
    
    // Guardar archivo
    XLSX.writeFile(workbook, fileName + '.xlsx');
};

window.exportToPdf = async function(fileName, data) {
    const doc = new jsPDF();
    
    // Configurar documento
    doc.setFontSize(12);
    doc.setFont('helvetica');
    
    // Añadir título
    doc.setFontSize(16);
    doc.text(fileName, 20, 20);
    doc.setFontSize(12);
    
    // Añadir datos
    const flatData = flattenData(data);
    let y = 40;
    
    flatData.forEach(item => {
        Object.entries(item).forEach(([key, value]) => {
            if (y > 280) {
                doc.addPage();
                y = 20;
            }
            doc.text(`${key}: ${value}`, 20, y);
            y += 10;
        });
        y += 5;
    });
    
    // Guardar archivo
    doc.save(fileName + '.pdf');
};

window.exportToCsv = function(fileName, data) {
    const flatData = flattenData(data);
    const headers = Object.keys(flatData[0]);
    
    let csvContent = headers.join(',') + '\n';
    
    flatData.forEach(item => {
        const row = headers.map(header => {
            const value = item[header];
            return `"${value}"`;
        });
        csvContent += row.join(',') + '\n';
    });
    
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    saveAs(blob, fileName + '.csv');
};

window.saveAsFile = function(fileName, content, contentType) {
    const blob = new Blob([content], { type: contentType });
    saveAs(blob, fileName);
};

// Funciones auxiliares
function flattenData(data) {
    if (Array.isArray(data)) {
        return data.map(item => flattenObject(item));
    }
    return [flattenObject(data)];
}

function flattenObject(obj, prefix = '') {
    return Object.keys(obj).reduce((acc, k) => {
        const pre = prefix.length ? prefix + '.' : '';
        if (typeof obj[k] === 'object' && obj[k] !== null && !Array.isArray(obj[k])) {
            Object.assign(acc, flattenObject(obj[k], pre + k));
        } else if (Array.isArray(obj[k])) {
            acc[pre + k] = obj[k].join('; ');
        } else {
            acc[pre + k] = obj[k];
        }
        return acc;
    }, {});
} 