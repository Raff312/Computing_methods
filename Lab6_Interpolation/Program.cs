using System;

namespace Lab6_Interpolation {
    public partial class Program {
        private static void Main() {
            while (ProcessCommandInput()) { }
        }

        private static bool ProcessCommandInput() {
            Console.WriteLine();
            WriteCommandDescriptions();
            string commandText = null;
            while (string.IsNullOrWhiteSpace(commandText)) {
                Console.Write("> ");
                commandText = Console.ReadLine();
            }

            var parts = commandText.Split(' ', ',');
            var command = parts[0];

            try {
                var commandDefinition = GetCommandDefinitionByCode(command);
                if (commandDefinition != null) {
                    if (commandDefinition.Action == null) {
                        Console.Write("Goodbye...");
                        return false;
                    }

                    Console.WriteLine($"Executing command '{command}'...\n");
                    commandDefinition.Action();
                    ConsoleTools.WriteLine(ConsoleColor.Green, $"\nCommand '{command}' completed.");
                } else {
                    ConsoleTools.WriteLine(ConsoleColor.Yellow, $"\nUnknown command '{command}'");
                }
            } catch (Exception e) {
                ConsoleTools.WriteLine(ConsoleColor.Red, $"\nException during command '{command}': {e.Message}");
            }

            return true;
        }

        private static void WriteCommandDescriptions() {
            foreach (var commandDefinition in CommandDefinitions) {
                Console.Write("'");
                ConsoleTools.Write(ConsoleColor.Green, commandDefinition.Codes[0]);
                for (var i = 1; i < commandDefinition.Codes.Length; i++) {
                    Console.Write("', '");
                    ConsoleTools.Write(ConsoleColor.Green, commandDefinition.Codes[i]);
                }
                Console.Write("' - ");
                ConsoleTools.WriteLine(ConsoleColor.White, commandDefinition.Description);
            }
        }
    }
}
