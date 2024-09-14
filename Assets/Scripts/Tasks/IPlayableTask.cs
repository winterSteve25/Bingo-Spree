using Items;

namespace Tasks
{
    public interface IPlayableTask
    {
        string LeftText();
        string RightText();
        int GetScore();
        
        void Start();
        void ItemPickedUp(Item item);
    }
}