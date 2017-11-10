import { Directive } from '@angular/core';
import { NG_VALIDATORS, FormControl, Validator, ValidationErrors } from '@angular/forms';

@Directive({
    selector: '[gpa]',
    providers: [{ provide: NG_VALIDATORS, useExisting: GPAValidatorDirective, multi: true }]
})
export class GPAValidatorDirective implements Validator {
    validate(c: FormControl): ValidationErrors | null {
        if (!c.value) {
            return null;
        }

        const numberValue = Number(c.value);
        const minGPA = 1;
        const maxGPA = 5;
        const isValidGPA = numberValue >= minGPA && numberValue <= maxGPA;
        const message = {
            'gpa': {
                'message': `Grade Point Average must be a valid number between ${minGPA.toFixed(1)} and ${maxGPA.toFixed(1)}`
            }
        };
        return isValidGPA ? null : message;
    }
}
