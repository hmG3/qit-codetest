import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { ClassListComponent } from './components/classes/class-list/class-list.component';
import { ClassEditComponent } from './components/classes/class-edit/class-edit.component';
import { StudentListComponent } from './components/students/student-list/student-list.component';
import { StudentEditComponent } from './components/students/student-edit/student-edit.component';
import { ModalComponent } from './components/shared/form-modal/form-modal.component';
import { ValidationErrorsComponent } from './components/shared/validation-errors/validation-errors.component';

import { StudentService } from './components/students/student.service';
import { ClassService } from './components/classes/class.service';
import { EmitterService } from './services/emitter.service';
import { AgePipe } from './pipes/age.pipe';
import { AgeValidatorDirective } from './directives/age.validator.directive';
import { GPAValidatorDirective } from './directives/gpa.validator.directive';
import { TeacherValidatorDirective } from './directives/teacher.validator.directive';

@NgModule({
    declarations: [
        AppComponent,
        ClassListComponent,
        ClassEditComponent,
        StudentListComponent,
        StudentEditComponent,
        ModalComponent,
        ValidationErrorsComponent,
        AgePipe,
        AgeValidatorDirective,
        GPAValidatorDirective,
        TeacherValidatorDirective
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule
    ],
    providers: [
        { provide: 'WEBAPI_URL', useValue: 'http://localhost:8081/api' },
        ClassService,
        StudentService,
        EmitterService
    ],
})
export class AppModuleShared {
}

