import { Component, Input, ViewChild, OnChanges } from '@angular/core';

import { Student } from '../student.model';
import { Class } from '../../classes/class.model';
import { StudentEditComponent } from '../student-edit/student-edit.component';
import { StudentService } from '../student.service';
import { EmitterService } from '../../../services/emitter.service';

@Component({
    selector: 'app-student-list',
    templateUrl: './student-list.component.html'
})
export class StudentListComponent implements OnChanges {
    @Input() private schoolAppId: string;
    @Input() private readonly studentListId: string;
    @ViewChild(StudentEditComponent) private editFormComponent: StudentEditComponent;

    private availableStudents: Array<Student>;
    private editActionFormTitle: string;
    private editableStudent: Student;
    private selectedClass: Class;
    private errorMessage: boolean;

    constructor(private readonly studentService: StudentService) {
        this.studentListId = 'STUDENTS_COMPONENT_LIST_EVENT';
    }

    ngOnChanges(): void {
        EmitterService.get(this.schoolAppId).subscribe((c: Class) => {
            this.selectedClass = c;
            this.loadAvailableStudents(this.selectedClass.id);
        });
        EmitterService.get(this.studentListId).subscribe(() => this.loadAvailableStudents(this.selectedClass.id));
    }

    loadAvailableStudents(classId: string) {
        this.studentService.getStudentsOfClass(classId).subscribe(students =>
            this.availableStudents = students,
            error => {
                this.errorMessage = error;
                console.log(error);
            });
    }

    addStudent() {
        this.editActionFormTitle = 'Add new student';
        this.editableStudent = new Student();
        this.editableStudent.classId = this.selectedClass.id;
        this.editFormComponent.openForm();
    }

    editStudent(studentToEdit: Student) {
        this.editActionFormTitle = `Edit student: ${studentToEdit.name}`;
        this.editableStudent = Object.assign(new Student(), studentToEdit);
        this.editFormComponent.openForm();
    }

    deleteStudent(studentToDelete: Student) {
        this.studentService.removeStudent(studentToDelete).subscribe(
            () => EmitterService.get(this.studentListId).emit(),
            error => {
                this.errorMessage = error;
                console.log(error);
            });
    }
}
