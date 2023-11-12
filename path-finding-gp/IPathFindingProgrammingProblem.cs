using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace path_finding_gp
{
    public interface IPathFindingProgrammingProblem : IPathFindingProblem
    {
        // machine code
        //0x - goto relative -(x) instructions   // 0 - nop
        //1x - goto relative +(x) instructions   // 10- nop
        //2x - move forward
        //3x - turn left
        //4x - turn right
        //5x - if wall exists forward, skip next instruction

        //extra variables
        const int Time_limit = 1000;
        const int Max_instruction_code = 59;

        //extra methods
        void Execute(List<int> machine_code, StreamWriter log);
        void LogExecutetion(List<int> machine_code, string log);
        float Objective(List<int> machine_code);
        void RunTestCase(string log_filename);
    }
}
