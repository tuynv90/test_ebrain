﻿// ======================================
// Author: Ebrain Team
// Email:  info@ebrain.com.vn
// Copyright (c) 2017 www.ebrain.com.vn
// 
// ==> Contact Us: contact@ebrain.com.vn
// ======================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ebrain.admin.bc.Models;
using ebrain.admin.bc.Repositories.Interfaces;
using ebrain.admin.bc.Utilities;
using ebrain.admin.bc.Report;

namespace ebrain.admin.bc.Repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public int Total { get; private set; }

        public ClassRepository(ApplicationDbContext context) : base(context)
        { }


        public IEnumerable<Class> GetTopActive(int count)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Class>> GetClassCurrent(Guid? studentId)
        {
            var dt = DateTime.Now.Date;
            var list = new List<Class>();
            // get current date between fromDate and toDate
            var classes = this.appContext.ClassStudent.Where
                (p => !p.IsDeleted
                 && p.StudentId == studentId
                 //&& p.StartDate.HasValue && p.StartDate.Value.Date <= dt
                 //&& p.EndDate.HasValue && p.EndDate.Value.Date >= dt
                 ).OrderByDescending(p => p.UpdatedDate);
            foreach (var item in classes)
            {
                var cl = this.appContext.Class.FirstOrDefault(p => p.ClassId == item.ClassId);
                if (cl != null)
                {
                    list.Add(cl);
                }
            }
            return list;
        }

        public DateTime? GetClassEndDate(Guid? materialId, Guid? classId, Guid? studentId, DateTime? fromDate)
        {
            var toDate = fromDate.HasValue ? fromDate.Value.AddMonths(4) : DateTime.Now;

            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_ScheduleStudent_EndDate")
                               .WithSqlParam("@materialId", (materialId != null ? materialId.ToString() : null))
                               .WithSqlParam("@classId", (classId != null ? classId.ToString() : null))
                               .WithSqlParam("@studentId", (studentId != null ? studentId.ToString() : null))
                               .WithSqlParam("@fromDate", fromDate)
                               .WithSqlParam("@toDate", toDate)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                return someTypeList.Count > 0 ? someTypeList.FirstOrDefault().EndDate.Value : (DateTime?)null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Class>> Search(string filter, string value, Guid? userLogin, string branchIds)
        {
            var cls = await this.appContext.Class.Where(p => p.IsDeleted == false &&
                   branchIds.Contains(p.BranchId.ToString())
                ).ToListAsync();
            if (userLogin.HasValue)
            {
                var sup = this.appContext.Supplier.FirstOrDefault(p => p.UserLoginId == userLogin);
                if (sup != null)
                {
                    cls = cls.Where(p => p.SupplierId == sup.SupplierId).ToList();
                }
                else
                {
                    cls = new List<Class>();
                }

            }
            return cls;
        }

        public async void SaveStudent(Class[] classes, Guid? studentId, Guid createById, string branchIds)
        {
            var branchId = createById.GetBranchOfCurrentUser(this.appContext);

            var classOfStudents = GetClassStudent(branchIds, null, null, null, null, studentId);

            var classIds = classes.Select(p => p.ClassId);

            var iodStNotExists = classOfStudents.Where(p => !classIds.Contains(p.ClassId));

            //Isdeleted = true 
            foreach (var itemDetail in iodStNotExists)
            {
                var itemStudent = this.appContext.ClassStudent.FirstOrDefault(p => p.ClassId == itemDetail.ClassId && p.StudentId == studentId && p.IsDeleted == false);
                if (itemStudent != null)
                {
                    itemStudent.IsDeleted = true;
                }
            }

            //set students 
            foreach (var item in classes)
            {
                var itemClassExist = this.appContext.ClassStudent.FirstOrDefault(
                    p => p.ClassId == item.ClassId && p.StudentId == studentId
                    && p.IOStockId == item.IOStockId
                    && p.IOStockDetailId == item.IOStockDetailId
                    && p.IsDeleted == false);
                if (itemClassExist != null)
                {
                    itemClassExist.UpdatedBy = createById;
                    itemClassExist.UpdatedDate = DateTime.Now;
                    itemClassExist.MaterialId = item.MaterialId;
                    itemClassExist.StartDate = item.StartDate;
                    itemClassExist.EndDate = item.EndDate;
                    itemClassExist.IOStockId = item.IOStockId;
                    itemClassExist.BranchId = branchId;
                    itemClassExist.ClassId = item.ClassId;
                    itemClassExist.IOStockDetailId = item.IOStockDetailId;
                }
                else
                {
                    itemClassExist = new ClassStudent
                    {
                        StudentId = studentId,
                        ClassStudentId = Guid.NewGuid(),
                        ClassId = item.ClassId,
                        CreatedBy = createById,
                        UpdatedBy = createById,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        MaterialId = item.MaterialId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IOStockId = item.IOStockId,
                        IOStockDetailId = item.IOStockDetailId,
                        BranchId = branchId
                    };
                    await this.appContext.ClassStudent.AddAsync(itemClassExist);
                }
            }

            appContext.SaveChanges();

        }

        public async void SaveClassExamine(ClassExamine[] examines)
        {
            foreach (var item in examines)
            {
                var itemExist = this.appContext.ClassExamine.FirstOrDefault
                                    (
                                        p => p.StudentId == item.StudentId
                                        && p.ClassId == item.ClassId
                                        && p.ExamineId == item.ExamineId
                                    );
                if (itemExist != null)
                {
                    itemExist.Mark = item.Mark;
                }
                else
                {
                    await this.appContext.ClassExamine.AddAsync(item);
                }

            }
            this.appContext.SaveChanges();
        }

        public async void SaveClassExamineNote(ClassExamineNote[] examines)
        {
            foreach (var item in examines)
            {
                var itemExist = this.appContext.ClassExamineNote.FirstOrDefault
                                    (
                                        p => p.StudentId == item.StudentId
                                        && p.ClassId == item.ClassId
                                        && p.ExamineNoteId == item.ExamineNoteId
                                        && p.ExamineId == item.ExamineId
                                    );
                if (itemExist != null)
                {
                    itemExist.Attend = item.Attend;
                    itemExist.NotAttend = item.NotAttend;
                }
                else
                {
                    await this.appContext.ClassExamineNote.AddAsync(item);
                }

            }
            this.appContext.SaveChanges();
        }


        public async Task<bool> SaveClassOffset(ClassOffset[] classes, Guid? classId, Guid? studentId)
        {
            try
            {
                if (classes != null && classes.Length > 0)
                {
                    var classIds = classes.Select(p => p.ClassOffsetId).ToArray();
                    var iodStNotExists = this.appContext.ClassOffset.Where(p => !classIds.Contains(p.ClassOffsetId));

                    //Isdeleted = true 
                    foreach (var itemDetail in iodStNotExists)
                    {
                        var itemNot = this.appContext.ClassOffset.FirstOrDefault(p => p.ClassOffsetId == itemDetail.ClassOffsetId);
                        if (itemNot != null)
                        {
                            itemNot.IsDeleted = true;
                        }
                    }

                    //updated values
                    foreach (var item in classes)
                    {
                        var itemExist = this.appContext.ClassOffset.FirstOrDefault(p => p.ClassOffsetId == item.ClassOffsetId && p.IsDeleted == false);
                        if (itemExist == null)
                        {
                            item.CreatedDate = DateTime.Now;
                            item.UpdatedDate = DateTime.Now;
                            this.appContext.ClassOffset.Add(item);
                        }

                        studentId = item.StudentId;
                        classId = item.ClassId;

                    }
                }
                else
                {
                    var iodStNotExists = this.appContext.ClassOffset.Where(p => p.ClassId == classId && p.StudentId == studentId);
                    foreach (var itemNot in iodStNotExists)
                    {
                        itemNot.IsDeleted = true;
                    }
                }

                appContext.SaveChanges();
                // update endDate
                return this.UpdateEndDate(studentId, classId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool UpdateEndDate(Guid? studentId, Guid? classId)
        {
            // update endDate
            var itemStudent = this.appContext.ClassStudent.FirstOrDefault(p => p.StudentId == studentId && p.ClassId == classId && !p.IsDeleted);
            if (itemStudent != null)
            {
                var dt = this.GetClassEndDate(itemStudent.MaterialId, classId, studentId, itemStudent.StartDate);
                if (dt.HasValue)
                {
                    itemStudent.EndDate = dt;
                }
            }
            return appContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveClassEx(ClassEx[] classes, Guid? classId, Guid? studentId)
        {
            if (classes != null && classes.Length > 0)
            {
                var classIds = classes.Select(p => p.ClassExId).ToArray();
                var iodStNotExists = this.appContext.ClassEx.Where(p => !classIds.Contains(p.ClassExId));

                //Isdeleted = true 
                foreach (var itemDetail in iodStNotExists)
                {
                    var itemNot = this.appContext.ClassEx.FirstOrDefault(p => p.ClassExId == itemDetail.ClassExId);
                    if (itemNot != null)
                    {
                        itemNot.IsDeleted = true;
                    }
                }

                foreach (var item in classes)
                {
                    var itemExist = this.appContext.ClassEx.FirstOrDefault(p => p.ClassExId == item.ClassExId && p.IsDeleted == false);
                    if (itemExist == null)
                    {
                        item.CreatedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        this.appContext.ClassEx.Add(item);
                    }
                }
            }
            else
            {
                var iodStNotExists = this.appContext.ClassPending.Where(p => p.ClassId == classId && p.StudentId == studentId);
                foreach (var itemNot in iodStNotExists)
                {
                    itemNot.IsDeleted = true;
                }
            }
            return appContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveClassPending(ClassPending[] classes, Guid? classId, Guid? studentId)
        {
            if (classes != null && classes.Length > 0)
            {
                var classIds = classes.Select(p => p.ClassPendingId).ToArray();
                var iodStNotExists = this.appContext.ClassPending.Where(p => !classIds.Contains(p.ClassPendingId));

                //Isdeleted = true 
                foreach (var itemDetail in iodStNotExists)
                {
                    var itemNot = this.appContext.ClassPending.FirstOrDefault(p => p.ClassPendingId == itemDetail.ClassPendingId);
                    if (itemNot != null)
                    {
                        itemNot.IsDeleted = true;
                    }
                }

                foreach (var item in classes)
                {
                    var itemExist = this.appContext.ClassPending.FirstOrDefault(p => p.ClassPendingId == item.ClassPendingId && p.IsDeleted == false);
                    if (itemExist == null)
                    {
                        item.CreatedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        this.appContext.ClassPending.Add(item);
                    }
                    studentId = item.StudentId;
                    classId = item.ClassId;
                }
            }
            else
            {
                var iodStNotExists = this.appContext.ClassPending.Where(p => p.ClassId == classId && p.StudentId == studentId);
                foreach (var itemNot in iodStNotExists)
                {
                    itemNot.IsDeleted = true;
                }
            }
            appContext.SaveChanges();
            // update endDate
            return this.UpdateEndDate(studentId, classId);
        }

        private DateTime? GetLastEndDateOfClass(Guid? classId, Guid? studentId)
        {
            var list = this.GetScheduleStudent(classId, studentId, 0, 0);
            if (list.Count > 0)
            {
                return list.OrderByDescending(p => p.LearnDate).First().LearnDate;
            }
            return null;
        }

        public async Task<Class> Save(Class value, ClassTime[] classTimes, ClassStudent[] classStudents, Guid? index)
        {
            value.BranchId = value.CreatedBy.GetBranchOfCurrentUser(this.appContext);
            var itemExist = await Get(index);
            if (itemExist != null)
            {
                itemExist.ClassCode = value.ClassCode;
                itemExist.ClassName = value.ClassName;
                itemExist.Note = value.Note;
                itemExist.UpdatedBy = value.UpdatedBy;
                itemExist.UpdatedDate = DateTime.Now;
                itemExist.BranchId = value.BranchId;
                itemExist.StartDate = value.StartDate;
                itemExist.EndDate = value.EndDate;
                itemExist.StatusId = value.StatusId;
                itemExist.SupplierId = value.SupplierId;
                itemExist.MaxStudent = value.MaxStudent;
                itemExist.MaterialId = value.MaterialId;
                itemExist.LongLearn = value.LongLearn;

                var cTimeExists = await GeTimeByClassId(index);
                //update deleted
                var iodIds = classTimes.Select(p => p.ClassTimeId);
                var iodNotExists = cTimeExists.Where(p => !iodIds.Contains(p.ClassTimeId));
                foreach (var itemDetail in iodNotExists)
                {
                    itemDetail.IsDeleted = true;
                }

                //set times
                foreach (var item in classTimes)
                {
                    var itemClassExist = await this.appContext.ClassTime.FirstOrDefaultAsync(p => p.ClassTimeId == item.ClassTimeId);
                    if (itemClassExist != null)
                    {
                        itemClassExist.BranchId = value.BranchId;
                        itemClassExist.EndTime = item.EndTime;
                        itemClassExist.StartTime = item.StartTime;
                        itemClassExist.SupplierId = item.SupplierId;
                        itemClassExist.RoomId = item.RoomId;
                        itemClassExist.OnTodayId = item.OnTodayId;
                        itemClassExist.ClassId = itemExist.ClassId;
                    }
                    else
                    {
                        item.ClassId = itemExist.ClassId;
                        await this.appContext.ClassTime.AddAsync(item);
                    }
                }

                var cStudentExists = await GeStudentByClassId(index);
                //update deleted
                var iodStIds = classStudents.Select(p => p.ClassStudentId);
                var iodStNotExists = cStudentExists.Where(p => !iodStIds.Contains(p.ClassStudentId));
                foreach (var itemDetail in iodStNotExists)
                {
                    itemDetail.IsDeleted = true;
                    // updated  offset
                    var offset = this.appContext.ClassOffset.Where(p => p.ClassId == itemDetail.ClassId && p.StudentId == itemDetail.StudentId);
                    foreach (var itemNot in offset)
                    {
                        itemNot.IsDeleted = true;
                    }
                    // updated exes
                    var exes = this.appContext.ClassEx.Where(p => p.ClassId == itemDetail.ClassId && p.StudentId == itemDetail.StudentId);
                    foreach (var itemNot in exes)
                    {
                        itemNot.IsDeleted = true;
                    }
                    // updated pending
                    var pending = this.appContext.ClassPending.Where(p => p.ClassId == itemDetail.ClassId && p.StudentId == itemDetail.StudentId);
                    foreach (var itemNot in pending)
                    {
                        itemNot.IsDeleted = true;
                    }
                }

                //set students 
                foreach (var item in classStudents)
                {
                    var itemClassExist = await this.appContext.ClassStudent.FirstOrDefaultAsync(p => p.ClassStudentId == item.ClassStudentId);
                    if (itemClassExist != null)
                    {
                        itemClassExist.BranchId = value.BranchId;
                        itemClassExist.StudentId = item.StudentId;
                        itemClassExist.ClassId = itemClassExist.ClassId;
                        itemClassExist.StartDate = item.StartDate;
                        itemClassExist.EndDate = item.EndDate;
                        itemClassExist.MaterialId = item.MaterialId;
                        itemClassExist.IOStockId = item.IOStockId;
                        itemClassExist.IOStockDetailId = item.IOStockDetailId;
                    }
                    else
                    {
                        item.ClassId = itemExist.ClassId;
                        await this.appContext.ClassStudent.AddAsync(item);
                    }
                }
            }
            else
            {
                var result = await appContext.Class.AddAsync(value);
                var classId = result.Entity.ClassId;
                //Times
                foreach (var item in classTimes)
                {
                    item.ClassTimeId = Guid.NewGuid();
                    item.ClassId = classId;
                    item.BranchId = value.BranchId;
                }
                //Students
                foreach (var item in classStudents)
                {
                    item.ClassStudentId = Guid.NewGuid();
                    item.ClassId = classId;
                    item.BranchId = value.BranchId;
                }
                await appContext.ClassTime.AddRangeAsync(classTimes);
                await appContext.ClassStudent.AddRangeAsync(classStudents);
                itemExist = result.Entity;
            }

            await appContext.SaveChangesAsync();
            return itemExist;
        }

        private async Task<IEnumerable<ClassTime>> GeTimeByClassId(Guid? classId)
        {
            return await this.appContext.ClassTime.Where(p => p.IsDeleted == false && p.ClassId == classId).ToListAsync();
        }

        private async Task<IEnumerable<ClassStudent>> GeStudentByClassId(Guid? classId)
        {
            return await this.appContext.ClassStudent.Where(p => p.IsDeleted == false && p.ClassId == classId).ToListAsync();
        }

        public async Task<bool> Delete(string id)
        {
            var classId = new Guid(id);
            //delete class
            var itemExist = appContext.Class.FirstOrDefault(p => p.ClassId.Equals(classId));
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
                itemExist.CreatedDate = DateTime.Now;
            }
            // delete time
            var itemTimes = this.appContext.ClassTime.Where(p => !p.IsDeleted && p.ClassId.Equals(classId));
            foreach (var item in itemTimes)
            {
                item.IsDeleted = true;
                item.CreatedDate = DateTime.Now;
            }
            // delete student
            var itemStudents = this.appContext.ClassStudent.Where(p => !p.IsDeleted && p.ClassId.Equals(classId));
            foreach (var item in itemStudents)
            {
                item.IsDeleted = true;
                item.CreatedDate = DateTime.Now;
            }
            await appContext.SaveChangesAsync();
            return true;
        }

        public Task<Class> Get(Guid? index)
        {
            return this.appContext.Class.FirstOrDefaultAsync(p => p.ClassId == index);
        }

        public List<ClassList> GetClassesOfBranches(string branchIds)
        {
            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_ClassesOfBranches")
                               .WithSqlParam("@BranchIds", branchIds)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClassList> GetClasses(string branchIds, string value, Guid? statusId, Guid? supplierId)
        {
            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_Classes")
                               .WithSqlParam("@statusId", (statusId != null ? statusId.ToString() : null))
                               .WithSqlParam("@supplierId", (supplierId != null ? supplierId.ToString() : null))
                               .WithSqlParam("@filterValue", value)
                               .WithSqlParam("@BranchIds", branchIds)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClassList> GetClassOffset(Guid? classId, Guid? studentId)
        {
            var cls = appContext.ClassOffset.Where(p => p.ClassId == classId && p.StudentId == studentId && p.IsDeleted == false);
            return cls.Select(p => new ClassList
            {
                ClassOffsetId = p.ClassOffsetId,
                ClassId = p.ClassId.HasValue ? p.ClassId.Value : Guid.Empty,
                StudentId = p.StudentId,
                ShiftId = p.ShiftId,
                LearnDate = p.LearnDate
            }).ToList();
        }

        public List<ClassList> GetClassEx(Guid? classId, Guid? studentId)
        {
            var cls = appContext.ClassEx.Where(p => p.ClassId == classId && p.StudentId == studentId && p.IsDeleted == false);
            return cls.Select(p => new ClassList
            {
                ClassExId = p.ClassExId,
                ClassId = p.ClassId.HasValue ? p.ClassId.Value : Guid.Empty,
                StudentId = p.StudentId,
                ShiftId = p.ShiftId,
                LearnDate = p.LearnDate
            }).ToList();
        }

        public List<ClassList> GetClassPending(Guid? classId, Guid? studentId)
        {
            var cls = appContext.ClassPending.Where(p => p.ClassId == classId && p.StudentId == studentId && p.IsDeleted == false);
            return cls.Select(p => new ClassList
            {
                ClassPendingId = p.ClassPendingId,
                ClassId = p.ClassId.HasValue ? p.ClassId.Value : Guid.Empty,
                StudentId = p.StudentId,
                FromDate = p.FromDate,
                ToDate = p.ToDate
            }).ToList();
        }

        public List<ClassList> GetClassSummary(string branchIds, string value, Guid? statusId, Guid? supplierId, Guid? classId, string userLogin, int page, int size)
        {
            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_ClassSummary")
                               .WithSqlParam("@statusId", (statusId != null ? statusId.ToString() : null))
                               .WithSqlParam("@supplierId", (supplierId != null ? supplierId.ToString() : null))
                               .WithSqlParam("@filterValue", value)
                               .WithSqlParam("@BranchIds", branchIds)
                               .WithSqlParam("@classId", classId)
                               .WithSqlParam("@userLogin", (userLogin != string.Empty ? userLogin : null))
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                someTypeList = someTypeList.GroupBy(p => p.ClassId).Select(p => new ClassList
                {
                    ClassId = p.Key,
                    ClassCode = p.First().ClassCode,
                    ClassName = p.First().ClassName,
                    LongLearn = p.First().LongLearn,
                    MaxStudent = p.First().MaxStudent,
                    CreatedDate = p.First().CreatedDate,
                    StartDate = p.First().StartDate,
                    EndDate = p.First().EndDate,
                    SupplierId = p.First().SupplierId,
                    SupplierName = p.First().SupplierName,
                    Address = p.First().Address,
                    FullName = p.First().FullName,
                    CountStudent = p.First().CountStudent,
                    MaterialName = p.Select(c => c.MaterialName).Aggregate((i, j) => $"{i}, {j}")
                }).ToList();

                //paging
                this.Total = someTypeList.Count();
                if (size > 0 && page >= 0)
                {
                    someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size).ToList();
                }

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ClassList> GetClassStudent(string branchIds, string value, Guid? statusId, Guid? supplierId, Guid? classId, Guid? studentId)
        {
            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_ClassStudent")
                               .WithSqlParam("@statusId", (statusId != null ? statusId.ToString() : null))
                               .WithSqlParam("@supplierId", (supplierId != null ? supplierId.ToString() : null))
                               .WithSqlParam("@filterValue", value)
                               .WithSqlParam("@BranchIds", branchIds)
                               .WithSqlParam("@classId", classId)
                               .WithSqlParam("@studentId", studentId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ClassExamineList> GetClassExamine(string branchIds, Guid? classId, Guid? studentId, Guid? materialId)
        {
            try
            {
                List<ClassExamineList> someTypeList = new List<ClassExamineList>();
                this.appContext.LoadStoredProc("dbo.sp_ClassExamine")
                               .WithSqlParam("@BranchIds", branchIds)
                               .WithSqlParam("@classId", classId)
                               .WithSqlParam("@studentId", studentId)
                               .WithSqlParam("@materialId", materialId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassExamineList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ClassExamineNoteList> GetClassExamineNote(string branchIds, Guid? classId, Guid? studentId, Guid? examineId)
        {
            try
            {
                List<ClassExamineNoteList> someTypeList = new List<ClassExamineNoteList>();
                this.appContext.LoadStoredProc("dbo.sp_ClassExamineNote")
                               .WithSqlParam("@BranchIds", branchIds)
                               .WithSqlParam("@classId", classId)
                               .WithSqlParam("@studentId", studentId)
                               .WithSqlParam("@examineId", examineId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassExamineNoteList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClassList> GetScheduleStudent(Guid? classId, Guid? studentId, int page, int size)
        {
            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_ScheduleStudent")
                               .WithSqlParam("@classId", classId)
                               .WithSqlParam("@studentId", studentId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                var results = someTypeList.ToList();
                //paused => remove datas learning
                var listPaused = someTypeList.Where(p => p.IsPause == true);
                foreach (var item in listPaused)
                {
                    var itemExists = someTypeList.Where(p => p.LearnDate.Value.Date == item.LearnDate.Value.Date && p.IsPause == false);
                    foreach (var itemE in itemExists)
                        results.Remove(itemE);
                }

                this.Total = results.Count();
                if (size > 0 && page >= 0)
                {
                    results = (from c in results select c).Skip(page * size).Take(size).ToList();
                }

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ClassList> GetStudentMaterialDept(string branchIds, string filterValue, string studentId, int page, int size)
        {
            try
            {
                List<ClassList> someTypeList = new List<ClassList>();
                this.appContext.LoadStoredProc("dbo.sp_Students_MaterialDept")
                               .WithSqlParam("@branchIds", branchIds)
                               .WithSqlParam("@filterValue", filterValue)
                               .WithSqlParam("@studentId", studentId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<ClassList>().ToList();
                               });

                this.Total = someTypeList.Count();
                if (size > 0 && page >= 0)
                {
                    someTypeList = (from c in someTypeList select c).Skip(page * size).Take(size).ToList();
                }

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }
    }
}
