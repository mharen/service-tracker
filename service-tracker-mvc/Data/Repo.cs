using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using service_tracker_mvc.Models;
using service_tracker_mvc.Extensions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace service_tracker_mvc.Data
{
    public class Repo : IDisposable
    {
        public Repo()
        {
            db = new DataContext();
        }
        private DataContext db { get; set; }

        public IQueryable<Site> Sites
        {
            get
            {
                return HttpContext.Current.Cache.GetOrStore(
                    "Sites",
                    () => db.Sites.Include(s => s.Organization).OrderBy(c => c.Name).ToList()
                ).AsQueryable();
            }
        }

        public Site Add(Site entity)
        {
            HttpContext.Current.Cache.Remove("Sites"); // invalidate cache
            return db.Sites.Add(entity);
        }

        public Site Remove(Site entity)
        {
            HttpContext.Current.Cache.Remove("Sites"); // invalidate cache
            return db.Sites.Remove(entity);
        }

        public DbEntityEntry<Site> Entry(Site entity)
        {
            return db.Entry<Site>(entity);
        }

        public IQueryable<Servicer> Servicers
        {
            get
            {
                return HttpContext.Current.Cache.GetOrStore(
                    "Servicers",
                    () => db.Servicers.OrderBy(s => s.Name).ToList()
                ).AsQueryable();
            }
        }

        public Servicer Add(Servicer entity)
        {
            HttpContext.Current.Cache.Remove("Servicers"); // invalidate cache
            return db.Servicers.Add(entity);
        }

        public Servicer Remove(Servicer entity)
        {
            HttpContext.Current.Cache.Remove("Servicers"); // invalidate cache
            return db.Servicers.Remove(entity);
        }

        public DbEntityEntry<Servicer> Entry(Servicer entity)
        {
            return db.Entry<Servicer>(entity);
        }

        public IQueryable<Service> Services
        {
            get
            {
                return HttpContext.Current.Cache.GetOrStore(
                    "Services",
                    () => db.Services.OrderBy(c => c.Sku).ToList()).AsQueryable();
            }
        }

        public Service Add(Service entity)
        {
            HttpContext.Current.Cache.Remove("Services"); // invalidate cache
            return db.Services.Add(entity);
        }

        public Service Remove(Service entity)
        {
            HttpContext.Current.Cache.Remove("Services"); // invalidate cache
            return db.Services.Remove(entity);
        }

        public DbEntityEntry<Service> Entry(Service entity)
        {
            return db.Entry<Service>(entity);
        }

        public IQueryable<User> Users
        {
            get
            {
                return HttpContext.Current.Cache.GetOrStore(
                    "Users",
                    () => db.Users.OrderBy(c => c.Email).ToList()).AsQueryable();
            }
        }

        public User Add(User entity)
        {
            HttpContext.Current.Cache.Remove("Users"); // invalidate cache
            return db.Users.Add(entity);
        }

        public User Remove(User entity)
        {
            HttpContext.Current.Cache.Remove("Users"); // invalidate cache
            return db.Users.Remove(entity);
        }

        public DbEntityEntry<User> Entry(User entity) { return db.Entry<User>(entity); }

        public IQueryable<Profile> Profiles
        {
            get
            {
                return HttpContext.Current.Cache.GetOrStore(
                "Profiles",
                () => db.Profiles.OrderBy(c => c.Name).ToList()).AsQueryable();
            }
        }

        public Profile Add(Profile entity)
        {
            HttpContext.Current.Cache.Remove("Profiles"); // invalidate cache
            return db.Profiles.Add(entity);
        }

        public Profile Remove(Profile entity)
        {
            HttpContext.Current.Cache.Remove("Profiles"); // invalidate cache
            return db.Profiles.Remove(entity);
        }

        public DbEntityEntry<Profile> Entry(Profile entity) { return db.Entry<Profile>(entity); }

        public IQueryable<Organization> Organizations
        {
            get
            {
                return HttpContext.Current.Cache.GetOrStore(
                "Organizations",
                () => db.Organizations.OrderBy(c => c.Name).ToList()).AsQueryable();
            }
        }

        public Organization Add(Organization entity)
        {
            HttpContext.Current.Cache.Remove("Organizations"); // invalidate cache
            return db.Organizations.Add(entity);
        }

        public Organization Remove(Organization entity)
        {
            HttpContext.Current.Cache.Remove("Organizations"); // invalidate cache
            return db.Organizations.Remove(entity);
        }

        public DbEntityEntry<Organization> Entry(Organization entity)
        {
            return db.Entry<Organization>(entity);
        }

        public IQueryable<Invoice> Invoices
        {
            get
            {
                return db.Invoices.OrderBy(c => c.ServiceDate).ToList().AsQueryable();
            }
        }

        public Invoice Add(Invoice entity)
        {
            return db.Invoices.Add(entity);
        }

        public Invoice Remove(Invoice entity)
        {
            return db.Invoices.Remove(entity);
        }

        public DbEntityEntry<Invoice> Entry(Invoice entity)
        {
            return db.Entry<Invoice>(entity);
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public void Dispose() { if (db != null) { db.Dispose(); } }
    }
}