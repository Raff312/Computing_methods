using System;
using System.ComponentModel;

namespace Lab5_Division {
    public static class Utils {
        public static T GetValueFromUser<T>(string msg) {
            while (true) {
                Console.Write(msg);
                var userAnswer = Console.ReadLine();
                try {
                    return Utils.Convert<T>(userAnswer);
                } catch (Exception) {
                    ConsoleTools.WriteLine(ConsoleColor.Red, "Invalid value type. Try again...");
                }
            }
        }

        public static T Convert<T>(this string input) {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter == null) {
                throw new Exception();
            }

            return (T)converter.ConvertFromString(input);
        }
    }
}