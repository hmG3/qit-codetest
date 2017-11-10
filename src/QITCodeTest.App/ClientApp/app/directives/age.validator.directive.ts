import { Directive } from '@angular/core';
import { NG_VALIDATORS, FormControl, Validator, ValidationErrors } from '@angular/forms';

import { AgePipe } from '../pipes/age.pipe'

@Directive({
    selector: '[age]',
    providers: [{ provide: NG_VALIDATORS, useExisting: AgeValidatorDirective, multi: true }]
})
export class AgeValidatorDirective implements Validator {
    constructor() { }

    validate(c: FormControl): ValidationErrors | null {
        var ageValue = new AgePipe().transform(c.value);
        const isValidAge = ageValue <= 80;
        const message = {
            'dob': {
                'message': 'The birthday must be not more than 80 years of age'
            }
        };
        return isValidAge ? null : message;
    }
}
