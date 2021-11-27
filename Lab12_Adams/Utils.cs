namespace Lab12_Adams {
    public static class Utils {
        public static T? GetValueFromUser<T>(string msg) {
            while (true) {
                Console.Write(msg);
                var userAnswer = Console.ReadLine();
                try {
                    return (T?)Convert.ChangeType(userAnswer, typeof(T));
                } catch (Exception) {
                    ConsoleTools.WriteLine(ConsoleColor.Red, "Invalid value type. Try again...");
                }
            }
        }

        public static void Swap<T>(ref T lhs, ref T rhs) {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static bool IsEven(int n) {
            return (n & 1) == 0;
        }
    }
}