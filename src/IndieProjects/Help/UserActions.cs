using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using IndieProjects.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IndieProjects.Help
{

    /// <summary>
    /// Разделение лайков для статей и проектов
    /// </summary>
    public enum LikeStatus
    {
        Article,
        Project
    }

    public class UserActions
    {
        /// <summary>
        /// Добавление лайка
        /// </summary>
        /// <param name="context">Конект данных</param>
        /// <param name="user">Пользователь</param>
        /// <param name="id">Id страницы</param>
        /// <param name="status">К какому типу страницы относится лайк(Проект или Статья)</param>
        /// <returns></returns>
        public async Task<int?> AddLike(IndieContext context, User user, int id, LikeStatus status)
        {
            switch (status)
            {
                case LikeStatus.Project:
                    Like like = context.Likes.Include(x => x.Project).Where(x => x.Project.ProjectID == id && x.User == user).FirstOrDefault();
                    Project project = context.Projects.Where(x => x.ProjectID == id).FirstOrDefault();
                    if (like == null)
                    {
                        Like newlike = new Like
                        {
                            Project = project,
                            User = user
                        };
                        context.Likes.Add(newlike);
                        project.Likes.Add(newlike);
                        await context.SaveChangesAsync();
                        return context.Likes.Where(x => x.Project == project).Count();
                    }
                    else
                    {
                        project.Likes.Remove(like);
                        context.Likes.Remove(like);
                        await context.SaveChangesAsync();
                        return context.Likes.Where(x => x.Project == project).Count();
                    }
               case LikeStatus.Article:
                    Like LikeArticle = context.Likes.Include(x => x.Article).Where(x => x.Article.ID == id && x.User == user).FirstOrDefault();
                    Article article = context.Articles.Where(x => x.ID == id).FirstOrDefault();
                    if (LikeArticle == null)
                    {
                        Like newlike = new Like
                        {
                            Article = article,
                            User = user
                        };
                        context.Likes.Add(newlike);
                        article.Likes.Add(newlike);
                        await context.SaveChangesAsync();
                        return context.Likes.Where(x => x.Article == article).Count();
                    }
                    else
                    {
                        article.Likes.Remove(LikeArticle);
                        context.Likes.Remove(LikeArticle);
                        await context.SaveChangesAsync();
                        return context.Likes.Where(x => x.Article == article).Count();
                    }
            }
            return null;
        }

        public static void SendMessage(User Sender, User Getter, IndieContext context)
        {
           // context.Messages.Add(new Message() {Author = Sender, Getter = Getter.Id,Content });
        }
    }
}
