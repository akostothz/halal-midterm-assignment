using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace path_finding_gp
{
    public class PathFinding : IPathFindingProgrammingProblem
    {
        public List<List<int>> map = new List<List<int>>();
        private int startX, startY, exitX, exitY;
        private int posX, posY;
        private int direction = 1;


        public void ForwardPos(ref int x, ref int y)
        {
            x = posX + ((direction == 1 || direction == 3) ? -(direction - 2) : 0);
            y = posY + ((direction == 0 || direction == 2) ? (direction - 1) : 0);
        }

        public void Start()
        {
            posX = startX;
            posY = startY;
            direction = 1;
        }

        public void TurnLeft()
        {
            direction = (direction - 1 + 4) % 4;
        }

        public void TurnRight()
        {
            direction = (direction + 1) % 4;
        }
        public bool IsWallForward()
        {
            int nextX = 0;
            int nextY = 0;
            ForwardPos(ref nextX, ref nextY);
            return map[nextY][nextX] == 1;
        }

        public void MoveForward()
        {
            if (!IsWallForward())
                ForwardPos(ref posX, ref posY);
        }
        public bool IsExit()
        {
            return posX == exitX && posY == exitY;
        }

        public float DistanceToExit()
        {
            if (IsExit())
                return 0;
            return (float)Math.Sqrt(Math.Pow(posX - exitX, 2) + Math.Pow(posY - exitY, 2));
        }

        public void LoadMapFromFile(string fileName)
        {
            map.Clear();
            List<int> mapLine = new List<int>();
            using (StreamReader fmap = new StreamReader(fileName))
            {
                int x = 0, y = 0;
                while (!fmap.EndOfStream)
                {
                    char chr = (char)fmap.Read();
                    if (chr == 13 || chr == 10)
                    {
                        if (mapLine.Count != 0)
                        {
                            map.Add(new List<int>(mapLine));
                            mapLine.Clear();
                            y++;
                            x = 0;
                        }
                    }
                    else
                    {
                        if (chr == '*')
                            mapLine.Add(1);
                        else
                        {
                            mapLine.Add(0);
                            if (chr == 'S')
                            {
                                startX = x;
                                startY = y;
                            }
                            else if (chr == 'E')
                            {
                                exitX = x;
                                exitY = y;
                            }
                        }
                        x++;
                    }
                }
            }
        }

        public void SaveMapToStream(StreamWriter file)
        {
            for (int y = 0; y <= 9; y++)
            {
                for (int x = 0; x <= 9; x++)
                {
                    char chr = ' ';
                    if (x == posX && y == posY)
                    {
                        switch (direction)
                        {
                            case 0: chr = (char)193; break;
                            case 1: chr = (char)195; break;
                            case 2: chr = (char)194; break;
                            case 3: chr = (char)180; break;
                        }
                    }
                    else if (map[y][x] == 1)
                    {
                        chr = '*';
                    }
                    else if (x == startX && y == startY)
                    {
                        chr = 'S';
                    }
                    else if (x == exitX && y == exitY)
                    {
                        chr = 'E';
                    }
                    file.Write(chr);
                }
                file.WriteLine();
            }
        }



        //PathFindingProgramProblem

        private const int TimeLimit = 1000;

        public void Execute(List<int> machineCode, StreamWriter log)
        {
            Start();
            if (log != null)
            {
                SaveMapToStream(log);
            }
            int t = 0;
            int ip = 0;
            while (!IsExit() && t < TimeLimit && ip < machineCode.Count)
            {
                int cmd = machineCode[ip];
                switch (cmd / 10)
                {
                    case 0:
                        ip -= (cmd % 10 + 1);
                        break;
                    case 1:
                        ip += (cmd % 10 - 1);
                        break;
                    case 2:
                        MoveForward();
                        break;
                    case 3:
                        TurnLeft();
                        break;
                    case 4:
                        TurnRight();
                        break;
                    case 5:
                        if (IsWallForward())
                        {
                            ip++;
                        }
                        break;
                }
                ip++;
                if (log != null)
                {
                    log.WriteLine("Time:" + t);
                    SaveMapToStream(log);
                }
                t++;
            }
        }

        public void LogExecutetion(List<int> machineCode, string logFile)
        {
            using (StreamWriter log = new StreamWriter(logFile))
            {
                log.WriteLine("Program code:");
                for (int ip = 0; ip < machineCode.Count; ip++)
                {
                    int cmd = machineCode[ip];
                    switch (cmd / 10)
                    {
                        case 0:
                            log.WriteLine("GOTO -" + (cmd % 10));
                            break;
                        case 1:
                            log.WriteLine("GOTO +" + (cmd % 10));
                            break;
                        case 2:
                            log.WriteLine("MOVE");
                            break;
                        case 3:
                            log.WriteLine("LEFT");
                            break;
                        case 4:
                            log.WriteLine("RIGHT");
                            break;
                        case 5:
                            log.WriteLine("IF_WALL_SKIP_NEXT");
                            break;
                    }
                }
                log.WriteLine();
                log.WriteLine("Execution log:");
                Execute(machineCode, log);
            }
        }

        public float Objective(List<int> machineCode)
        {
            Execute(machineCode, null);
            return DistanceToExit();
        }

        public void RunTestCase(string logFilename)
        {
            List<int> sampleCode = new List<int>
            {
            40, // 0 - LEFT
            40, // 1 - LEFT
            30, // 2 - RIGHT
            50, // 3 - IF_WALL_SKIP_NEXT
            12, // 4 - GOTO +2
            03, // 5 - GOTO -3
            20, // 6 - MOVE
            07  // 7 - GOTO -7
            };

            LogExecutetion(sampleCode, logFilename);
        }

    }
}
