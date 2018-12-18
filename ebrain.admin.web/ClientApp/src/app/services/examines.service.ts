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
import { Examine } from '../models/examine.model';
import 'rxjs/add/operator/map';

import { ExaminesEndpoint } from './examines-endpoint.service';
import { ConfigurationService } from './configuration.service';
import { JwtHelper } from './jwt-helper';
import { Results } from "../models/results.model";
import { ExamineMaterial } from '../models/examinematerial.model';
import { ExamineDocument } from '../models/examinedocument.model';

@Injectable()
export class ExaminesService {

    constructor(private router: Router, private configurations: ConfigurationService, private endpointFactory: ExaminesEndpoint) {
        this.initializeStatus();
    }

    search(filter: string, value: string, page: number, size: number) {
        return this.endpointFactory.search(filter, value, page, size)
            .map((response: Response) => <Results<Examine>>response.json());
    }

    searchExamineMaterial( materialId: string) {
        return this.endpointFactory.searchExamineMaterial(materialId)
            .map((response: Response) => <ExamineMaterial[]>response.json());
    }

    searchExamineDocument( materialId: string, examineId: string) {
        return this.endpointFactory.searchExamineDocument(materialId, examineId)
            .map((response: Response) => <ExamineDocument[]>response.json());
    }

    get(index: string) {
        return this.endpointFactory.get(index)
            .map((response: Response) => <Examine>response.json());
    }

    save(value: Examine) {
        return this.endpointFactory.save(value)
            .map((response: Response) => <Examine>response.json());
    }

    saveExamineMaterial(values: ExamineMaterial[]) {
        return this.endpointFactory.saveExamineMaterial(values)
            .map((response: Response) => <boolean>response.json());
    }

    saveExamineDocument(values: ExamineDocument[]) {
        return this.endpointFactory.saveExamineDocument(values)
            .map((response: Response) => <boolean>response.json());
    }

    delete(id: string) {
        return this.endpointFactory.delete(id);
    }

    private initializeStatus() {

    }
}