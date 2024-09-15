using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(menuName = "Tasks/No Bump", fileName = "No Bump Task")]
    public class NoBumpTask : TaskSO
    {
        private class T : IPlayableTask
        {
            public string LeftText()
            {
                return "- Dodged customers";
            }

            public string RightText()
            {
                return PlayerTasks.Instance.CollidedWithNPC ? "No" : "Yes";
            }

            public int GetScore()
            {
                return PlayerTasks.Instance.CollidedWithNPC ? 0 : 300;
            }

            public List<Penalty> GetPenalties()
            {
                List<Penalty> penalties = new List<Penalty>();

                if (PlayerTasks.Instance.CollidedWithNPC)
                {
                    penalties.Add(new Penalty("Failed", 100));
                }

                return penalties;
            }
        }

        public override IPlayableTask Create()
        {
            return new T();
        }
    }
}