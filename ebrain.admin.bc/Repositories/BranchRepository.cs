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
using ebrain.admin.bc.Report;
using ebrain.admin.bc.Utilities;

namespace ebrain.admin.bc.Repositories
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public int Total { get; private set; }

        public BranchRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Branch> GetBranchOfUser(Guid userId)
        {
            var user = this.appContext.Users.FirstOrDefault(p => p.Id == userId.ToString());
            if (user != null)
            {
                var branchId = user.BranchId.HasValue ? user.BranchId.Value : Guid.Empty;
                return await this.appContext.Branch.FirstOrDefaultAsync(p => p.BranchId == branchId);
            }
            return null;
        }

        public IEnumerable<BranchUser> GetAllBranchOfUser(Guid userId)
        {
            try
            {
                List<BranchUser> someTypeList = new List<BranchUser>();
                this.appContext.LoadStoredProc("dbo.sp_BranchCurrentOfUser")
                               .WithSqlParam("userId", userId).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<BranchUser>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BranchList> GetBranchHead(string branchId)
        {
            try
            {
                List<BranchList> someTypeList = new List<BranchList>();
                this.appContext.LoadStoredProc("dbo.sp_BranchOfBranchHead")
                               .WithSqlParam("@branchId", branchId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<BranchList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BranchList> GetMaterialBranchHead(string materialId)
        {
            try
            {
                List<BranchList> someTypeList = new List<BranchList>();
                this.appContext.LoadStoredProc("dbo.sp_MaterialOfBranchClone")
                               .WithSqlParam("@materialId", materialId)
                               .ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<BranchList>().ToList();
                               });

                return someTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetAllBranchOfUserString(Guid userId)
        {
            return GetAllBranchOfUser(userId).Select(p => p.BranchId).ToArray().ConvertArrayGuidToString();
        }

        public async Task<IEnumerable<Branch>> GetAll()
        {
            return this.appContext.Branch.Where(p => p.IsDeleted == false);
        }

        public async Task<IEnumerable<Branch>> Search(string filter, string value, int page, int size)
        {
            var list = from c in this.appContext.Branch
                       where (string.IsNullOrEmpty(value) || value.Contains(c.BranchName) || value.Contains(c.BranchCode)) && !c.IsDeleted
                       select c;

            //
            this.Total = list.Count();

            if (size > 0 && page >= 0)
            {
                list = (from c in list
                        orderby c.CreatedDate descending
                        select c).Skip(page * size).Take(size);
            }

            return await list.ToListAsync();
        }

        public async Task<Branch> Get(Guid? index)
        {
            return await this.appContext.Branch.FirstOrDefaultAsync(p => p.BranchId == index);
        }

        public async Task<BranchHead> GetBranchHead(Guid? index)
        {
            return await this.appContext.BranchHead.FirstOrDefaultAsync(p => p.BranchId == index);
        }

        public async Task<BranchSMS> GetBranchSMS(Guid? index)
        {
            return await this.appContext.BranchSMS.FirstOrDefaultAsync(p => p.BranchId == index);
        }

        public async Task<BranchZalo> GetBranchZalo(Guid? index)
        {
            return await this.appContext.BranchZalo.FirstOrDefaultAsync(p => p.BranchId == index);
        }

        public async Task<Branch> Save(Branch value, Guid? oldId)
        {
            var item = await appContext.Branch.FirstOrDefaultAsync(x => x.BranchId == oldId);

            if (item != null)
            {
                item.Address = value.Address;
                item.BranchCode = value.BranchCode;
                //item.BranchId 
                item.BranchName = value.BranchName;
                item.Email = value.Email;
                item.FAX = value.FAX;
                item.IsHQ = value.IsHQ;
                item.LogoName = value.LogoName;
                item.Note = value.Note;
                item.PhoneNumber = value.PhoneNumber;
                item.UpdatedBy = value.UpdatedBy;
                item.UpdatedDate = value.UpdatedDate;
            }
            else
            {
                var result = await appContext.Branch.AddAsync(value);
                item = result.Entity;
            }

            //Config SMS 
            var sms = await appContext.BranchSMS.FirstOrDefaultAsync(p => p.BranchId == oldId);
            if (sms != null)
            {
                sms.BranchId = item.BranchId;
                sms.UserName = value.UserName;
                sms.Password = value.Password;
                sms.CPCode = value.CPCode;
                sms.RequestID = value.RequestID;
                sms.ServiceId = value.ServiceId;
                sms.CommandCode = value.CommandCode;
                sms.ContentType = value.ContentType;
                sms.UpdatedBy = value.UpdatedBy;
                sms.UpdatedDate = value.UpdatedDate;
            }
            else
            {
                sms = new BranchSMS
                {
                    BranchId = item.BranchId,
                    UserName = value.UserName,
                    Password = value.Password,
                    CPCode = value.CPCode,
                    RequestID = value.RequestID,
                    ServiceId = value.ServiceId,
                    CommandCode = value.CommandCode,
                    ContentType = value.ContentType,
                    UpdatedBy = value.UpdatedBy,
                    UpdatedDate = value.UpdatedDate,
                    CreatedBy = value.UpdatedBy,
                    CreatedDate = value.UpdatedDate
                };
                await appContext.BranchSMS.AddAsync(sms);
            }

            //config zalo
            var zalo = await appContext.BranchZalo.FirstOrDefaultAsync(p => p.BranchId == oldId);
            var itemZalo = value.BranchZalo;
            if (zalo != null)
            {

                zalo.BranchId = item.BranchId;
                zalo.UserName = itemZalo.UserName;
                zalo.Password = itemZalo.Password;
                zalo.Code = itemZalo.Code;
                zalo.Secret = itemZalo.Secret;
                zalo.AppId = itemZalo.AppId;
                zalo.CallBackUrl = itemZalo.CallBackUrl;
                zalo.HomeUrl = itemZalo.HomeUrl;
                zalo.UpdatedBy = itemZalo.UpdatedBy;
                zalo.UpdatedDate = itemZalo.UpdatedDate;
            }
            else
            {
                zalo = new BranchZalo
                {
                    BranchId = item.BranchId,
                    UserName = itemZalo.UserName,
                    Password = itemZalo.Password,
                    Code = itemZalo.Code,
                    Secret = itemZalo.Secret,
                    AppId = itemZalo.AppId,
                    CallBackUrl = itemZalo.CallBackUrl,
                    HomeUrl = itemZalo.HomeUrl,
                    UpdatedBy = itemZalo.UpdatedBy,
                    UpdatedDate = itemZalo.UpdatedDate,
                    CreatedBy = itemZalo.UpdatedBy,
                    CreatedDate = itemZalo.UpdatedDate
                };
                await appContext.BranchZalo.AddAsync(zalo);
            }
            //
            await appContext.SaveChangesAsync();
            //
            return item;
        }

        public async Task<Branch> SaveHead(Branch[] values, Guid? branchParentId, Guid userId)
        {
            var item = await Get(branchParentId);
            if (item != null)
            {
                foreach (var itemHead in values)
                {
                    var itemExistD = this.appContext.BranchHead.FirstOrDefault(
                            p => p.BranchParentId == branchParentId
                            && p.BranchId == itemHead.BranchId);

                    if (itemExistD != null)
                    {
                        itemExistD.IsDeleted = !itemHead.IsExist;
                    }
                    else
                    {
                        itemExistD = new BranchHead
                        {
                            BranchHeadId = Guid.NewGuid(),
                            BranchId = itemHead.BranchId,
                            BranchParentId = branchParentId,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now,
                            IsDeleted = !itemHead.IsExist
                        };
                        await appContext.BranchHead.AddAsync(itemExistD);
                    }

                    await appContext.SaveChangesAsync();
                }
            }
            return item;
        }

        public async Task<bool> Delete(Guid id)
        {
            var itemExist = appContext.Branch.FirstOrDefault(p => p.BranchId == id);

            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            return await appContext.SaveChangesAsync() > 0;
        }

        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }
    }
}
