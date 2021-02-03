using System;

namespace amongus_game_flow
{
    class Program
    {
        static void Main(string[] args)
        {
            int playerIdx = 0;

            for (int i = 0; i < 2; i++)
            {
                Global.task.GenerateTask(i, 1, 1);
            }
            Global.task.DoTask(playerIdx, "Swipe Card", "Admin");
            Global.task.GetTaskProgress(playerIdx);
            Global.task.GetAllTaskProgress();
        }
    }
}
