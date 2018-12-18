﻿// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

import { IOStockDetail } from "./iostockdetail.model";

export class IOStock {
    // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(ioid?: string) {
        this.id = ioid;
    }

    public id: string;
    public name: string;
    public code: string;
    public purchaseOrderCode: string;
    public purchaseOrderId: string;
    public note: string;
    public studentId: string;
    public createDate: Date;
    public createBy: string;
    public html: string;
    public totalPrice: number;
    public Quantity: number;
    public SellPrice: number;
    public ioTypeId: number;
    public ioDetails: IOStockDetail[];
}