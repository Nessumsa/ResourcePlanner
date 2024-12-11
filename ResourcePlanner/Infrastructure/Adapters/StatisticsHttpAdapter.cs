using Microsoft.Win32;
using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ResourcePlanner.Infrastructure.Adapters
{
    class StatisticsHttpAdapter : ICreateAdapter<Statistics>
    {
        private readonly HttpClient _client;

        public StatisticsHttpAdapter(HttpClient client)
        {
            this._client = client;
        }
        public async Task<bool> CreateAsync(Statistics entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/bookings/statistic", jsonContent);

            if (!response.IsSuccessStatusCode)
                return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var bookings = JsonConvert.DeserializeObject<List<Booking>>(jsonResponse);

            // Save JSON to a file
            if (bookings != null)
            {
                SaveBookingsToFile(bookings);
            }

            return true;
        }
        private void SaveBookingsToFile(List<Booking> bookings)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save Bookings"
            };

            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;

                using (var writer = new StreamWriter(filePath))
                {
                    // Write CSV headers
                    writer.WriteLine("Id,InstitutionId,UserId,ResourceId,Date,StartTime,EndTime");

                    // Write data for each booking
                    foreach (var booking in bookings)
                    {
                        writer.WriteLine($"{booking.Id},{booking.InstitutionId},{booking.UserId},{booking.ResourceId}," +
                                         $"{booking.Date:yyyy-MM-dd},{booking.StartTime},{booking.EndTime}");
                    }
                }

                MessageBox.Show("Statstics rapport saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
