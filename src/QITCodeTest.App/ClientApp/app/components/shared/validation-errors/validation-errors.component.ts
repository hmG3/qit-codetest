import { Component, Input } from '@angular/core';
import { AbstractControlDirective, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
    selector: 'validation-errors',
    templateUrl: './validation-errors.component.html'
})
export class ValidationErrorsComponent {
    private static readonly errorMessages: ValidationMessages = {
        'required': () => 'This field is required',
        'age': (params: ValidationErrors) => params.message,
        'gpa': (params: ValidationErrors) => params.message,
        'teacher': (params: ValidationErrors) => params.message
    };

    @Input() private control: AbstractControlDirective | AbstractControl;

    shouldShowErrors(): boolean {
        return this.control &&
            <ValidationErrors>this.control.errors &&
            (<boolean>this.control.dirty || <boolean>this.control.touched);
    }

    listOfErrors(): string[] {
        return Object.keys(<ValidationErrors>this.control.errors)
            .map((field: string) => this.getMessage(field, (<ValidationErrors>this.control.errors)[field]));
    }

    private getMessage(type: string, params: ValidationErrors): string {
        return ValidationErrorsComponent.errorMessages[type](params);
    }
}

interface ValidationMessages {
    [field: string]: (params: ValidationErrors) => string;
}
