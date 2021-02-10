using System;
using System.Collections.Generic;
using System.Text;

namespace amongus_game_flow
{
    public class Global
    {
        public static GameState game = GameState.Instance;
        public static TaskControl task = TaskControl.Instance;
        public static MeetingControl meeting = MeetingControl.Instance;
        public static SkillControl skill = new SkillControl();
        public static Room room = new Room();
    }
}
