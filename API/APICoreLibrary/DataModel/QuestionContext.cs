using Microsoft.EntityFrameworkCore;

namespace APICoreLibrary.DataModel
{
    public class QuestionContext : DbContext
    {
        public QuestionContext(DbContextOptions<QuestionContext> options): base(options)
        {

        }
        public DbSet<AnswerDM> Answers { get; set; }
        public DbSet<QuestionDM> Questions { get; set; }
        public DbSet<PlayerDM> Players { get; set; }
        public DbSet<QuestionAnswerDM> QuestionAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionDM>().HasMany(q => q.Answers).WithMany(a => a.Questions).UsingEntity<QuestionAnswerDM>(e => e
                    .HasOne(qa => qa.Answers)
                    .WithMany(a => a.QuestionAnswers)
                    .HasForeignKey(qa => qa.QA_A_ID),
                j => j
                    .HasOne(qa => qa.Question)
                    .WithMany(q => q.Question)
                    .HasForeignKey(qa => qa.QA_Q_ID));
            modelBuilder.Entity<PlayerQuestionAnswerDM>().HasOne(p => p.Question).WithMany(q => q.Players).HasForeignKey(q => q.PQA_Q_ID);
            modelBuilder.Entity<PlayerQuestionAnswerDM>().HasOne(p => p.Player).WithMany(p => p.PlayerQuestions).HasForeignKey(p => p.PQA_P_ID);
        }
    }
}
