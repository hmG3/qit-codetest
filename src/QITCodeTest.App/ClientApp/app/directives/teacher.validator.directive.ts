import { Directive } from '@angular/core';
import { NG_VALIDATORS, FormControl, Validator, ValidationErrors } from '@angular/forms';

@Directive({
    selector: '[teacher]',
    providers: [{ provide: NG_VALIDATORS, useExisting: TeacherValidatorDirective, multi: true }]
})
export class TeacherValidatorDirective implements Validator {
    validate(c: FormControl): ValidationErrors | null {
        const nameValue = String(c.value);
        const isTeacherValid = nameValue.startsWith('Mr') || nameValue.startsWith('Ms');
        const message = {
            'teacher': {
                'message': 'Teacher Name should starts with salutation like Mr. or Ms.'
            }
        };
        return isTeacherValid ? null : message;
    }
}
