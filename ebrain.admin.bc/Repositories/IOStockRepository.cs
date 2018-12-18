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
    public class IOStockRepository : Repository<IOStock>, IIOStockRepository
    {
        public int Total { get; private set; }
        public IOStockRepository(ApplicationDbContext context) : base(context)
        { }


        public IEnumerable<IOStock> GetTopActive(int count)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<IOStockDetail>> GetDetailByIOId(Guid? id)
        {
            return await this.appContext.IOStockDetail.Where(p => p.IsDeleted == false && p.IOStockId == id).ToListAsync();
        }
        public async Task<IOStock> FindById(Guid? id)
        {
            return await this.appContext.IOStock.FirstOrDefaultAsync(p => p.IsDeleted == false && p.IsCancel == false && p.IOStockId == id);
        }

        public async Task<IEnumerable<IOStock>> GetAlls(string branchIds)
        {
            return await this.appContext.IOStock.Where(p => p.IsDeleted == false && p.IsCancel == false &&
                    branchIds.Contains(p.BranchId.ToString())
                ).ToListAsync();
        }

        public async Task<IEnumerable<IOStock>> Search(string filter, string value, string branchIds)
        {
            return await this.appContext.IOStock.Where(p => p.IsDeleted == false && p.IsCancel == false &&
                    branchIds.Contains(p.BranchId.ToString())
                ).ToListAsync();
        }
        public IEnumerable<IOStockDetailList> GetIOStockDetailList
            (DateTime fromDate, DateTime toDate, string filterValue, int ioTypeId, string branchIds, int page, int size)
        {
            try
            {
                List<IOStockDetailList> someTypeList = new List<IOStockDetailList>();
                this.appContext.LoadStoredProc("dbo.sp_IOStockListDetail")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("ioTypeId", ioTypeId)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockDetailList>().ToList();
                               });

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

        public IEnumerable<IOStockDetailList> GetIOStockDetailListDept
            (Guid? studentId, Guid? ioStockId, string filterValue, bool isGetDept, string branchIds, int page, int size)
        {
            try
            {
                List<IOStockDetailList> someTypeList = new List<IOStockDetailList>();
                this.appContext.LoadStoredProc("dbo.sp_IOStockListDetailDept")
                               .WithSqlParam("@studentId", studentId)
                               .WithSqlParam("@ioStockId", ioStockId)
                               .WithSqlParam("@isGetDept", isGetDept)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockDetailList>().ToList();
                               });

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

        public IEnumerable<IOStockDetailPro> GetIOStockDetailProList(Guid? ioStockDetailId, int page, int size)
        {
            try
            {
                List<IOStockDetailPro> someTypeList = new List<IOStockDetailPro>();
                this.appContext.LoadStoredProc("dbo.sp_IOStockDetailPromotionList")
                               .WithSqlParam("@IOStockDetailId", ioStockDetailId).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockDetailPro>().ToList();
                               });

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

        public IEnumerable<PromotionsDetail> GetPromotionMaterials(Guid? materialId, bool? isForever, string branchIds, int page, int size)
        {
            try
            {
                List<PromotionsDetail> someTypeList = new List<PromotionsDetail>();
                this.appContext.LoadStoredProc("dbo.sp_Promotion_Materials")
                               .WithSqlParam("@materialId", materialId)
                               .WithSqlParam("@isForever", isForever)
                               .WithSqlParam("@branchIds", branchIds).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionsDetail>().ToList();
                               });

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

        public IEnumerable<PromotionsDetail> GetPromotionIOStockDetail(Guid? ioStockDetailId)
        {
            try
            {
                List<PromotionsDetail> someTypeList = new List<PromotionsDetail>();
                this.appContext.LoadStoredProc("dbo.sp_Promotion_IOStockDetail")
                               .WithSqlParam("@iostockDetailId", ioStockDetailId).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<PromotionsDetail>().ToList();
                               });

                //paging
                this.Total = someTypeList.Count();
                return someTypeList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<IOStockDetailList> GetWarehouseCard(
            DateTime fromDate, DateTime toDate, string filterValue, int ioTypeId, string branchIds, int page, int size)
        {
            try
            {
                List<IOStockDetailList> someTypeList = new List<IOStockDetailList>();
                this.appContext.LoadStoredProc("dbo.sp_WarehouseCard")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("ioTypeId", ioTypeId)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockDetailList>().ToList();
                               });

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

        public IEnumerable<IOStockList> GetIOStockList(DateTime fromDate, DateTime toDate, string filterValue, int ioTypeId, string branchIds, int page, int size)
        {
            try
            {
                List<IOStockList> someTypeList = new List<IOStockList>();
                this.appContext.LoadStoredProc("dbo.sp_IOStockList")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("ioTypeId", ioTypeId)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("filterValue", filterValue).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockList>().ToList();
                               });

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

        public IEnumerable<IOStockListPayment> GetIOStockPaymentList
            (DateTime fromDate, DateTime toDate, string filterValue, string ioId, int ioTypeId, bool isInput, bool isWaitingClass, string branchIds, int page, int size)
        {
            try
            {
                List<IOStockListPayment> someTypeList = new List<IOStockListPayment>();
                this.appContext.LoadStoredProc("dbo.sp_IOStockList_Payment")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("ioTypeId", ioTypeId)
                               .WithSqlParam("filterValue", filterValue)
                               .WithSqlParam("ioId", ioId)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("isWaitingClass", isWaitingClass)
                               .WithSqlParam("isInput", isInput).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockListPayment>().ToList();
                               });

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

        public IEnumerable<IOStockListPayment> GetIOStockPaymentListDetail
            (DateTime fromDate, DateTime toDate, string filterValue, string ioId, int ioTypeId, bool isInput,
            bool isWaitingClass, bool isLearning, string branchIds, int page, int size)
        {
            try
            {
                List<IOStockListPayment> someTypeList = new List<IOStockListPayment>();
                this.appContext.LoadStoredProc("dbo.sp_IOStockList_PaymentDetail")
                               .WithSqlParam("fromDate", fromDate)
                               .WithSqlParam("toDate", toDate)
                               .WithSqlParam("ioTypeId", ioTypeId)
                               .WithSqlParam("filterValue", filterValue)
                               .WithSqlParam("ioId", ioId)
                               .WithSqlParam("branchIds", branchIds)
                               .WithSqlParam("isWaitingClass", isWaitingClass)
                               .WithSqlParam("isLearning", isLearning)
                               .WithSqlParam("isInput", isInput).ExecuteStoredProc((handler) =>
                               {
                                   someTypeList = handler.ReadToList<IOStockListPayment>().ToList();
                               });

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

        public async Task<bool> SaveDept(IOStockDetail[] iosd)
        {
            foreach (var item in iosd)
            {
                var itemExistD = this.appContext.IOStockDetail.FirstOrDefault(p => p.IOStockDetailId == item.IOStockDetailId);
                if (itemExistD != null)
                {
                    itemExistD.InputExport = item.InputExport;
                    itemExistD.ByExport = item.UpdatedBy;
                    itemExistD.DateExport = item.UpdatedDate;
                }
            }
            return appContext.SaveChanges() > 0;
        }
        public async Task<IOStock> Save(IOStock value, IOStockDetail[] iosd, Guid? index)
        {
            try
            {
                //get branchId
                value.BranchId = value.CreatedBy.GetBranchOfCurrentUser(this.appContext);
                var item = this.appContext.IOStock.FirstOrDefault(p => p.IOStockId == index);
                if (item != null)
                {
                    item.Note = value.Note;
                    item.StudentId = value.StudentId;
                    item.CreatedDate = value.CreatedDate;
                    item.TotalPrice = value.TotalPrice;
                    item.TotalPriceBeforeVAT = value.TotalPriceBeforeVAT;

                    var iodExists = await GetDetailByIOId(item.IOStockId);

                    //update deleted
                    var iodIds = iosd.Select(p => p.IOStockDetailId);
                    var iodNotExists = iodExists.Where(p => !iodIds.Contains(p.IOStockDetailId));
                    foreach (var itemDetail in iodNotExists)
                    {
                        itemDetail.IsDeleted = true;
                    }
                    //Insert
                    foreach (var itemDetail in iosd)
                    {
                        var itemExistD = this.appContext.IOStockDetail.FirstOrDefault(p => p.IOStockDetailId == itemDetail.IOStockDetailId);
                        if (itemExistD != null)
                        {
                            itemExistD.InputQuantity = itemDetail.InputQuantity;
                            itemExistD.PriceBeforeVAT = itemDetail.PriceBeforeVAT;
                            itemExistD.PriceAfterVAT = itemDetail.PriceAfterVAT;
                            itemExistD.TotalPrice = itemDetail.TotalPrice;
                            itemExistD.DisCountMoney = itemDetail.DisCountMoney;
                            itemExistD.TotalPriceBeforeVAT = itemDetail.TotalPriceBeforeVAT;
                        }
                        else
                        {
                            itemDetail.IOStockId = item.IOStockId;
                            await appContext.IOStockDetail.AddAsync(itemDetail);
                        }


                        // promotions
                        foreach (var itemP in itemDetail.IOPros)
                        {
                            var pro = this.appContext.IOStockDetailPro.FirstOrDefault(
                                            p => p.IOStockDetailId == itemP.IOStockDetailId
                                            && p.PromotionDetailId == itemP.PromotionDetailId
                                            && p.IsDeleted == false);
                            if (pro == null)
                            {
                                pro = new IOStockDetailPro
                                {
                                    IOStockDetailProId = Guid.NewGuid(),
                                    IOStockId = item.IOStockId,
                                    IOStockDetailId = itemDetail.IOStockDetailId,
                                    PromotionId = itemP.PromotionId,
                                    PromotionDetailId = itemP.PromotionDetailId,
                                    PercentDiscount = itemP.PercentDiscount,
                                    MoneyDiscount = itemP.MoneyDiscount,
                                    CreatedBy = item.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = item.UpdatedBy,
                                    UpdatedDate = item.UpdatedDate
                                };

                                await appContext.IOStockDetailPro.AddAsync(pro);
                            }
                        }
                    }
                }
                else
                {
                    var result = await appContext.IOStock.AddAsync(value);
                    foreach (var itemD in iosd)
                    {
                        await appContext.IOStockDetail.AddAsync(itemD);
                        // promotions
                        foreach (var pro in itemD.IOPros)
                        {
                            await appContext.IOStockDetailPro.AddAsync(pro);
                        }
                    }
                    item = result.Entity;
                }
                //
                await appContext.SaveChangesAsync();

                var itemExist = this.appContext.Users.FirstOrDefault(p => p.Id == item.UpdatedBy.ToString());
                item.UpdatedByName = itemExist != null ? itemExist.FullName : string.Empty;
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(string id)
        {
            //Check exist in materials
            var unitExist = this.appContext.Material.FirstOrDefault(p => p.UnitId.Equals(new Guid(id)));
            if (unitExist != null) throw new Exception("Exist in materials");

            var itemExist = appContext.Unit.FirstOrDefault(p => p.UnitId.Equals(new Guid(id)));
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            await appContext.SaveChangesAsync();
            return true;
        }
        public async Task<Boolean> DeleteMaster(Guid? id)
        {
            var itemExist = appContext.IOStock.FirstOrDefault(p => p.IOStockId == id);
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            await appContext.SaveChangesAsync();
            return true;
        }
        public async Task<Boolean> CancelMaster(Guid? id)
        {
            var itemExist = appContext.IOStock.FirstOrDefault(p => p.IOStockId == id);
            if (itemExist != null)
            {
                itemExist.IsDeleted = true;
            }
            await appContext.SaveChangesAsync();
            return true;

        }

        private ApplicationDbContext appContext
        {
            get { return (ApplicationDbContext)_context; }
        }
    }
}
