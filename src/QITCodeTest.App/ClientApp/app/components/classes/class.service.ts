import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { Class } from './class.model';

@Injectable()
export class ClassService {
    private readonly classesServiceUrl: string;
    private headers = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });

    constructor(
        private readonly http: Http,
        @Inject('WEBAPI_URL') private readonly serviceUrl: string) {
        this.classesServiceUrl = `${this.serviceUrl}/classes`;
    }

    getClasses(): Observable<Array<Class>> {
        return this.http.get(this.classesServiceUrl)
            .map((res: Response) => res.json())
            .catch((error: Response) => {
                return Observable.throw(error.json().data || 'Internal server error');
            });
    }

    createClass(entity: Class): Observable<boolean> {
        return this.http.post(this.classesServiceUrl, entity, this.headers)
            .map((res: Response) => res.ok)
            .catch((error: Response) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }

    editClass(entity: Class): Observable<boolean> {
        return this.http.put(`${this.classesServiceUrl}/${entity.id}`, entity, this.headers)
            .map((res: Response) => res.ok)
            .catch((error: Response) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }

    removeClass(entity: Class): Observable<boolean> {
        return this.http.delete(`${this.classesServiceUrl}/${entity.id}`, this.headers)
            .map((res: Response) => res.ok)
            .catch((error: Response) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }
}
