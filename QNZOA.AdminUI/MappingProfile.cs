using AutoMapper;
using QNZOA.Data;
using QNZOA.Model;
using System.Linq;

namespace QNZOA.AdminUI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Customer, CustomerVM>()
                 .ForMember(d => d.ProjectCount, opt => opt.MapFrom(source => source.Projects.Count));
            CreateMap<Customer, CustomerForSelectVM>();

           
            CreateMap<CustomerIM, Customer>();
            CreateMap<Customer, CustomerIM>();

            CreateMap<Project, ProjectVM>()
                  .ForMember(d => d.UserCount, opt => opt.MapFrom(source => source.UserProjects.Count))
                  .ForMember(d => d.ManagerName, opt => opt.MapFrom(source => $"{source.ManagerNavigation.RealName} [{source.ManagerNavigation.Username}]"))
                  .ForMember(d => d.TaskCount, opt => opt.MapFrom(source => source.TaskLists.Count))
                  .ForMember(d => d.CustomerName, opt => opt.MapFrom(source => source.Customer.Name));

            CreateMap<Project, ProjectDetailVM>()
                 .ForMember(d => d.UserCount, opt => opt.MapFrom(source => source.UserProjects.Count))
                 .ForMember(d => d.ManagerName, opt => opt.MapFrom(source => $"{source.ManagerNavigation.RealName} [{source.ManagerNavigation.Username}]"))
                 .ForMember(d => d.TaskCount, opt => opt.MapFrom(source => source.TaskLists.Count))
                 .ForMember(d => d.CustomerName, opt => opt.MapFrom(source => source.Customer.Name));


            CreateMap<ProjectIM, Project>();
            CreateMap<Project, ProjectIM>();

            CreateMap<ProjectBusiness, ProjectBusinessVM>()
             .ForMember(d => d.ProjectName, opt => opt.MapFrom(source => source.Project.Name))
             .ForMember(d => d.CreatedDate, opt => opt.MapFrom(source => source.Project.CreatedDate))
             .ForMember(d => d.Paymented, opt => opt.MapFrom(source => source.Paymentlogs.Where(d => d.Money > 0).Sum(d => d.Money)));

            CreateMap<ProjectBusinessIM, ProjectBusiness>();
            CreateMap<ProjectBusiness, ProjectBusinessIM>()
                 .ForMember(d => d.ProjectName, opt => opt.MapFrom(source => source.Project.Name));


            CreateMap<Paymentlog, PaymentlogVM>()
                .ForMember(d => d.ProjectName, opt => opt.MapFrom(source => source.Project.Name));
            CreateMap<PaymentlogIM, Paymentlog>();
            CreateMap<Paymentlog, PaymentlogIM>();



            //CreateMap<User, UserVM>();
            CreateMap<User, UserForSelectVM>();
            //CreateMap<UserIM, User>();
            //CreateMap<User, UserIM>();

            //CreateMap<Role, RoleIM>();
            //CreateMap<RoleIM, Role>();
            //CreateMap<Role, RoleVM>()
            //       .ForMember(d => d.UserCount, opt => opt.MapFrom(source => source.UserRoles.Count));

            //CreateMap<Menu, MenuVM>();
            //CreateMap<Menu, MenuIM>();
            //CreateMap<MenuIM, Menu>();
            //CreateMap<MenuCategory, MenuCategoryVM>();
            //CreateMap<Menu, RoleMenusIM>();




            //CreateMap<ArticleCategory, ArticleCategoryIM>();
            //CreateMap<ArticleCategoryIM, ArticleCategory>();
            //CreateMap<ArticleCategory, ArticleCategoryVM>();

            //CreateMap<Article, ArticleVM>()
            //     .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));
            //CreateMap<Article, ArticleDetailVM>()
            //     .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));


            //CreateMap<Article, ArticleIM>();
            //CreateMap<ArticleIM, Article>();

            //CreateMap<EmailAccount, EmailAccountVM>();
            //CreateMap<EmailAccount, EmailAccountIM>();
            //CreateMap<EmailAccountIM, EmailAccount>();


            //CreateMap<LinkCategory, LinkCategoryIM>();
            //CreateMap<LinkCategoryIM, LinkCategory>();
            //CreateMap<LinkCategory, LinkCategoryVM>();

            //CreateMap<Link, LinkVM>()
            //     .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(source => source.Category.Title));

            //CreateMap<Link, LinkIM>();
            //CreateMap<LinkIM, Link>();

            //CreateMap<BadgeIM, Badge>();
            //CreateMap<Badge, BadgeIM>();
            //CreateMap<Badge, BadgeVM>();

            //CreateMap<TaskIM, Task>();
            //CreateMap<Task, TaskIM>();
            //CreateMap<Task, TaskVM>()
            //     .ForMember(d => d.BadgeIds, opt => opt.MapFrom(source => source.TaskBadges.Select(d=>d.BadgeId).ToList()))
            //     .ForMember(d => d.ProjectName, opt => opt.MapFrom(source => source.Project.Name))
            //     .ForMember(d => d.Username, opt => opt.MapFrom(source =>  source.PerformerNavigation.Username))
            //     .ForMember(d => d.RealName, opt => opt.MapFrom(source => source.PerformerNavigation.RealName))
            //     .ForMember(d => d.PhotoUrl, opt => opt.MapFrom(source => source.PerformerNavigation.PhotoUrl));

            //CreateMap<Task, TaskDetailVM>()
            //   .ForMember(d => d.BadgeIds, opt => opt.MapFrom(source => source.TaskBadges.Select(d => d.BadgeId).ToList()))
            //   .ForMember(d => d.ProjectName, opt => opt.MapFrom(source => source.Project.Name))
            //   .ForMember(d => d.Username, opt => opt.MapFrom(source => source.PerformerNavigation.Username))
            //   .ForMember(d => d.RealName, opt => opt.MapFrom(source => source.PerformerNavigation.RealName));

        }
    }
}
