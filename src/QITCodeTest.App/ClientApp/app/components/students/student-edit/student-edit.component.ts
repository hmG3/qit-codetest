import { Component, Input, ViewChild } from '@angular/core';

import { Student } from '../student.model';
import { EmitterService } from '../../../services/emitter.service';
import { StudentService } from '../student.service';
import { ModalComponent } from '../../shared/form-modal.component';

@Component({
    selector: 'app-student-edit',
    templateUrl: './student-edit.component.html'
})
export class StudentEditComponent {
    @Input() private studentListId: string;
    @Input() private model: Student;
    @Input() private formTitle: string;
    @ViewChild(ModalComponent) private modalDialog: ModalComponent;
    private errorMessage: string;

    constructor(private readonly studentService: StudentService) { }

    openForm() {
        this.modalDialog.show();
    }

    closeForm() {
        this.errorMessage = '';
        this.modalDialog.hide();
    }

    save() {
        let actionMethod = null;
        if (!this.model.id) {
            actionMethod = this.studentService.createStudent(this.model);
        } else {
            actionMethod = this.studentService.editStudent(this.model);
        }

        actionMethod.subscribe(() => {
            this.closeForm();
            EmitterService.get(this.studentListId).emit();
        }, error => {
            this.errorMessage = error;
            console.log(error);
        });
    }
}
