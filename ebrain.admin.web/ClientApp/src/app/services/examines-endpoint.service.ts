// ======================================
// Author: Ebrain Team
// Email:  johnpham@ymail.com
// Copyright (c) 2017 supperbrain.visualstudio.com
// 
// ==> Contact Us: supperbrain@outlook.com
// ======================================

import { Injectable, Injector } from '@angular/core';
import { Http, Headers, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';
import { Examine } from '../models/examine.model';


@Injectable()
export class ExaminesEndpoint extends EndpointFactory {

    private readonly _serviceUrl: string = "/api/examine";
    private get serviceUrl() { return this.configurations.baseUrl + this._serviceUrl; }


    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }

    search(filter: string, value: string, page: number, size: number): Observable<Response> {

        let url = this.getUrl('search?filter=' + filter + '&value=' + value + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.search(filter, value, page, size));
            });
    }

    searchExamineMaterial(materialId: string): Observable<Response> {

        let url = this.getUrl('getexaminematerial?materialId=' + materialId + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.searchExamineMaterial(materialId));
            });
    }

    searchExamineDocument(materialId: string, examineId: string): Observable<Response> {

        let url = this.getUrl('getexaminedocument?examineId=' + examineId+ '&materialId=' + materialId 
                + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.searchExamineDocument(materialId, examineId));
            });
    }

    get(index: string): Observable<Response> {

        let url = this.getUrl('get?index=' + index + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.get(index));
            });
    }

    getall(): Observable<Response> {
        let url = this.getUrl('getall');
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getall());
            });
    }

    save(value: any): Observable<Response> {
        let url = this.getUrl('update');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.save(value));
            });
    }

    saveExamineMaterial(value: any): Observable<Response> {
        let url = this.getUrl('updateexaminematerial');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.save(value));
            });
    }

    saveExamineDocument(value: any): Observable<Response> {
        let url = this.getUrl('updateexaminedocument');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.save(value));
            });
    }

    delete(id: string): Observable<Response> {
        let url = this.getUrl('remove');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(id);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.delete(id));
            });
    }

    protected handleError(error, continuation: () => Observable<any>) {

        if (error.status == 401) {

        }

        if (error.url && error.url.toLowerCase().includes(this.serviceUrl.toLowerCase())) {
            return Observable.throw('session expired');
        }
        else {
            return Observable.throw(error || 'server error');
        }
    }

    private getUrl(prefix: string): string {
        return this.serviceUrl + '/' + prefix;
    }
}