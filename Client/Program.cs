using System;

namespace SpaceRTS
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = GameWorld.Instance)
                game.Run();
        }
    }
}
