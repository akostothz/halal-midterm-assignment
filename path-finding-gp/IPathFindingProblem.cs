using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_finding_gp
{
    public interface IPathFindingProblem
    {
        //List<List<int>> Map { get; set; }

        //int Start_x { get; set; }
        //int Start_y { get; set; }
        //int Exit_x { get; set; }
        //int Exit_y { get; set; }

        //int Pos_x { get; set; }
        //int Pos_y { get; set; }
        //int Direction { get; set; } // 0 - up, 1 - right, 2 - down, 3 - left

        void ForwardPos(ref int x, ref int y);
        void Start();
        void TurnLeft();
        void TurnRight();
        bool IsWallForward();
        void MoveForward();
        bool IsExit();
        float DistanceToExit();
        void LoadMapFromFile(string fileName);
        void SaveMapToStream(StreamWriter file);
    }
}
