using Projektas.Shared.Enums;

namespace Projektas.Shared.Extensions
{
    public static class GameModeExtension
    {
        public static string? GameModeToString(this GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.FourByFour:
                    return "4x4";
                case GameMode.NineByNine:
                    return "9x9";
                case GameMode.SixteenBySixteen:
                    return "16x16";
                default:
                    return null;
            }
        }
    }
}
