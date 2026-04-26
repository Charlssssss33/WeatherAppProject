using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Models
{
    public static class WeatherCodeMapper
    {
        public static string GetDescription(int code)
        {
            return code switch
            {
                0 => "Ясно",
                1 => "В основном Ясно",
                2 => "Переменная облачность",
                3 => "Пасмурно",
                45 => "Туман",
                51 => "Морось",
                61 => "Дождь",
                71 => "Снег",
                80 => "Ливень",
                _ => "Неизвестно"
            };
        }
        

        
    }
}
