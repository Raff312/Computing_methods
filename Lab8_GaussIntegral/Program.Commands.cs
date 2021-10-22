namespace Lab8_GaussIntegral {
    partial class Program {
        private static readonly List<double> _tiList = new List<double> {
            0, 0.57735026918962576450, -0.57735026918962576450, 0, 0.77459666924148337703, 
            -0.77459666924148337703, 0.33998104358485626480, -0.33998104358485626480, 
            0.86113631159405257522, -0.86113631159405257522, 0, 0.53846931010568309103, 
            -0.53846931010568309103, 0.90617984593866399279, -0.90617984593866399279,
            0.23861918608319690863, -0.23861918608319690863, 0.66120938646626451366, 
            -0.66120938646626451366, 0.93246951420315202781, -0.93246951420315202781,
            0, 0.40584515137739716690, -0.40584515137739716690, 0.74153118559939443986, 
            -0.74153118559939443986, 0.94910791234275852452, -0.94910791234275852452,
            0.18343464249564980493, -0.18343464249564980493, 0.52553240991632898581, 
            -0.52553240991632898581, 0.79666647741362673959, -0.79666647741362673959, 
            0.96028985649753623168, -0.96028985649753623168
        };

        private static readonly List<double> _wiList = new List<double> {
            2, 1, 1, 8 / 9.0, 5 / 9.0, 5 / 9.0, 0.65214515486254614262, 
            0.65214515486254614262, 0.34785484513745385737, 0.34785484513745385737,
            0.56888888888888888889, 0.47862867049936646804, 0.47862867049936646804, 
            0.23692688505618908751, 0.23692688505618908751, 0.46791393457269104738, 
            0.46791393457269104738, 0.36076157304813860756, 0.36076157304813860756,
            0.17132449237917034504, 0.17132449237917034504, 0.41795918367346938775, 
            0.38183005050511894495, 0.38183005050511894495, 0.27990539148927666790,
            0.27990539148927666790, 0.12948496616886969327, 0.12948496616886969327,
            0.36268378337836198296, 0.36268378337836198296, 0.31370664587788728733, 
            0.31370664587788728733, 0.22238103445337447054, 0.22238103445337447054, 
            0.10122853629037625915, 0.10122853629037625915
        };

        private static readonly List<double> _results = new List<double>(new double[8]);

        private class CommandDefinition {
            public string[] Codes { get; }
            public string Description { get; set; }
            public Action? Action { get; set; }

            public CommandDefinition(params string[] codes) {
                Codes = codes;
                Description = string.Empty;
            }
        }

        private static readonly CommandDefinition[] CommandDefinitions = {
            new CommandDefinition("1") {
                Description = "Run",
                Action = Run
            },
            new CommandDefinition("2") {
                Description = "Compare",
                Action = Compare
            },
            new CommandDefinition("0", "exit") {
                Description = "Exit from program",
                Action = null
            }
        };

        private static CommandDefinition? GetCommandDefinitionByCode(string code) {
            code = code.ToLowerInvariant();
            return CommandDefinitions.FirstOrDefault(x => x.Codes.Contains(code));
        }

        private static void Run() {
            var n = Utils.GetValueFromUser<int>("Enter n(1-8): ");
            if (n < 1 || n > 8) {
                throw new Exception("Incorrect n");
            }

            var a = Utils.GetValueFromUser<double>("Enter a: ");
            var b = Utils.GetValueFromUser<double>("Enter b: ");

            var integralRes = ComputeIntegral(n, a, b);
            Console.WriteLine($"Result = {integralRes}");

            _results[n - 1] = integralRes;
        }

        private static double ComputeIntegral(int n, double a, double b) {
            if (a > b) {
                Utils.Swap(ref a, ref b);
            }

            var abSum = a + b;
            var abDist = b - a;

            var sum = 0.0;
            var nSum = Sum(n - 1);
            for (var i = 0; i < n; i++) {
                var ti = _tiList[nSum + i - 1];
                var xi = (abSum + ti * abDist) / 2;
                var wi = _wiList[nSum + i - 1];
                sum += wi * Fx(xi);
            }

            return sum * abDist / 2;
        }

        private static int Sum(int n) {
            return n * (n + 1) / 2;
        }

        private static double Fx(double x) {
            return Math.Cos(1 / Math.Pow(x, 2));
        }

        private static void Compare() {
            var n1 = Utils.GetValueFromUser<int>("Enter n1: ");
            var n2 = Utils.GetValueFromUser<int>("Enter n2: ");
            var eps = Math.Abs(_results[n1 - 1] - _results[n2 - 1]);
            Console.WriteLine($"Epsilon = {eps}");
        }
    }
}