using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APICoreLibrary.DataModel
{
    [Table("QuestionAnswer")]
    public class QuestionAnswerDM
    {
        [Key]
        public int QA_ID { get; set; }
        public bool QA_IsCorrectAnswer { get; set; }
        public QuestionDM Question { get; set; }
        public int QA_Q_ID { get; set; }
        public AnswerDM Answers { get; set; }
        public int QA_A_ID { get; set; }

    }
}
