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
import { Student } from '../models/student.model';


@Injectable()
export class IOStudentsEndpoint extends EndpointFactory {

    private readonly _serviceUrl: string = "/api/iostudent";
    private readonly _usersUrl: string = "/api/account/users";

    private get serviceUrl() { return this.configurations.baseUrl + this._serviceUrl; }
    private get usersUrl() { return this.configurations.baseUrl + this._usersUrl; }

    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }

    search(filter: string, value: string): Observable<Response> {

        let url = this.getUrl('search?filter=' + filter + '&value=' + value);

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.search(filter, value));
            });
    }

    getAll(page?: number, pageSize?: number): Observable<Response> {

        let url = this.getUrl('getall?page=' + page + '&pageSize=' + pageSize);

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getAll(page, pageSize));
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


    deletemaster(value: any): Observable<Response> {
        let url = this.getUrl('deletemaster');
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

    cancel(value: any): Observable<Response> {
        let url = this.getUrl('cancel');
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

    getdefault(index, isinput, isClone: number = 0): Observable<Response> {

        let url = "";
        if (isinput == false) {
            url = this.getUrl('getdefault?index=' + index + '&isClone=' + isClone + '&hash_id=' + Math.random());
        } else {
            url = this.getUrl('getdefaultinput?index=' + index + '&isClone=' + isClone + '&hash_id=' + Math.random());
        }

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getdefault(index, isinput));
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