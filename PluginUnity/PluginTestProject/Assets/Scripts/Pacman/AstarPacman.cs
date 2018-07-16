using DNAI.AStarFix;

namespace Assets.Scripts.Pacman
{
    public class Manager { }

    public class AstarPacman : AStarFix
    {
        private static Manager _manager;

        public int param1;
        public int param2;

        public int Output { get; }

        public class Position { float x; float y; float z; }

        public class PositionGraph
        {
            public Position getNode(int idx, PositionGraph t)
            {
                _manager = null;
                return null;
            }

            public void appendNode()
            {

            }
        }
    }

    public class Dummy : Position
    {
        public Dummy()
        {
            _manager = null;
        }
    }
}