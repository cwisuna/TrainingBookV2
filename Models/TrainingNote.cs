namespace TrainingBookV2.Models
{
    public class TrainingNote
    {
        public int TrainingNoteID { get; set; }
        public int UserTrainingStepID { get; set; }
        public UserTrainingStep UserTrainingStep { get; set; }
        public int AuthorID { get; set; }
        public ApplicationUser Author { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
