using System;

namespace amongus_game_flow
{
    public class SkillControl
    {
        public void Use(string skill, string data)
        {
            switch (skill)
            {
                case "ShowTask":
                    this.ShowTask(data);
                    break;
                case "Meeting":
                    this.Meeting();
                    break;
                case "Kill":
                    this.Kill();
                    break;
                case "Pohuai":
                    this.Pohuai(data);
                    break;
                default:
                    break;
            }
        }
        public void ShowTask(string taskName)
        {
            Console.WriteLine("open mini game:" + taskName);
        }
        public void Meeting()
        {
            Global.meeting.StartDiscuss();
        }
        public void Kill()
        {
            // TODO: at kill range
            int idx = Global.room.players.FindIndex(v => !v.isImpostor && !v.dead);
            if (idx >= 0)
            {
                Console.WriteLine("kill player " + idx);
                Global.room.players[idx].dead = true;
                // sync game
            }
        }
        public void Pohuai(string type)
        {
            Global.task.setJinji(Int16.Parse(type));
        }
    }
    public class Skill
    {
        private int cd = 10;
        private int delay = 0;
        private string skillName = "";
        private long lastUseTime = 0;
        public Skill(int cd, int delay, string skill)
        {
            this.cd = cd;
            this.delay = delay;
            this.skillName = skill;
            if (this.delay > 0)
            {
                this.lastUseTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                    + this.cd - this.delay;
            }
        }

        void Use(string skill, string data)
        {
            this.skillName = skill;
            Global.skill.Use(skill, data);
            this.lastUseTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        void UpdateSkillDisplay()
        {

        }

        void Update()
        {
            if (this.skillName == "use")
            {
                return;
            }
            long l = this.lastUseTime + this.cd - DateTimeOffset.Now.ToUnixTimeSeconds();
            if (l < 0)
            {

            }
            else
            {
                Console.WriteLine(this.skillName + ":" + l);
            }
        }
    }
}
