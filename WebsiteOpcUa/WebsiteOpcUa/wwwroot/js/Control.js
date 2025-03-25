// Zewnętrzny plik Control.js

$(document).ready(function () {
    // Funkcja do wysyłania danych
    function sendData(name, nameIn) {
        var inputValue = $(nameIn).val();  // Pobranie wartości inputa

        // Wysłanie żądania AJAX do kontrolera
        $.ajax({
            url: '/Control/SubmitAction',
            type: 'POST',
            data: {
                name: name,
                inputValue: inputValue
            },
            complete: function () {
                console.log('Dane zostały wysłane');
            }
        });
    }

    // Obsługa kliknięcia przycisków i Enter dla każdego pola
    $('#submitButton').click(function () {
        sendData('SetPoint', '#inputValue');
    });
    $('#inputValue').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            sendData('SetPoint', '#inputValue');
        }
    });

    $('#submitButtonP').click(function () {
        sendData('Gain', '#inputValueP');
    });
    $('#inputValueP').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            sendData('Gain', '#inputValueP');
        }
    });

    $('#submitButtonI').click(function () {
        sendData('Ti', '#inputValueI');
    });
    $('#inputValueI').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            sendData('Ti', '#inputValueI');
        }
    });

    let chartData = {
        labels: [],
        datasets: [{
            label: 'Wartość y',
            data: [],
            borderColor: 'rgba(75, 192, 192, 1)',
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderWidth: 1,
            fill: false
        }]
    };

    let chartData1 = {
        labels: [],
        datasets: [{
            label: 'Wartość u',
            data: [],
            borderColor: 'rgba(255, 140, 0, 0.8)',
            backgroundColor: 'rgba(255, 140, 0, 0.8)',
            borderWidth: 1,
            fill: false
        }]
    };

    // Funkcja inicjalizująca wykres
    function createChart() {

        const ctx = document.getElementById('myChart').getContext('2d');
        const ctx1 = document.getElementById('myChart1').getContext('2d');
        chart = new Chart(ctx, {
            type: 'line',
            data: chartData,
            options: {
                scales: {
                    x: {
                        display: true,
                        title: {
                            display: true,
                            text: 'Czas / Indeks'
                        }
                    },
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Wartość'
                        }
                    }
                }
            }
        });

        chart1 = new Chart(ctx1, {
            type: 'line',
            data: chartData1,
            options: {
                scales: {
                    x: {
                        display: true,
                        title: {
                            display: true,
                            text: 'Czas / Indeks'
                        }
                    },
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Wartość'
                        }
                    }
                }
            }
        });
    }

    // Funkcja do aktualizowania wykresu
    function updateChart(newLabel, newValue) {
        if (chart.data.labels.length > 100) {
            chart.data.labels.shift();
            chart.data.datasets[0].data.shift();
        }
        chart.data.labels.push(newLabel);
        chart.data.datasets[0].data.push(newValue);
        chart.update();
    }

    function updateChart1(newLabel, newValue) {
        if (chart1.data.labels.length > 100) {
            chart1.data.labels.shift();
            chart1.data.datasets[0].data.shift();
        }
        chart1.data.labels.push(newLabel);
        chart1.data.datasets[0].data.push(newValue);
        chart1.update();
    }

    // Funkcja do aktualizowania tabeli
    function updateTable() {
        $.ajax({
            url: '/Control/GetData',
            type: 'GET',
            success: function (data) {
                data.forEach(function (item) {
                    let existingRow = $(`#data-table-body tr[data-name="${item.name}"]`);
                    if (existingRow.length) {
                        existingRow.find('.value-cell').text(item.value); 
                    } else {
                        let row = `
                           <tr data-name="${item.name}">
                           <td>${item.name}</td>
                            <td class="value-cell">${item.value}</td>
                            </tr>`;
                           $('#data-table-body').append(row);
                    }
                        if (item.name === "y") {
                            updateChart(new Date().toLocaleTimeString(), item.value);
                       }
                       if (item.name === "u") {
                           updateChart1(new Date().toLocaleTimeString(), item.value);
                           console.log("ra");
                        }
                });

            }
        });
    }

    // Pierwsze uruchomienie i inicjalizacja wykresu
    createChart();
    updateTable();
    setInterval(updateTable, 3000);
});
