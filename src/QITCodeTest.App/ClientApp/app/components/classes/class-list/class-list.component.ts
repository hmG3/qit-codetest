import { Component, Input, OnInit, OnChanges, ViewChild } from '@angular/core';

import { Class } from '../class.model';
import { EmitterService } from '../../../services/emitter.service';
import { ClassService } from '../class.service';
import { ClassEditComponent } from '../class-edit/class-edit.component';

@Component({
    selector: 'app-class-list',
    templateUrl: './class-list.component.html',
    styleUrls: ['./class-list.component.css']
})
export class ClassListComponent implements OnInit, OnChanges {
    @Input() private schoolAppId: string;
    @Input() private classListId: string;
    @ViewChild(ClassEditComponent) private editFormComponent: ClassEditComponent;

    private availableClasses: Array<Class>;

    private editActionFormTitle: string;
    private editableClass: Class;
    private selectedRowIndex: number;

    private hasErrors: boolean;

    constructor(private readonly classService: ClassService) {
        this.classListId = 'CLASSES_COMPONENT_LIST_EVENT';
     }

    ngOnInit() {
        this.loadAvailableClasses();
    }

    ngOnChanges(): void {
        EmitterService.get(this.classListId).subscribe(() => this.loadAvailableClasses());
    }

    loadAvailableClasses() {
        this.classService.getClasses().subscribe(classes =>
            this.availableClasses = classes,
            error => {
                this.hasErrors = true;
                console.log(error);
            });
    }

    addClass() {
        this.editActionFormTitle = 'Add new class';
        this.editableClass = new Class();
        this.editFormComponent.openForm();
    }

    editClass(classToEdit: Class) {
        this.editActionFormTitle = `Edit class: ${classToEdit.name}`;
        this.editableClass = Object.assign(new Class(), classToEdit);
        this.editFormComponent.openForm();
    }

    deleteClass(classToDelete: Class) {
        this.classService.removeClass(classToDelete).subscribe(
            () => EmitterService.get(this.classListId).emit(),
            error => {
                this.hasErrors = true;
                console.log(error);
            });
    }

    loadStudents(selectedRowIndex: number, selectedClass: Class) {
        this.selectedRowIndex = selectedRowIndex;
        EmitterService.get(this.schoolAppId).emit(selectedClass.id);
    }
}
