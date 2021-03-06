import { Component, Input, ViewChild } from '@angular/core';

import { Class } from '../class.model';
import { ModalComponent } from '../../shared/form-modal/form-modal.component';
import { ClassService } from '../class.service';
import { EmitterService } from '../../../services/emitter.service';

@Component({
    selector: 'app-class-edit',
    templateUrl: './class-edit.component.html'
})
export class ClassEditComponent {
    @Input() private classListId: string;
    @Input() private model: Class;
    @Input() private formTitle: string;
    @ViewChild(ModalComponent) private modalDialog: ModalComponent;

    private errorMessage: string;

    constructor(private readonly classService: ClassService) { }

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
            actionMethod = this.classService.createClass(this.model);
        } else {
            actionMethod = this.classService.editClass(this.model);
        }

        actionMethod.subscribe(() => {
            this.closeForm();
            EmitterService.get(this.classListId).emit();
        }, error => {
            this.errorMessage = error;
            console.log(error);
        });
    }
}
