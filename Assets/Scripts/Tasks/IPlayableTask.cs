using System.Collections.Generic;
using System.Linq;
using Items;

namespace Tasks
{
    public interface IPlayableTask
    {
        string LeftText();
        string RightText();
        int GetScore();

        List<Penalty> GetPenalties()
        {
            return Enumerable.Empty<Penalty>().ToList();
        }

        void Start()
        {
        }
        
        void ItemPickedUp(Item item)
        {
        }
    }
}