using Projektas.Server.Enums;

namespace Projektas.Server.Extensions
{
    public static class OperationExtensions
    {
        public static string GetOperationSymbol(this Operation operation)
        {
            switch (operation)
            {
                case Operation.Addition:
                    return "+";
                case Operation.Subtraction:
                    return "-";
                case Operation.Multiplication:
                    return "*";
                case Operation.Division:
                    return "/";
                default:
                    return string.Empty;
            }
        }
    }
}
