﻿// ======================================
// Author: Ebrain Team
// Email:  info@ebrain.com.vn
// Copyright (c) 2017 www.ebrain.com.vn
// 
// ==> Contact Us: contact@ebrain.com.vn
// ======================================

using ebrain.admin.bc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Repositories.Interfaces
{
    public interface IConsultantRepository : IRepository<Consultant>
    {
        int Total { get; }
        IEnumerable<Consultant> GetTopActive(int count);

        Task<IEnumerable<Consultant>> Search(string filter, string value, string branchIds, int page, int size);
        Task<IEnumerable<ConsultantContact>> SearchConsultant(string filter, string value, DateTime fromDate, DateTime toDate, string branchIds, int page, int size);
        Task<Consultant> Get(Guid? index);
        Task<ConsultantContact> GetConsultant(Guid? index);
        Task<ConsultantContact> SaveConsultant(ConsultantContact value, Guid? index);
        Task<Consultant> Save(Consultant value, Guid? index);
        Task<Boolean> Delete(string id);
        Task<bool> DeleteConsultant(string id);
    }
}
