using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APICoreLibrary.DataModel
{
    [Table("PlayerQuestionAnswer")]
    public class PlayerQuestionAnswerDM
    {
        [Key]
        public int PQA_ID { get; set; }
        public PlayerDM Player { get; set; }
        public int PQA_P_ID { get; set; }
        public QuestionDM Question { get; set; }
        public int PQA_Q_ID { get; set; }
        public bool PQA_IsAnswered { get; set; }
        public bool PQA_IsCorrect { get; set; }
    }
}
