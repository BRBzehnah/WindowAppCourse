using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerCourse.Api.Models
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
       
        
            //public void Configure(EntityTypeBuilder<Author> builder)
            //{
            //    builder.HasKey(a => a.Id);
            //    builder.HasOne(a => a.Course).WithOne(c => c.Author);
            //}

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.AllUsers).WithMany(au => au.Projects);
            builder.HasMany(p => p.AllDesks).WithOne(d => d.Project).HasForeignKey(d => d.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
