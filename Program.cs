using System;
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
            Console.WriteLine(Global.room.Self.isImpostor ? "impostor" : "crewmate");
            Global.skill.Use(SKILL_NAME.ShowTask, "Admin");
            Global.skill.Use(SKILL_NAME.Meeting, "");
            Global.skill.Use(SKILL_NAME.Kill, "");
            Global.skill.Use(SKILL_NAME.Damage, "1");
            Console.ReadLine();
        }
        static void TestSkillImpostor()
        {
            Global.room.selfIdx = Global.room.impIdxs[0];
            Global.skill.Use(SKILL_NAME.ShowTask, "O2");
            Global.skill.Use(SKILL_NAME.Meeting, "");
            Global.skill.Use(SKILL_NAME.Kill, "");
            Global.skill.Use(SKILL_NAME.Damage, "1");
            Console.ReadLine();
        }
    }
}
