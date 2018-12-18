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
import { Class } from '../models/class.model';


@Injectable()
export class ClassesEndpoint extends EndpointFactory {

    private readonly _serviceUrl: string = "/api/classes";
    private get serviceUrl() { return this.configurations.baseUrl + this._serviceUrl; }


    constructor(http: Http, configurations: ConfigurationService, injector: Injector) {
        super(http, configurations, injector);
    }

    search(filter: string, value: string, isUsageTeacher: number): Observable<Response> {

        let url = this.getUrl('search?filter=' + filter + '&value=' + value + '&isUsageTeacher=' + isUsageTeacher + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.search(filter, value, isUsageTeacher));
            });
    }

    getClassCurrent(studentId: string): Observable<Response> {

        let url = this.getUrl('getclasscurrent?studentId=' + studentId + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassCurrent(studentId));
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

    saveStudent(value: any): Observable<Response> {
        let url = this.getUrl('updatestudent');
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

    saveExamine(value: any): Observable<Response> {
        let url = this.getUrl('updateclassexamine');
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

    saveExamineNote(value: any): Observable<Response> {
        let url = this.getUrl('updateclassexaminenote');
        let header = this.getAuthHeader(true);
        let params = JSON.stringify(value);

        return this.http.post(url, params, header)
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.saveExamineNote(value));
            });
    }

    saveOffset(value: any): Observable<Response> {
        let url = this.getUrl('updateclassoffset');
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

    savePending(value: any): Observable<Response> {
        let url = this.getUrl('updateclasspending');
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


    saveEx(value: any): Observable<Response> {
        let url = this.getUrl('updateclassex');
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

    getFirstClass(index: string): Observable<Response> {
        let url = this.getUrl('getfirstclass?index=' + index + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.get(index));
            });
    }

    getDefault(index: string): Observable<Response> {
        let url = this.getUrl('getdefault?index=' + index + '&hash_id=' + Math.random());

        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.get(index));
            });
    }

    getClassPending(studentId: string, classId: string): Observable<Response> {

        let url = this.getUrl('getclasspending?studentId=' + studentId + '&classId=' + classId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassPending(studentId, classId));
            });
    }

    getClassOffset(studentId: string, classId: string): Observable<Response> {

        let url = this.getUrl('getclassoffset?studentId=' + studentId + '&classId=' + classId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassOffset(studentId, classId));
            });
    }

    getClassEx(studentId: string, classId: string): Observable<Response> {

        let url = this.getUrl('getclassex?studentId=' + studentId + '&classId=' + classId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassEx(studentId, classId));
            });
    }

    getClassesofbranches(branchIds: string): Observable<Response> {

        let url = this.getUrl('getclassesofbranches?branchIds=' + branchIds + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassesofbranches(branchIds));
            });
    }

    getClasses(filter: string, value: string, statusId: string, supplierId: string): Observable<Response> {

        let url = this.getUrl('getclasses?filter=' + filter + '&value=' + value + '&statusId=' + statusId + '&supplierId=' + supplierId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClasses(filter, value, statusId, supplierId));
            });
    }

    outputCSV(filter: string, value: string, statusId: string, supplierId: string, classId: string,
        isUsageTeacher: number, page: number, size: number): Observable<Response> {

        let url = this.getUrl('csv?filter=' + filter + '&value=' + value + '&statusId=' + statusId + '&supplierId=' + supplierId
            + '&classId=' + classId
            + '&isUsageTeacher=' + isUsageTeacher
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getsummaries(filter, value, statusId, supplierId, classId, isUsageTeacher, page, size));
            });
    }

    getsummaries(filter: string, value: string, statusId: string, supplierId: string, classId: string,
        isUsageTeacher: number, page: number, size: number): Observable<Response> {

        let url = this.getUrl('getsummaries?filter=' + filter + '&value=' + value + '&statusId=' + statusId + '&supplierId=' + supplierId
            + '&classId=' + classId
            + '&isUsageTeacher=' + isUsageTeacher
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getsummaries(filter, value, statusId, supplierId, classId, isUsageTeacher, page, size));
            });
    }

    getClassEndDate(classId: string, materialId: string, fromDate: string): Observable<Response> {

        let url = this.getUrl('getclassenddate?classId=' + classId + '&materialId=' + materialId + '&fromDate=' + fromDate + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassEndDate(classId, materialId, fromDate));
            });
    }

    getScheduleStudent(classId: string, studentId: string, page: number, size: number): Observable<Response> {

        let url = this.getUrl('getschedulestudent?classId=' + classId + '&studentId=' + studentId
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getScheduleStudent(classId, studentId, page, size));
            });
    }

    getClassByStudentId(filter: string, value: string, statusId: string, supplierId: string, classId: string, studentId: string): Observable<Response> {

        let url = this.getUrl('getclassbystudentid?filter=' + filter + '&value=' + value + '&statusId=' + statusId + '&supplierId=' + supplierId + '&classId=' + classId
            + '&studentId=' + studentId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassByStudentId(filter, value, statusId, supplierId, classId, studentId));
            });
    }

    getClassExamines(classId: string, studentId: string, materialId: string): Observable<Response> {

        let url = this.getUrl('getclassexamine?classId=' + classId 
                    + '&studentId=' + studentId 
                    + '&materialId=' + materialId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassExamines(classId, studentId, materialId));
            });
    }
    getClassExaminesNote(classId: string, studentId: string, examineId: string): Observable<Response> {

        let url = this.getUrl('getclassexaminenote?classId=' + classId 
        + '&studentId=' + studentId + '&examineId=' + examineId + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getClassExaminesNote(classId, studentId, examineId));
            });
    }

    

    getStudentMaterialDept(filterValue: string, page: number, size: number): Observable<Response> {
        let url = this.getUrl('getstudentmaterialdept?filterValue=' + filterValue
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.getStudentMaterialDept(filterValue, page, size));
            });
    }

    csvStudentMaterialDept(filterValue: string, page: number, size: number): Observable<Response> {
        let url = this.getUrl('csvstudentmaterialdept?filterValue=' + filterValue
            + '&page=' + page + '&size=' + size + '&hash_id=' + Math.random());
        return this.http.get(url, this.getAuthHeader())
            .map((response: Response) => {
                return response;
            })
            .catch(error => {
                return this.handleError(error, () => this.csvStudentMaterialDept(filterValue, page, size));
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