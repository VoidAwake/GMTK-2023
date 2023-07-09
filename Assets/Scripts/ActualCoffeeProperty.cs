namespace DefaultNamespace
{
    public class ActualCoffeeProperty
    {
        public string propertyName;
        public bool correct;
        public int score;
        public string value;

        public ActualCoffeeProperty(string propertyName, bool correct, int score, string value)
        {
            this.propertyName = propertyName;
            this.correct = correct;
            this.score = score;
            this.value = value;
        }
    }
}