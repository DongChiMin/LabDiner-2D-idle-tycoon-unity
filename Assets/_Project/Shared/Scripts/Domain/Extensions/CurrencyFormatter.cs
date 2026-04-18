using System;
using System.Globalization;
using UnityEngine;

namespace LabDiner.Shared
{
    public static class CurrencyFormatter
    {
        private static readonly string[] Units = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        public static string Format(this double value)
        {
            if (value < 1000) return value.ToString("0");

            int unitIndex = -1;
            double tempValue = value;

            while (tempValue >= 1000 && unitIndex < Units.Length - 1)
            {
                tempValue /= 1000;
                unitIndex++;
            }

            // Dùng "0.##" để: 1000 -> 1a, 1230 -> 1.23a, 1200 -> 1.2a
            return $"{tempValue:0.##}{Units[unitIndex]}";
        }

        /// <summary>
        /// Hàm này cho phép người dùng nhập các chuỗi như "1.5a", "2b", "500", "3.2c" và sẽ trả về giá trị double tương ứng (1500, 2000, 500, 3200000).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Format(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;

            // 1. Dọn dẹp chuỗi: Xóa khoảng trắng, chuyển về chữ thường
            input = input.Trim().ToLower();

            // 2. Kiểm tra ký tự cuối cùng có phải là chữ cái đơn vị không
            char lastChar = input[input.Length - 1];
            double multiplier = 1;

            // Kiểm tra xem lastChar có phải là chữ cái (a-z) không
            if (char.IsLetter(lastChar))
            {
                int unitIndex = Array.IndexOf(Units, lastChar.ToString());
                if (unitIndex >= 0)
                {
                    // Dùng Math.Pow (double) thay vì Mathf.Pow (float) để tránh nổ số
                    multiplier = Math.Pow(1000, unitIndex + 1);
                    input = input.Substring(0, input.Length - 1);
                }
            }

            // 3. Parse số với InvariantCulture để chấp nhận dấu chấm (.) bất kể máy ở vùng nào
            // Thêm NumberStyles.Any để hỗ trợ cả số âm (dấu -)
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                return number * multiplier;
            }

            return 0;
        }
    }
}
