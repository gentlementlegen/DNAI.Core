using static DNAI.Astar2.Astar2;

namespace Assets.Scripts.Pacman
{
    public static class Extensions
    {
        public static string Display(this Position p)
        {
            return p.X + " " + p.Y + " " + p.Z;
        }

        public static Position toPosition(this UnityEngine.Vector3 p)
        {
            return new Position
            {
                X = p.x,
                Y = p.y,
                Z = p.z
            };
        }
    }
}