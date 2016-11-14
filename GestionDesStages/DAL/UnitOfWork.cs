using GestionDesStages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDesStages.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private GenericRepository<Departement> departementRepository;
        private GenericRepository<Encadrant> encadrantRepository;
        private GenericRepository<Stage> stageRepository;
        private GenericRepository<Stagiaire> stagiaireRepository;
        private GenericRepository<Sujet> sujetRepository;

        public GenericRepository<Departement> DepartementRepository
        {
            get
            {

                if (this.departementRepository == null)
                {
                    this.departementRepository = new GenericRepository<Departement>(context);
                }
                return departementRepository;
            }
        }

        public GenericRepository<Encadrant> EncadrantRepository
        {
            get
            {

                if (this.encadrantRepository == null)
                {
                    this.encadrantRepository = new GenericRepository<Encadrant>(context);
                }
                return encadrantRepository;
            }
        }


        public GenericRepository<Stage> StageRepository
        {
            get
            {

                if (this.stageRepository == null)
                {
                    this.stageRepository = new GenericRepository<Stage>(context);
                }
                return stageRepository;
            }
        }

        public GenericRepository<Stagiaire> StagiaireRepository
        {
            get
            {

                if (this.stagiaireRepository == null)
                {
                    this.stagiaireRepository = new GenericRepository<Stagiaire>(context);
                }
                return stagiaireRepository;
            }
        }

        public GenericRepository<Sujet> SujetRepository
        {
            get
            {

                if (this.sujetRepository == null)
                {
                    this.sujetRepository = new GenericRepository<Sujet>(context);
                }
                return sujetRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}