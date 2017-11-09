import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { Student } from './student.model';

@Injectable()
export class StudentService {
    private readonly studentsServiceUrl: string;
    private headers = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });

    constructor(
        private readonly http: Http,
        @Inject('WEBAPI_URL') private readonly serviceUrl: string) {
        this.studentsServiceUrl = `${this.serviceUrl}/students`;
    }

    getStudentsOfClass(studentClassId: string): Observable<Array<Student>> {
        return this.http.get(`${this.studentsServiceUrl}/class/${studentClassId}`)
            .map((res: Response) => res.json())
            .catch((error: any) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }

    createStudent(entity: Student): Observable<boolean> {
        return this.http.post(this.studentsServiceUrl, entity, this.headers)
            .map((res: Response) => res.json())
            .catch((error: any) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }

    editStudent(entity: Student): Observable<boolean> {
        return this.http.put(`${this.studentsServiceUrl}/${entity.id}`, entity, this.headers)
            .map((res: Response) => res.ok)
            .catch((error: any) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }

    removeStudent(entity: Student): Observable<boolean> {
        return this.http.delete(`${this.studentsServiceUrl}/${entity.id}`, this.headers)
            .map((res: Response) => res.ok)
            .catch((error: any) => {
                return Observable.throw(error.text() || 'Internal server error');
            });
    }
}
