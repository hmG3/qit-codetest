import { Component, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'form-modal',
    templateUrl: './form-modal.component.html',
    styleUrls: ['./form-modal.component.css']
})
export class ModalComponent {
    @Output() closeEmitter = new EventEmitter<ModalResult>();
    @Output() positiveLabelAction = new EventEmitter();

    private showModal: boolean;

    show() {
        this.showModal = true;
    }

    hide() {
        this.showModal = false;
        this.closeEmitter.next({
            action: ModalAction.POSITIVE
        });
    }

    positiveAction() {
        this.positiveLabelAction.next(this);
        return false;
    }

    cancelAction() {
        this.hide();
        return false;
    }
}

export enum ModalAction { POSITIVE, CANCEL }

export interface ModalResult {
    action: ModalAction;
}
