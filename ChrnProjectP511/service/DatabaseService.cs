using ChrnProjectP511.Data;
using ChrnProjectP511.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.service
{
    public class DatabaseService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task SaveToHistoryAsync(WeatherData weather)
        {
            var record = new WeatherHistory
            {
                CityName = weather.CityName,
                Temperature = weather.Temperature,
                WindSpeed = weather.WindSpeed,
                Humidity = weather.Humidity,
                WeatherDescription = weather.WeatherDiscription,
                SearchedAt = DateTime.Now
            };

            _context.History.Add(record);
            await _context.SaveChangesAsync();
        }


        public async Task<List<WeatherHistory>> GetHistoryAsync()
        {
            return await _context.History
                .OrderByDescending(x => x.SearchedAt)
                .Take(10)
                .ToListAsync();
        }


        public async Task ClearHistoryAsync()
        {
            _context.History.RemoveRange(_context.History);
            await _context.SaveChangesAsync();
        }

        public async Task<WeatherIcon?> GetIconByCodeAsync(int weatherCode)
        {
            return await _context.Icons
                .FirstOrDefaultAsync(x => x.WeatherCode == weatherCode);
        }
    }
}
