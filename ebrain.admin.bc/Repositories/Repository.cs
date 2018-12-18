﻿// ======================================
// Author: Ebrain Team
// Email:  info@ebrain.com.vn
// Copyright (c) 2017 www.ebrain.com.vn
// 
// ==> Contact Us: contact@ebrain.com.vn
// ======================================

using ebrain.admin.bc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        //public virtual void Add(TEntity entity)
        //{
        //    _entities.Add(entity);
        //}

        //public virtual void AddRange(IEnumerable<TEntity> entities)
        //{
        //    _entities.AddRange(entities);
        //}


        //public virtual void Update(TEntity entity)
        //{
        //    _entities.Update(entity);
        //}

        //public virtual void UpdateRange(IEnumerable<TEntity> entities)
        //{
        //    _entities.UpdateRange(entities);
        //}



        //public virtual void Remove(TEntity entity)
        //{
        //    _entities.Remove(entity);
        //}

        //public virtual void RemoveRange(IEnumerable<TEntity> entities)
        //{
        //    _entities.RemoveRange(entities);
        //}


        //public virtual int Count()
        //{
        //    return _entities.Count();
        //}


        //public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return _entities.Where(predicate);
        //}

        //public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        //{
        //    return _entities.SingleOrDefault(predicate);
        //}

        //public virtual TEntity Get(int id)
        //{
        //    return _entities.Find(id);
        //}

        public async Task<bool> Execute(string command, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(command, parameters) > 0;
        }
    }
}
