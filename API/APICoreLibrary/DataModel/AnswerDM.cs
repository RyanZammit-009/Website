using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APICoreLibrary.DataModel
{
    [Table("Answer")]
    public class AnswerDM
    {
        [Key]
        public int A_ID { get; set; }
        public string A_AnswerText { get; set; }
        public ICollection<QuestionDM> Questions { get; set; }
        public ICollection<QuestionAnswerDM> QuestionAnswers { get; set; }

    }
}
