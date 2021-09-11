using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Lab2_Sweep {
    public static class Utils {
        public static T Convert<T>(this string input) {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter == null) {
                throw new Exception();
            }

            return (T)converter.ConvertFromString(input);
        }

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

        public static void ShowArr(IList<double> list) {
            foreach (var value in list) {
                Console.Write(value + "\t");
            }
        }
    }
}