
using System;
using System.Collections.Generic;
using System.Threading;

namespace amongus_game_flow
{
    class Program
    {
        static void Main(string[] args)
        {
            string type = args[0];
            switch (type)
            {
                case "task":
                    TestTaskflow();
                    break;
                case "meeting":
                    TestMeetingflow();
                    break;
                case "skill-crewmate":
                    TestSkillCrewmate();
                    break;
                case "skill-impostor":
                    TestSkillImpostor();
                    break;
                case "game":
                    TestGameFlow();
                    break;
                default:
                    break;
            }

        }

        static void TestTaskflow()
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
        static void TestMeetingflow()
        {
            Global.meeting.StartDiscuss();
            Thread.Sleep(GameConfig.DiscussionTime * 1000 + 500);
            Global.meeting.Vote(0, 1);
            Global.meeting.Vote(1, 1);
            Global.meeting.Vote(2, 0);
            Console.ReadLine();
        }

        static void TestSkillCrewmate()
        {
            Global.skill.Use(SKILL_NAME.ShowTask, "Admin");
            Global.skill.Use(SKILL_NAME.Meeting, "");
            Global.skill.Use(SKILL_NAME.Kill, "");
            Global.skill.Use(SKILL_NAME.Damage, "1");
            Console.ReadLine();
        }
        static void TestSkillImpostor()
        {
            Global.skill.Use(SKILL_NAME.ShowTask, "O2");
            Global.skill.Use(SKILL_NAME.Meeting, "");
            Global.skill.Use(SKILL_NAME.Kill, "");
            Global.skill.Use(SKILL_NAME.Damage, "1");
            Console.ReadLine();
        }

        static void TestGameFlow()
        {
            // 做任务结束
            foreach (KeyValuePair<int, List<List<PlayerTaskItem>>> entry in Global.task.RealTaskData)
            {
                Global.task.RealTaskData[entry.Key].ForEach(v =>
                {
                    v.ForEach(vv => Global.task.DoTask(entry.Key, vv.name, vv.location));
                });
            }
            // 紧急结束
            Global.skill.Use(SKILL_NAME.Damage, "1");
            // TODO: Reset Game
            Console.ReadLine();

        }
    }
}
