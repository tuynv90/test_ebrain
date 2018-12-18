﻿// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Branch } from '../models/branch.model';
import { Results } from '../models/results.model';
import 'rxjs/add/operator/map';

import { BranchesEndpoint } from './branches-endpoint.service';
import { ConfigurationService } from './configuration.service';
import { JwtHelper } from './jwt-helper';

@Injectable()
export class BranchesService {

    constructor(private router: Router, private configurations: ConfigurationService, private endpointFactory: BranchesEndpoint) {
        this.initializeStatus();
    }

    getAll() {
        return this.endpointFactory.getAll()
            .map((response: Response) => <Branch[]>response.json());
    }

    search(filter: string, value: string, page: number, size: number) {
        return this.endpointFactory.search(filter, value, page, size)
            .map((response: Response) => <Results<Branch>>response.json());
    }

    get(index: string) {
        return this.endpointFactory.get(index)
            .map((response: Response) => <Branch>response.json());
    }

    save(value: Branch) {
        return this.endpointFactory.save(value)
            .map((response: Response) => <Branch>response.json());
    }

    saveHead(value: Branch[]) {
        return this.endpointFactory.saveHead(value)
            .map((response: Response) => <Branch[]>response.json());
    }

    delete(id: string) {
        return this.endpointFactory.delete(id)
            .map((response: Response) => <Boolean>response.json());
    }

    getBranchesOfUser() {
        return this.endpointFactory.getBranchesOfUser()
            .map((response: Response) => <Branch[]>response.json());
    }

    getBranchHead(index: string) {
        return this.endpointFactory.getBranchHead(index)
            .map((response: Response) => <Branch[]>response.json());
    }

    getMaterialBranch(materialId: string) {
        return this.endpointFactory.getMaterialBranch(materialId)
            .map((response: Response) => <Branch[]>response.json());
    }

    outputCSV(filter: string, value: string, page: number, size: number) {
        return this.endpointFactory.outputCSV(filter, value, page, size)
            .map((response: Response) => <string>response.json());
    }

    private initializeStatus() {
        
    }
}