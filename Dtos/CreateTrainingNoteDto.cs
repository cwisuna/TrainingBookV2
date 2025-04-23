namespace TrainingBookV2.Dtos
{
    public class CreateTrainingNoteDto
    {
        public int UserTrainingStepId { get; set; }
        public int AuthorId { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
