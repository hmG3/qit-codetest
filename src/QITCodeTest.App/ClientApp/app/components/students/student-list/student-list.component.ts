import { Component, Input, ViewChild, OnChanges } from '@angular/core';

import { EmitterService } from '../../../services/emitter.service';
import { StudentService } from '../student.service';
import { Student } from '../student.model';
import { StudentEditComponent } from '../student-edit/student-edit.component';

@Component({
    selector: 'app-student-list',
    templateUrl: './student-list.component.html'
})
export class StudentListComponent implements OnChanges {
    @Input() private studentListId: string;
    @Input() private schoolAppId: string;
    @ViewChild(StudentEditComponent) private editFormComponent: StudentEditComponent;
    private availableStudents: Array<Student>;
    private editActionFormTitle: string;
    private editableStudent: Student;
    private selectedClassId: string;
    private hasErrors: boolean;

    constructor(private readonly studentService: StudentService) {
        this.studentListId = 'STUDENTS_COMPONENT_LIST_EVENT';
     }

    ngOnChanges(): void {
        EmitterService.get(this.schoolAppId).subscribe((id: string) => {
            this.selectedClassId = id;
            this.loadAvailableStudents(this.selectedClassId);
        });
        EmitterService.get(this.studentListId).subscribe(() => this.loadAvailableStudents(this.selectedClassId));
    }

    loadAvailableStudents(classId: string) {
        this.studentService.getStudentsOfClass(classId).subscribe(students =>
            this.availableStudents = students,
            error => {
                this.hasErrors = true;
                console.log(error);
            });
    }

    addStudent() {
        this.editActionFormTitle = 'Add new student';
        this.editableStudent = new Student();
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
                this.hasErrors = true;
                console.log(error);
            });
    }
}
