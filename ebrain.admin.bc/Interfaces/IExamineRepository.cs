﻿// ======================================
// Author: Ebrain Team
// Email:  info@ebrain.com.vn
// Copyright (c) 2017 www.ebrain.com.vn
// 
// ==> Contact Us: contact@ebrain.com.vn
// ======================================

using ebrain.admin.bc.Models;
using ebrain.admin.bc.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Repositories.Interfaces
{
    public interface IExamineRepository : IRepository<Examine>
    {
        int Total { get; }
        IEnumerable<Examine> GetTopActive(int count);
        Task<Examine> FindById(Guid? id);
        Task<IEnumerable<Examine>> Search(string filter, string value, string branchIds, int page, int size);
        Task<IEnumerable<ExamineMaterialList>> SearchExamineMaterial(Guid? materialId, string branchIds);
        Task<IEnumerable<ExamineDocumentList>> SearchExamineDocument(Guid? examineId, Guid? materialId, string branchIds);
        Task<IEnumerable<Examine>> GetAllExamines(string branchIds);
        Task<Examine> Save(Examine value, Guid? id);
        Task<Boolean> Save(IEnumerable<ExamineMaterial> values);
        Task<Boolean> Save(IEnumerable<ExamineDocument> values);
        Task<Boolean> Delete(string id);
    }
}
