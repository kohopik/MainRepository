using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using IndieProjects.Model;

namespace IndieProjects.Migrations
{
    [DbContext(typeof(IndieContext))]
    [Migration("20170224151320_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IndieProjects.Model.Article", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateOfPublish");

                    b.Property<int>("Likes");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("IndieProjects.Model.ArticleCommentaries", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArticleID");

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateSend");

                    b.Property<int?>("ParentCommentaryID");

                    b.HasKey("ID");

                    b.HasIndex("ArticleID");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentCommentaryID");

                    b.ToTable("ArticleCommentaries");
                });

            modelBuilder.Entity("IndieProjects.Model.ChatMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("DateSend");

                    b.Property<int>("ProjectId");

                    b.HasKey("ID");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("IndieProjects.Model.DeveloperProject", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("ProjectID");

                    b.HasKey("UserId", "ProjectID");

                    b.HasIndex("ProjectID");

                    b.HasIndex("UserId");

                    b.ToTable("DeveloperProject");
                });

            modelBuilder.Entity("IndieProjects.Model.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateOfSend");

                    b.Property<string>("Getter");

                    b.Property<int>("MessageStatus");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("IndieProjects.Model.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<DateTime>("DateOfPublish");

                    b.Property<string>("Description");

                    b.Property<int>("Likes");

                    b.Property<string>("Links");

                    b.Property<string>("Name");

                    b.Property<string>("ProjectManagerId");

                    b.Property<int>("StatusProject");

                    b.HasKey("ProjectID");

                    b.HasIndex("ProjectManagerId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("IndieProjects.Model.ProjectCommentaries", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateSend");

                    b.Property<int?>("ParentCommentaryID");

                    b.Property<int?>("ProjectID");

                    b.HasKey("ID");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentCommentaryID");

                    b.HasIndex("ProjectID");

                    b.ToTable("ProjectCommentaries");
                });

            modelBuilder.Entity("IndieProjects.Model.Summary", b =>
                {
                    b.Property<string>("UserID");

                    b.Property<string>("AboutHimself");

                    b.Property<string>("LinksToProject");

                    b.Property<int>("SummaryStatus");

                    b.HasKey("UserID");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("IndieProjects.Model.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArticleID");

                    b.Property<string>("Name");

                    b.HasKey("TagId");

                    b.HasIndex("ArticleID");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("IndieProjects.Model.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("AboutHimself");

                    b.Property<string>("AboutMe");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Avatar");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FIO");

                    b.Property<bool>("IsDeveloper");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NickName");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("OwnSite");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Portfolio");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Skype");

                    b.Property<int>("Status");

                    b.Property<string>("ThePost");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("IndieProjects.Model.Vakanci", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateOfPublish");

                    b.Property<int?>("ProjectID");

                    b.Property<string>("Requirements");

                    b.Property<string>("Responsibilities");

                    b.Property<int>("StatusVakancy");

                    b.Property<string>("Title");

                    b.Property<int>("TypeOfEmployment");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Vakancies");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("IndieProjects.Model.Article", b =>
                {
                    b.HasOne("IndieProjects.Model.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("IndieProjects.Model.ArticleCommentaries", b =>
                {
                    b.HasOne("IndieProjects.Model.Article", "Article")
                        .WithMany("Commentaries")
                        .HasForeignKey("ArticleID");

                    b.HasOne("IndieProjects.Model.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("IndieProjects.Model.ArticleCommentaries", "ParentCommentary")
                        .WithMany("ChildsCommentary")
                        .HasForeignKey("ParentCommentaryID");
                });

            modelBuilder.Entity("IndieProjects.Model.ChatMessage", b =>
                {
                    b.HasOne("IndieProjects.Model.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("IndieProjects.Model.Project", "Chat")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IndieProjects.Model.DeveloperProject", b =>
                {
                    b.HasOne("IndieProjects.Model.Project", "Project")
                        .WithMany("Team")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IndieProjects.Model.User", "User")
                        .WithMany("Projects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IndieProjects.Model.Message", b =>
                {
                    b.HasOne("IndieProjects.Model.User", "Author")
                        .WithMany("Messages")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("IndieProjects.Model.Project", b =>
                {
                    b.HasOne("IndieProjects.Model.User", "ProjectManager")
                        .WithMany()
                        .HasForeignKey("ProjectManagerId");
                });

            modelBuilder.Entity("IndieProjects.Model.ProjectCommentaries", b =>
                {
                    b.HasOne("IndieProjects.Model.User", "Author")
                        .WithMany("Commentaries")
                        .HasForeignKey("AuthorId");

                    b.HasOne("IndieProjects.Model.ProjectCommentaries", "ParentCommentary")
                        .WithMany("ChildsCommentary")
                        .HasForeignKey("ParentCommentaryID");

                    b.HasOne("IndieProjects.Model.Project", "Project")
                        .WithMany("Commentaries")
                        .HasForeignKey("ProjectID");
                });

            modelBuilder.Entity("IndieProjects.Model.Summary", b =>
                {
                    b.HasOne("IndieProjects.Model.User", "Developer")
                        .WithOne("Summary")
                        .HasForeignKey("IndieProjects.Model.Summary", "UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IndieProjects.Model.Tag", b =>
                {
                    b.HasOne("IndieProjects.Model.Article")
                        .WithMany("Tags")
                        .HasForeignKey("ArticleID");
                });

            modelBuilder.Entity("IndieProjects.Model.Vakanci", b =>
                {
                    b.HasOne("IndieProjects.Model.Project", "Project")
                        .WithMany("Vakancies")
                        .HasForeignKey("ProjectID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("IndieProjects.Model.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("IndieProjects.Model.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IndieProjects.Model.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
