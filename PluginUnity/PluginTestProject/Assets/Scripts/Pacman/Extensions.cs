//using static DNAI.Astar2.Astar2;

//namespace Assets.Scripts.Pacman
//{
//    /// <summary>
//    /// Extension classes for help.
//    /// </summary>
//    public static class Extensions
//    {
//        public static string Display(this Position p)
//        {
//            return p.X + " " + p.Y + " " + p.Z;
//        }

//        public static Position toPosition(this UnityEngine.Vector3 p)
//        {
//            return new Position
//            {
//                X = p.x,
//                Y = p.y,
//                Z = p.z
//            };
//        }

//        public static UnityEngine.Vector3 ToVector3(this Position p)
//        {
//            return new UnityEngine.Vector3(p.X, p.Y, p.Z);
//        }

//        public static Position AsPosition(this UnityEngine.Vector2 v)
//        {
//            return new Position { X = v.x, Y = v.y, Z = 0 };
//        }

//        public static Position AsPosition(this UnityEngine.Vector3 v)
//        {
//            return new Position { X = v.x, Y = v.y, Z = v.z };
//        }
//    }
//}