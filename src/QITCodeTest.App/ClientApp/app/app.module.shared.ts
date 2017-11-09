import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { ModalComponent } from './components/shared/form-modal.component';
import { ClassListComponent } from './components/classes/class-list/class-list.component';
import { ClassEditComponent } from './components/classes/class-edit/class-edit.component';
import { StudentListComponent } from './components/students/student-list/student-list.component';
import { StudentEditComponent } from './components/students/student-edit/student-edit.component';

import { EmitterService } from './services/emitter.service';
import { StudentService } from './components/students/student.service';
import { ClassService } from './components/classes/class.service';
import { AgePipe } from './components/students/age.pipe';

@NgModule({
    declarations: [
        AppComponent,
        ModalComponent,
        ClassListComponent,
        ClassEditComponent,
        StudentListComponent,
        StudentEditComponent,
        AgePipe
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule
    ],
    providers: [
        { provide: 'WEBAPI_URL', useValue: 'http://localhost:50005/api' },
        EmitterService,
        ClassService,
        StudentService
    ],
})
export class AppModuleShared {
}

