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
import { GrpMaterial } from '../models/grpMaterial.model';


@Injectable()
export class IOStudentListEndpoint extends EndpointFactory {

    private readonly _serviceUrl: string = "/api/iostudent";
    private readonly _serviceIOUrl: string = "/api/iostock";
    private get serviceUrl() { return this.configurations.baseUrl + this._serviceUrl; }
    private get serviceIOUrl() { return this.configurations.baseUrl + this._serviceIOUrl; }

    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }

    search(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrl('search?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.search(filter, value, fromDate, toDate, page, size));
            });
    }

    reportsearch(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrl('reportsearch?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.search(filter, value, fromDate, toDate, page, size));
            });
    }

    getiobyiotypeid(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('getiobyiotypeid?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getiobyiotypeid(filter, value, fromDate, toDate, page, size));
            });
    }

    getIOStockDetailDept(filter: string, value: string, studentId: string, ioStockId: string,
        dept: number, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('getiostockdetaildept?filter=' + filter + '&value=' + value + '&studentId=' + studentId
            + '&ioStockId=' + ioStockId + '&dept=' + dept
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getIOStockDetailDept(filter, value, studentId, ioStockId, dept, page, size));
            });
    }

    onOutputCSV_IOStockDetailDept(filter: string, value: string, studentId: string, ioStockId: string,
        dept: number, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('csviostockdetaildept?filter=' + filter + '&value=' + value + '&studentId=' + studentId
            + '&ioStockId=' + ioStockId + '&dept=' + dept
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getIOStockDetailDept(filter, value, studentId, ioStockId, dept, page, size));
            });
    }

    saveDept(value: any): Observable<Response> {
        let url = this.getUrlIO('updatioedept');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.saveDept(value));
            });
    }

    reportiobyiotypeid(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('reportiobyiotypeid?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.reportiobyiotypeid(filter, value, fromDate, toDate, page, size));
            });
    }

    getiodetailbyiotypeid(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('getiodetailbyiotypeid?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getiodetailbyiotypeid(filter, value, fromDate, toDate, page, size));
            });
    }

    getIOStockDetailPro(iostockDetailId: string, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('getiostockdetailprolist?iostockDetailId=' + iostockDetailId
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getIOStockDetailPro(iostockDetailId, page, size));
            });
    }

    getPromotionIOStockDetail(ioStockDetailId: string): Observable<Response> {

        let url = this.getUrlIO('getpromotioniostockdetail?ioStockDetailId=' + ioStockDetailId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getPromotionIOStockDetail(ioStockDetailId));
            });
    }

    getPromotionMaterial(materialId: string, isForever: number, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('getpromotionmaterial?materialId=' + materialId + '&isForever=' + isForever
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getPromotionMaterial(materialId, isForever, page, size));
            });
    }

    reportiodetailbyiotypeid(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('reportiodetailbyiotypeid?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.reportiodetailbyiotypeid(filter, value, fromDate, toDate, page, size));
            });
    }

    getWarehouseCard(filter: string, value: string, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = this.getUrlIO('getwarehousecard?filter=' + filter + '&value=' + value + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getWarehouseCard(filter, value, fromDate, toDate, page, size));
            });
    }

    getIONew(): Observable<Response> {

        let url = this.getUrlIO('getionew?hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getIONew());
            });
    }

    getIOAll(): Observable<Response> {

        let url = this.getUrlIO('getioall?hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getIOAll());
            });
    }

    getiopayment(filter: string, value: string, isInput: number, isGetAll: number, isWaitingClass: number, ioid: string,
        fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = "";
        url = this.getUrlIO('getiopayment?filter=' + filter + '&value=' + value + '&getAll=' + isGetAll
            + '&isInput=' + isInput
            + '&isWaitingClass=' + isWaitingClass
            + '&ioid=' + ioid + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getiopayment(filter, value, isInput, isGetAll, isWaitingClass, ioid, fromDate, toDate, page, size));
            });
    }

    outputCSV(filter: string, value: string, isInput: number, isGetAll: number, isWaitingClass: number, ioid: string,
        fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = "";
        url = this.getUrlIO('csviopayment?filter=' + filter + '&value=' + value + '&getAll=' + isGetAll
            + '&isInput=' + isInput
            + '&isWaitingClass=' + isWaitingClass
            + '&ioid=' + ioid + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getiopayment(filter, value, isInput, isGetAll, isWaitingClass, ioid, fromDate, toDate, page, size));
            });
    }

    getiopaymentDetail(filter: string, value: string, isGetAll: number, isWaitingClass: number, ioid: string, isInput: boolean,
        isLearning: number, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = "";
        url = this.getUrlIO('getiopaymentdetail?filter=' + filter + '&value=' + value + '&getAll=' + isGetAll
            + '&isWaitingClass=' + isWaitingClass
            + '&isLearning=' + isLearning
            + '&ioid=' + ioid + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getiopaymentDetail(filter, value, isGetAll, isWaitingClass, ioid, isInput, isLearning, fromDate, toDate, page, size));
            });
    }

    outputCSVDetail(filter: string, value: string, isGetAll: number, isWaitingClass: number, ioid: string, isInput: boolean,
        isLearning: number, fromDate: Date, toDate: Date, page: number, size: number): Observable<Response> {

        let url = "";
        url = this.getUrlIO('csviopaymentdetail?filter=' + filter + '&value=' + value + '&getAll=' + isGetAll
            + '&isWaitingClass=' + isWaitingClass
            + '&isLearning=' + isLearning
            + '&ioid=' + ioid + '&fromDate=' + fromDate + '&toDate=' + toDate
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.outputCSVDetail(filter, value, isGetAll, isWaitingClass, ioid, isInput, isLearning, fromDate, toDate, page, size));
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
    private getUrlIO(prefix: string): string {
        return this.serviceIOUrl + '/' + prefix;
    }
}