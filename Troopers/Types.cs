namespace Troopers
{
    class Game
    {
        public string Title;
        public int[,] Map;
        public string State;

        public string GetState()
        {
            return State;
        }

        public void ChangeState(string state)
        {
            State = state;
        }
    }
}