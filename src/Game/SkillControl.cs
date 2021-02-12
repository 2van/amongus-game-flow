using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace amongus_game_flow
{
    public enum SKILL_NAME
    {
        ShowTask,
        Meeting,
        Kill,
        Damage,
    }
    public class SkillControl
    {
        public SkillControl()
        {
            if (Global.room.Self.isImpostor)
            {
                AddSkill(SKILL_NAME.Kill, 10);
                AddSkill(SKILL_NAME.Damage, 10);
            }
            AddSkill(SKILL_NAME.Meeting, 0, 20);
            AddSkill(SKILL_NAME.ShowTask);
        }
        private void AddSkill(SKILL_NAME skillName, int cd = 0, int delay = 0)
        {
            this.skills.Add(skillName, new Skill(cd, delay, skillName));
        }
        public Dictionary<SKILL_NAME, Skill> skills = new Dictionary<SKILL_NAME, Skill>();
        public void Use(SKILL_NAME skill, string data)
        {
            Console.WriteLine("Use " + skill + " " + data);
            Skill _skill;
            if (!skills.TryGetValue(skill, out _skill))
            {
                return; // or whatever you want to do
            }
            long l = _skill.lastUseTime + _skill.cd - DateTimeOffset.Now.ToUnixTimeSeconds();
            if (l > 0)
            {
                return;
            }
            _skill.lastUseTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            switch (skill)
            {
                case SKILL_NAME.ShowTask:
                    this.ShowTask(data);
                    break;
                case SKILL_NAME.Meeting:
                    this.Meeting();
                    break;
                case SKILL_NAME.Kill:
                    this.Kill();
                    break;
                case SKILL_NAME.Damage:
                    this.Damage(data);
                    break;
                default:
                    break;
            }
        }
        bool isEmergency(string taskName)
        {
            return taskName == "O2" || taskName == "Fix Light";
        }
        void ShowTask(string taskName)
        {
            if (Global.room.Self.isImpostor && !isEmergency(taskName))
            {
                return;
            }
            Console.WriteLine("open mini game:" + taskName);
        }
        void Meeting()
        {
            Global.meeting.StartDiscuss();
        }
        void Kill()
        {
            if (!Global.room.Self.isImpostor)
            {
                return;
            }
            // TODO: at kill range
            int idx = Global.room.players.FindIndex(v => !v.isImpostor && !v.dead);
            if (idx >= 0)
            {
                Console.WriteLine("kill player " + idx);
                Global.room.players[idx].dead = true;
                Global.game.CheckWin();
                // sync game
            }
        }
        void Damage(string type)
        {
            Global.task.setDamage(Int16.Parse(type));
        }
    }
    public class Skill
    {
        public int cd = 10;
        private int delay = 0;
        private SKILL_NAME skillName;
        public long lastUseTime = 0;
        public Skill(int cd, int delay, SKILL_NAME skill)
        {
            this.cd = cd;
            this.delay = delay;
            this.skillName = skill;
            if (this.delay > 0)
            {
                this.lastUseTime = DateTimeOffset.Now.ToUnixTimeSeconds() + this.delay;
            }
            Task.Run(async () =>
            {
                for (; ; )
                {
                    await Task.Delay(1000);
                    Update();
                }
            });
        }

        void UpdateSkillDisplay()
        {

        }

        void Update()
        {
            if (this.skillName == SKILL_NAME.ShowTask)
            {
                return;
            }
            long l = this.lastUseTime + this.cd - DateTimeOffset.Now.ToUnixTimeSeconds();
            if (l < 0)
            {

            }
            else
            {
                Console.WriteLine(this.skillName + " CD:" + l + "s");
            }
        }
    }
}
