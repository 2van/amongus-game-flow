using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace amongus_game_flow
{
    public class TaskControl
    {
        static private TaskControl _instance;
        static public TaskControl Instance
        {
            get
            {
                if (TaskControl._instance is null)
                {
                    TaskControl._instance = new TaskControl();
                }
                return TaskControl._instance;
            }
        }
        readonly private Dictionary<int, List<List<PlayerTaskItem>>> allTaskData = new Dictionary<int, List<List<PlayerTaskItem>>>();

        public List<List<PlayerTaskItem>> GenerateTask(int idx, int shortCount = 1, int longCount = 1)
        {
            TaskConfig taskconfig = JsonConvert.DeserializeObject<TaskConfig>(GameConfig.TaskConfigJson);
            List<TaskConfigItem> s = taskconfig.shortT.random(shortCount).ToList();
            List<List<TaskConfigItem>> l = taskconfig.longT.random(longCount).ToList();
            var newS = s.ConvertAll(v =>
            {
                var a = new PlayerTaskItem
                {
                    name = v.name,
                    location = v.locations.random(1).Single(),
                    finished = false,
                };
                return new List<PlayerTaskItem> { a };
            });
            var newL = l.ConvertAll(v =>
            {
                return v.ConvertAll(item =>
                  new PlayerTaskItem
                  {
                      name = item.name,
                      location = item.locations.random(1).Single(),
                      finished = false,
                  });
            });
            var list = newS.Concat(newL).ToList();
            allTaskData[idx] = list;

            Console.WriteLine("generateTask\n" + JsonConvert.SerializeObject(list, Formatting.Indented));
            return list;
        }
        public bool IsTaskItemFinished(string name, string location)
        {
            int idx = Global.room.selfIdx;
            if (allTaskData[idx] == null)
            {
                return false;
            }
            return allTaskData[idx].Exists(v => v.Exists(vv => vv.name == name && vv.location == location));
        }
        public bool DoTask(int idx, string name, string location)
        {
            if (allTaskData[idx] == null)
            {
                return false;
            }
            bool rs = allTaskData[idx].Exists(v => v.Exists(vv =>
            {
                if (vv.name == name && vv.location == location && !vv.finished)
                {
                    vv.finished = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }));
            if (rs)
            {
                Console.WriteLine("do task succ " + name + location);
            }
            else
            {
                Console.WriteLine("do task fail-unexisting " + name + location);
            }
            return rs;
        }
        public class TaskState
        {
            public bool finished;
            public List<int> progress;
            public PlayerTaskItem current;
        }

        private Dictionary<int, List<List<PlayerTaskItem>>> RealTaskData => allTaskData
            .Where((v, i) => !Global.room.players[i].isImpostor)
            .ToDictionary(i => i.Key, i => i.Value);
        public List<TaskState> GetTaskProgress(int idx)
        {
            var list = RealTaskData[idx].ConvertAll(v =>
            {
                var progress = new List<int> { v.Where(vv => vv.finished).Count(), v.Count };
                var current = v[progress[0] == v.Count ? v.Count - 1 : progress[0]];

                return new TaskState
                {
                    finished = progress[0] == progress[1],
                    progress = progress,
                    current = current
                };
            });
            return list;
        }

        public List<int> GetAllTaskProgress()
        {
            int total = 0;
            int finished = 0;
            foreach (KeyValuePair<int, List<List<PlayerTaskItem>>> entry in RealTaskData)
            {
                RealTaskData[entry.Key].ForEach(v =>
                {
                    total++;
                    if (v.All(vv => vv.finished))
                    {
                        finished++;
                    }
                });
            }
            Console.WriteLine("getAllTaskProgress " + finished + " " + total);
            return new List<int> { finished, total };
        }

        private void ShowMyTask()
        {
            List<TaskState> taskState = Global.task.GetTaskProgress(Global.room.selfIdx);
            List<string> taskDisplay = taskState.ConvertAll(t =>
            {
                string txt1 = t.finished ? "[Finished]" : $"[{t.current.location}]";
                string txt2 = t.current.name;
                string txt3 = String.Join("/", t.progress);
                return String.Join(" ", new string[] { txt1, txt2, txt3 });
            });
            Console.WriteLine("MyTask " + String.Join("\n", taskDisplay));
        }
        private void ShowAllTaskProgress()
        {
            List<int> allPorgress = Global.task.GetAllTaskProgress();
            Console.WriteLine("AllTaskProgress " + String.Join("/", allPorgress));
        }
        public void ShowTaskInfo()
        {
            ShowMyTask();
            ShowAllTaskProgress();
        }
    }
}