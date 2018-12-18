﻿import { IOStockDetailPro } from "./iostockdetailpro.model";

// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

export class IOStockDetail {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(ioid?: string) {
        this.id = ioid;
        this.pros = [];
    }
    public id: string;
    public purchaseOrderId: string;
    public purchaseOrderDetailId: string;
    public materialName: string;
    public materialCode: string;
    public typeMaterial: string;
    public grpMaterial: string;
    public materialId: string;
    public materialTypeId: string;
    public materialGrpId: string;
    public sellPrice: number;
    public disCountMoney: number;
    public quantity: number;
    public totalPrice: number; 
    public note: string;
    public proDetails: IOStockDetailPro[];
    public pros: any = IOStockDetailPro[0];
}