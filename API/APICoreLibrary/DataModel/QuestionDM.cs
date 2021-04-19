using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APICoreLibrary.DataModel
{
    [Table("Question")]
    public class QuestionDM
    {
        [Key]
        public int Q_ID { get; set; }
        public string Q_QuestionText { get; set; }
        public ICollection<QuestionAnswerDM> Question { get; set; }
        public ICollection<AnswerDM> Answers { get; set; }
        public ICollection<PlayerQuestionAnswerDM> Players { get; set; }
    }
}
