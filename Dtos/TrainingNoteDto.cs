namespace TrainingBookV2.Dtos
{
    public class TrainingNoteDto
    {
        public int TrainingNoteId { get; set; }
        public int UserTrainingStepId { get; set; }
        public int AuthorId { get; set; }
        public string Note { get;set; }
        public DateTime CreatedAt { get; set; }
    }
}
