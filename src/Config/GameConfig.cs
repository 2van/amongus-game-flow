using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaskConfigItem
{
    public string name;
    public List<string> locations;
}
public class ShortTaskConfigList : List<TaskConfigItem> { }

public class LongTaskConfigList : List<List<TaskConfigItem>> { }

public class TaskConfig
{
    public ShortTaskConfigList shortT;
    public LongTaskConfigList longT;
}

public class PlayerTaskItem
{
    public string name;
    public string location;
    public bool finished;
}
static public class GameConfig
{
    static public string TaskConfigJson = @"
{
    'shortT': [
        {
            'name': 'Swipe Card',
            'locations': [
                'Admin'
            ]
        },
        {
            'name': 'Start Reactor',
            'locations': [
                'Reactor'
            ]
        }
    ],
    'longT': [
        [
        {
            'name': 'DOWNLOAD DATA',
            'locations': [
                'Cafe'
            ]
        },
        {
            'name': 'UPLOAD DATA',
            'locations': [
                'Admin'
            ]
        }
        ]
    ]
}
";
}