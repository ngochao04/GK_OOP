using System;
using System.Collections.Generic;

namespace GKOOP
{
    public class AnswerDto
    {
        public Guid Id { get; set; }                
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }

        public Guid TopicId { get; set; }
        public string Content { get; set; }

        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class ExamDraft
    {
        public string Name { get; set; }
        public Guid SubjectId { get; set; }         
        public int DurationMinutes { get; set; }     
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
